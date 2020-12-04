<?php

namespace App\Http\Controllers;

use App\Models\Domain;
use App\Models\Link;
use App\Models\LinkVisit;
use App\Support\Visit;
use Carbon\Carbon;
use Exception;
use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\Routing\ResponseFactory;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Http\Response;
use Illuminate\Routing\Redirector;
use Jenssegers\Agent\Agent;
use QrCode;
use Str;

class LinkController extends Controller
{
    /**
     * Save the visit and redirect to the destination
     *
     * @param  Request  $request
     * @param $code
     * @return Application|RedirectResponse|Redirector|void
     * @throws Exception
     */
    public function visit(Request $request, $code)
    {
        /** @var Link $link */
        $link = Link::where('code', $code)->first();
        // Verify domain
        if ($domain = Domain::find(setting('app_domain'))) {
            if ($link->domain_id) {
                if (
                    (parse_url($link->domain->domain)['host'] != parse_url(url('/'))['host']) &&
                    (parse_url($link->domain->domain)['host'] != parse_url($domain->domain)['host'])
                ) {
                    return abort(404);
                }
            } else {
                if (parse_url(url('/'))['host'] != parse_url($domain->domain)['host']) {
                    return abort(404);
                }
            }
        }
        if ($link->domain_id) {
            if (parse_url($link->domain->domain)['host'] != parse_url(url('/'))['host']) {
                return abort(404);
            }
        }
        // Verify disabled
        if ($link->disabled) {
            return abort(404);
        }
        // Verify expiration date
        if ($link->expires_at && (Carbon::now()->greaterThanOrEqualTo($link->expires_at))) {
            return abort(404);
        }
        // Verify password
        if (!empty($link->password) && ($request->post('password') != $link->password)) {
            $error = null;
            if ($request->has('password')) {
                $error = __('Password does not match');
            }
            return view('frontend.short.password', ['link' => $link, 'error' => $error]);
        }
        $agent = new Agent();
        $linkVisit = new LinkVisit();
        $linkVisit->link_id = $link->id;
        $linkVisit->platform = Str::slug(strtolower($agent->platform()), '-');
        $linkVisit->device = Visit::getDevice($agent);
        $linkVisit->browser = strtolower($agent->browser());
        $linkVisit->location = strtolower(geoip($request->ip())['iso_code']);
        $linkVisit->crawler = $agent->isRobot();
        $linkVisit->referrer = Str::contains($request->server('HTTP_REFERER'), url('/')) ? null : $request->server('HTTP_REFERER');
        $linkVisit->ip = $request->ip();
        if ($linkVisit->save()) {
            return redirect($link->url);
        } else {
            throw new Exception('Unable to save visit log');
        }
    }

    /**
     * Return qr code of link
     *
     * @param  Request  $request
     * @param $code
     * @return Application|ResponseFactory|Response|void
     */
    public function qr(Request $request, $code)
    {
        $link = Link::where('code', $code)->get();
        if ($link->count() != 0) {
            /** @var Link $link */
            $link = $link[0];
            // Verify domain
            if ($link->domain_id && (parse_url($link->domain->domain)['host'] != parse_url(url('/'))['host'])) {
                return abort(404);
            } else {
                if (!$link->domain_id && $domain = Domain::find(setting('app_domain'))) {
                    if ((parse_url($domain->domain)['host'] != parse_url(url('/'))['host'])) {
                        return abort(404);
                    }
                }
            }
            // Verify disabled
            if ($link->disabled) {
                return abort(404);
            }
            // Verify expiration date
            if ($link->expires_at && (Carbon::now()->greaterThanOrEqualTo($link->expires_at))) {
                return abort(404);
            }
            // Return QR
            return response(QrCode::format('png')->size((int) $request->has('s') ? $request->get('s') : '150')->generate($link->getLink()))->header('Content-Type', 'image/png');
        }
        return abort(404);
    }
}
