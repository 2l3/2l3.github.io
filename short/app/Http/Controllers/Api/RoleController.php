<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\Role\StoreRequest;
use App\Http\Requests\Role\UpdateRequest;
use App\Models\Role;
use App\Models\User;
use Exception;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use Route;
use Str;

class RoleController extends Controller
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
        return responder()->success(Role::filter($request->all())->paginate($request->get('perPage')))->respond();
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
        $permissions = [];
        foreach ($request->get('permissions') as $key => $value) {
            if ($value) {
                $permissions[] = $key;
            }
        }
        $role = new Role();
        $role->name = $request->get('name');
        $role->type = 2;
        $role->permissions = json_encode($permissions);
        if ($role->save()) {
            return responder()->success(['message' => __('Data added correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Display the specified resource.
     *
     * @param  Role  $role
     * @return JsonResponse
     */
    public function show(Role $role)
    {
        if ($role->type === 1) {
            return responder()->error(__('Can not edit a base role of the system'))->respond(406);
        } else {
            return responder()->success($role)->respond();
        }
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  UpdateRequest  $request
     * @param  Role  $role
     * @return JsonResponse
     */
    public function update(UpdateRequest $request, Role $role)
    {
        $request->validated();
        $permissions = [];
        foreach ($request->get('permissions') as $key => $value) {
            if ($value) {
                $permissions[] = $key;
            }
        }
        $role->name = $request->get('name');
        $role->permissions = json_encode($permissions);
        if ($role->save()) {
            return responder()->success(['message' => __('Data updated correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  Role  $role
     * @return JsonResponse
     * @throws Exception
     */
    public function destroy(Role $role)
    {
        if ($role->type === 2 && ($role->id != setting('default_role'))) {
            User::where('role', $role->id)->update(['role' => setting('default_role')]);
            $role->delete();
            return responder()->success(['message' => __('The data has been successfully deleted')])->respond();
        } else {
            return responder()->error(__('Can not delete a default role'))->respond(406);
        }
    }

    /**
     * Get permissions list.
     *
     * @return JsonResponse
     */
    public function permissions()
    {
        $controllers = [];
        $controllersLabels = [];
        foreach (Route::getRoutes()->getIterator() as $route) {
            if (strpos($route->uri, 'api/dashboard') !== false) {
                $path = explode('@', str_replace($route->action['namespace'].'\\', '', $route->action['controller']))[0];
                $controllers[$path] = false;
                $controllersLabels[$path] = 'Manage'.' '.strtolower(implode(' ', preg_split('/(?=[A-Z])/', Str::plural(str_replace('Controller', '', $path)))));
            }
        }
        return responder()->success([
            'controllers' => $controllers,
            'controllerLabels' => $controllersLabels,
        ])->respond();
    }
}
