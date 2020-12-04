<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\User\StoreRequest;
use App\Http\Requests\User\UpdateRequest;
use App\Models\Role;
use App\Models\User;
use Exception;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;

class UserController extends Controller
{
    /**
     * Create a new instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('auth:api');
        $this->middleware('demo', ['only' => ['store', 'update', 'destroy']]);
    }

    /**
     * Display a listing of the resource.
     *
     * @param  Request  $request
     * @return JsonResponse
     */
    public function index(Request $request)
    {
        return responder()->success(User::filter($request->all())->paginate($request->get('perPage')))->respond();
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  StoreRequest  $request
     * @return JsonResponse
     */
    public function store(StoreRequest $request)
    {
        $request->validated();
        $user = new User();
        $user->name = $request->get('name');
        $user->surname = $request->get('surname');
        $user->email = $request->get('email');
        $user->status = $request->get('status');
        $user->role_id = $request->get('role');
        $user->password = bcrypt($request->get('password'));
        if ($user->save()) {
            return responder()->success(['message' => __('Data added correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Display the specified resource.
     *
     * @param  User  $user
     * @return JsonResponse
     */
    public function show(User $user)
    {
        if ($user->id === auth('api')->id()) {
            return responder()->error(__('Can not edit your user from the user manager, go to your account page'))->respond(406);
        } else {
            return responder()->success($user)->respond();
        }
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  UpdateRequest  $request
     * @param  User  $user
     * @return JsonResponse
     */
    public function update(UpdateRequest $request, User $user)
    {
        $request->validated();
        $user->name = $request->get('name');
        $user->surname = $request->get('surname');
        $user->email = $request->get('email');
        $user->status = $request->get('status');
        $user->role_id = $request->get('role');
        if ($user->save()) {
            return responder()->success(['message' => __('Data updated correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  User  $user
     * @return JsonResponse
     * @throws Exception
     */
    public function destroy(User $user)
    {
        if ($user->id === auth('api')->id()) {
            return responder()->error(__('You can not delete your own user'))->respond(406);
        } else {
            $user->delete();
            return responder()->success(['message' => __('The data has been successfully deleted')])->respond();
        }
    }

    /**
     * Ger roles list.
     *
     * @return JsonResponse
     */
    public function roles()
    {
        return responder()->success(Role::all())->respond();
    }
}
