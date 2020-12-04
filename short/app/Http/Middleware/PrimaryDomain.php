<?php

namespace App\Http\Middleware;

use App\Models\Domain;
use Closure;
use Illuminate\Http\Request;

class PrimaryDomain
{
    /**
     * Handle an incoming request.
     *
     * @param  Request  $request
     * @param  Closure  $next
     * @return mixed
     */
    public function handle($request, Closure $next)
    {
        $onlyPrimaryDomain = (bool) setting('only_primary_domain');
        if ($onlyPrimaryDomain && $domain = Domain::find(setting('app_domain'))) {
            if (parse_url($domain->domain)['host'] != parse_url(url('/'))['host']) {
                return $domain->redirect ? redirect($domain->redirect) : abort(404);
            }
        }
        return $next($request);
    }
}
