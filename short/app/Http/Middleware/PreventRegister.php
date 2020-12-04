<?php

namespace App\Http\Middleware;

use Closure;
use Illuminate\Http\JsonResponse;

class PreventRegister
{
    /**
     * Handle an incoming request.
     *
     * @param $request
     * @param  Closure  $next
     * @return JsonResponse|mixed
     */
    public function handle($request, Closure $next)
    {
        if (!e(setting('app_register') ? true : false)) {
            return responder()->error(__('Forbidden'))->respond(403);
        }
        return $next($request);
    }
}
