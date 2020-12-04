<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\Account\AvatarRequest;
use App\Http\Requests\Account\DetailsRequest;
use App\Http\Requests\Account\PasswordRequest;
use App\Models\User;
use App\Notifications\EmailVerify;
use App\Transformers\UserTransformer;
use Carbon\Carbon;
use DB;
use Hash;
use Illuminate\Http\JsonResponse;
use stdClass;
use Str;

class AccountController extends Controller
{
    /**
     * Create a new instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('auth:api');
        $this->middleware('demo');
    }

    /**
     * Change user details.
     *
     * @param  DetailsRequest  $request
     * @return JsonResponse
     */
    public function details(DetailsRequest $request)
    {
        $request->validated();
        /** @var User $user */
        $user = auth('api')->user();
        $user->name = $request->get('name');
        $user->surname = $request->get('surname');
        if ($request->get('email') != $user->email) {
            $user->email = $request->get('email');
            $user->email_verified_at = null;
        }
        if ($user->save()) {
            return responder()->success(['message' => __('Data updated correctly'), 'user' => (new UserTransformer())->transform(User::find(auth('api')->id()))])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Changing the user's password.
     *
     * @param  PasswordRequest  $request
     * @return JsonResponse
     */
    public function password(PasswordRequest $request)
    {
        $request->validated();
        // Check current password
        if (!(Hash::check($request->get('current_password'), auth('api')->user()->password))) {
            return responder()->error(__('The password entered does not match the current password'))->respond(406);
        }
        // Check if is a new password
        if (strcmp($request->get('current_password'), $request->get('password')) == 0) {
            return responder()->error(__('The new password can not be the same as the previous one'))->respond(406);
        }
        /** @var User $user */
        $user = auth('api')->user();
        $user->password = bcrypt($request->get('password'));
        if ($user->save()) {
            return responder()->success(['message' => __('Password updated correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Changing the user's avatar.
     *
     * @param  AvatarRequest  $request
     * @return JsonResponse
     */
    public function avatar(AvatarRequest $request)
    {
        $request->validated();
        /** @var User $user */
        $user = auth('api')->user();
        $file = $request->file('file')->store('avatar', 'public');
        $user->avatar = $file;
        if ($user->save()) {
            return responder()->success(['message' => __('Avatar updated correctly')])->respond();
        } else {
            return responder()->error(__('The file could not be uploaded'))->respond(500);
        }
    }

    /**
     * Send user email verification notification
     *
     * @return JsonResponse
     */
    public function verification()
    {
        /** @var User $user */
        $user = auth('api')->user();
        if (is_null($user->email_verified_at)) {
            // Create a new token to be sent to the user.
            $token = Str::random(60);
            // Save token in database
            DB::table('email_verifiers')->where('email', $user->email)->delete();
            DB::table('email_verifiers')->insert(['email' => $user->email, 'token' => $token, 'expires_at' => Carbon::now()->addMinutes(60), 'created_at' => Carbon::now()]);
            // Notification data object
            $objNotificationData = new stdClass();
            $objNotificationData->token = $token;
            $objNotificationData->user = $user;
            // Send email verification notification
            $user->notify((new EmailVerify($objNotificationData))->locale(setting('app_locale')));
            // Send response
            return responder()->success(['message' => __('An email has been sent with a link to verify the email, valid for :minutes minutes', ['minutes' => 60])])->respond();
        } else {
            return responder()->error(__('The email has already been verified'))->respond(406);
        }
    }
}
