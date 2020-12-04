<?php

namespace App\Http\Middleware;

use App\Models\User;
use Closure;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;

class CheckPermission
{
    /**
     * Handle an incoming request.
     *
     * @param $request
     * @param  Closure  $next
     * @return JsonResponse|mixed
     */
    public function handle(Request $request, Closure $next)
    {
        /** @var User $user */
        $user = auth('api')->user();
        if (strpos($request->route()->uri, 'api/dashboard') !== false) {
            if (!$user) {
                return responder()->error(__('Unauthorized'))->respond(401);
            } else {
                $path = explode('@', str_replace($request->route()->action['namespace'].'\\', '', $request->route()->action['controller']))[0];
                if (!$user->role->checkPermission($path)) {
                    return responder()->error(__('Forbidden'))->respond(403);
                }
            }
        }
        return $next($request);
    }
}
