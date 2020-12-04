<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\Auth\LoginRequest;
use App\Http\Requests\Auth\RecoverRequest;
use App\Http\Requests\Auth\RegisterRequest;
use App\Http\Requests\Auth\ResetRequest;
use App\Http\Requests\Auth\VerifyRequest;
use App\Models\User;
use App\Notifications\ResetPassword;
use App\Notifications\UserRegister;
use Carbon\Carbon;
use DB;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use stdClass;
use Str;

class AuthController extends Controller
{
    /**
     * Create a new AuthController instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('register', ['only' => ['register']]);
        $this->middleware('auth:api', ['except' => ['login', 'register', 'recover', 'reset', 'verify']]);
        $this->middleware('demo', ['only' => ['register', 'recover', 'reset']]);
    }

    /**
     * Get a JWT via given credentials.
     *
     * @param  LoginRequest  $request
     * @return JsonResponse
     */
    public function login(LoginRequest $request)
    {
        $request->validated();
        if (!$token = auth('api')->attempt(['email' => $request->get('email'), 'password' => $request->get('password')])) {
            return responder()->error(__('These credentials do not match our records, or the user is disabled'))->respond(406);
        } elseif (auth('api')->user()->status === 0) {
            return responder()->error(__('The user is deactivated, contact the administrator'))->respond(406);
        }
        return $this->respondWithToken($token);
    }

    /**
     * Get the token array structure.
     *
     * @param $token
     * @return JsonResponse
     */
    protected function respondWithToken($token)
    {
        return responder()->success([
            'access_token' => $token,
            'token_type' => 'bearer',
            'expires_in' => auth('api')->factory()->getTTL() * 60,
        ])->respond();
    }

    /**
     * Logout the user out (Invalidate the token).
     *
     * @return JsonResponse
     */
    public function logout()
    {
        auth('api')->logout();
        // Send response
        return responder()->success(['message' => __('Session closed successfully')])->respond();
    }

    /**
     * Get a JWT via given credentials.
     *
     * @param  RegisterRequest  $request
     * @return JsonResponse
     */
    public function register(RegisterRequest $request)
    {
        $request->validated();
        // Create new user
        $user = new User();
        $user->name = $request->get('name');
        $user->email = $request->get('email');
        $user->password = bcrypt($request->get('password'));
        $user->role_id = setting('default_role');
        $user->save();
        // Make email verification token
        $token = Str::random(60);
        // Save token in database
        DB::table('email_verifiers')->where('email', $user->email)->delete();
        DB::table('email_verifiers')->insert(['email' => $user->email, 'token' => $token, 'expires_at' => Carbon::now()->addMinutes(60), 'created_at' => Carbon::now()]);
        // Notification data object
        $objNotificationData = new stdClass();
        $objNotificationData->token = $token;
        $objNotificationData->user = $user;
        // Send reset password notification
        $user->notify((new UserRegister($objNotificationData))->locale(setting('app_locale')));
        // Send response
        return responder()->success($user)->respond();
    }

    /**
     * Send an email with a link to reset the user's password.
     *
     * @param  RecoverRequest  $request
     * @return JsonResponse
     */
    public function recover(RecoverRequest $request)
    {
        $request->validated();
        // Check if user exists
        /** @var User $user */
        $user = User::where('email', $request->get('email'))->first();
        if (!$user) {
            return responder()->error(__('The user is not correct'))->respond(406);
        }
        // Create a new token to be sent to the user.
        $token = Str::random(60);
        // Save token in database
        DB::table('password_resets')->where('email', $request->get('email'))->delete();
        DB::table('password_resets')->insert(['email' => $request->get('email'), 'token' => $token, 'created_at' => Carbon::now()]);
        // Notification data object
        $objNotificationData = new stdClass();
        $objNotificationData->token = $token;
        $objNotificationData->user = $user;
        // Send reset password notification
        $user->notify((new ResetPassword($objNotificationData))->locale(setting('app_locale')));
        // Send response
        return responder()->success(['message' => __('An email has been sent with a link to reset the password')])->respond();
    }

    /**
     * Change the password of the user with the code sent by email.
     *
     * @param  ResetRequest  $request
     * @return JsonResponse
     */
    public function reset(ResetRequest $request)
    {
        $request->validated();
        // Get token
        $tokenData = DB::table('password_resets')->where('token', $request->get('token'))->first();
        if ($tokenData) {
            // Get user
            /** @var User $user */
            $user = User::where('email', $tokenData->email)->first();
            if (!$user) {
                return responder()->error(__('The user is not correct'))->respond(406);
            }
            // Update password
            $user->password = bcrypt($request->get('password'));
            // Email verified
            if (is_null($user->email_verified_at)) {
                $user->email_verified_at = Carbon::now();
            }
            $user->save();
            // Delete token
            DB::table('password_resets')->where('email', $user->email)->delete();
            // Login user
            auth('api')->login($user);
            // Send response
            return $this->respondWithToken(auth('api')->refresh());
        } else {
            return responder()->error(__('The recovery token is incorrect or has already been used'))->respond(406);
        }
    }

    /**
     * Verify the user's email with the code sent by email.
     *
     * @param  VerifyRequest  $request
     * @return JsonResponse
     */
    public function verify(VerifyRequest $request)
    {
        $request->validated();
        // Get token
        $tokenData = DB::table('email_verifiers')->where('token', $request->get('token'))->first();
        if ($tokenData) {
            if (Carbon::now() > Carbon::create($tokenData->expires_at)) {
                return responder()->error(__('Verification code has expired'))->respond(406);
            }
            /** @var User $user */
            $user = User::where('email', $tokenData->email)->first();
            if (!$user) {
                return responder()->error(__('The user is not correct'))->respond(406);
            }
            if (!password_verify($request->get('password'), $user->password)) {
                return responder()->error(__('The entered password is incorrect'))->respond(406);
            }
            $user->email_verified_at = Carbon::now();
            $user->save();
            // Delete token
            DB::table('email_verifiers')->where('email', $user->email)->delete();
            // Login user
            auth('api')->login($user);
            // Send response
            return $this->respondWithToken(auth('api')->refresh());
        } else {
            return responder()->error(__('The verification token is incorrect or has already been used'))->respond(403);
        }
    }

    /**
     * Get the authenticated User.
     *
     * @return JsonResponse
     */
    public function user()
    {
        return responder()->success(auth('api')->user())->respond();
    }

    /**
     * Refresh a token.
     *
     * @return JsonResponse
     */
    public function refresh()
    {
        return $this->respondWithToken(auth('api')->refresh());
    }

    /**
     * Check if you have access permission.
     *
     * @param  Request  $request
     * @return JsonResponse
     */
    public function check(Request $request)
    {
        $access = auth('api')->user() ? true : false;
        if ($request->get('controller')) {
            /** @var User $user */
            $user = auth('api')->user();
            $access = $user->role->checkPermission($request->get('controller'));
        }
        return responder()->success(['access' => $access])->respond();
    }
}
