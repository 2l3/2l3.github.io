<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\Setting\StoreRequest;
use App\Models\Role;
use App\Models\Setting;
use Exception;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;

class SettingController extends Controller
{
    /**
     * Create a new instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('auth:api');
        $this->middleware('demo', ['only' => ['store']]);
    }

    /**
     * Display a listing of the resource.
     *
     * @return JsonResponse
     */
    public function index()
    {
        return responder()->success(Setting::all())->respond();
    }

    /**
     * Store all settings.
     *
     * @param  StoreRequest  $request
     * @return JsonResponse
     */
    public function store(StoreRequest $request)
    {
        $request->validated();
        $message = null;
        try {
            $key = null;
            if ($request->get('type') === 'settings') {
                foreach ($request->get('settings') as $key => $value) {
                    $message = __('Settings saved correctly');
                    if ($key !== 'app_icon' && $key !== 'app_background') {
                        /** @var Setting $settings */
                        if ($settings = Setting::find($key)) {
                            $settings->encodeValue($value);
                            $settings->save();
                        }
                    }
                }
            } elseif ($request->get('type') === 'images') {
                $message = __('Image updated correctly');
                if ($request->get('image') === 'icon') {
                    $key = 'app_icon';
                } elseif ($request->get('image') === 'background') {
                    $key = 'app_background';
                }
                $file = $request->file('file')->store($request->get('image'), 'public');
                /** @var Setting $settings */
                $settings = Setting::find($key);
                $settings->value = $file;
                $settings->save();
            }
            return responder()->success(['message' => $message])->respond();
        } catch (Exception $e) {
            return responder()->error($e->getMessage())->respond(500);
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

    /**
     * Get permissions list.
     *
     * @return JsonResponse
     */
    public function permissions()
    {
        return (new RoleController())->permissions();
    }

    /**
     * List domains
     *
     * @param  Request  $request
     * @return JsonResponse
     */
    public function domains(Request $request): JsonResponse
    {
        return (new DomainController())->index($request);
    }
}
