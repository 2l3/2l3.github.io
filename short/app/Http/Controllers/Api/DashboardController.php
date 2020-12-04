<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Models\Link;
use App\Models\LinkVisit;
use App\Support\Analytics;
use App\Support\Chart;
use Carbon\Carbon;
use Carbon\CarbonPeriod;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use Str;

class DashboardController extends Controller
{
    /**
     * Create a new AuthController instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('auth:api');
    }

    /**
     * @param  Request  $request
     * @return JsonResponse
     */
    public function analytics(Request $request): JsonResponse
    {
        $reports = [
            'link' => [],
            'platforms' => [],
            'devices' => [],
            'browsers' => [],
            'locations' => [],
            'referrers' => [],
            'clicks' => [],
        ];
        // Range
        if ($request->get('range') == 'yearly') {
            $start = Carbon::now()->startOfYear();
            $end = Carbon::now()->endOfYear();
        } else {
            if ($request->get('range') == 'monthly') {
                $start = Carbon::now()->startOfMonth();
                $end = Carbon::now()->endOfMonth();
            } else {
                $start = Carbon::now()->startOfWeek();
                $end = Carbon::now()->endOfWeek();
            }
        }
        // Platform report
        $platformLinkVisits = (new Analytics('platform', $start, $end))->onlyUser()->notNull()->get();
        foreach ($platformLinkVisits as $platformLinkVisit) {
            $reports['platforms'][] = [
                'code' => $platformLinkVisit['platform'],
                'label' => Chart::platformName($platformLinkVisit['platform']),
                'value' => $platformLinkVisit['count'],
                'color' => Chart::platformColor($platformLinkVisit['platform'])
            ];
        }
        // Device report
        $deviceLinkVisits = (new Analytics('device', $start, $end))->onlyUser()->notNull()->get();
        foreach ($deviceLinkVisits as $deviceLinkVisit) {
            $reports['devices'][] = [
                'label' => Str::ucfirst($deviceLinkVisit['device']),
                'value' => $deviceLinkVisit['count']
            ];
        }
        // Browser report
        $browserLinkVisits = (new Analytics('browser', $start, $end))->onlyUser()->notNull()->get();
        foreach ($browserLinkVisits as $browserLinkVisit) {
            $reports['browsers'][] = [
                'code' => Chart::browserCode($browserLinkVisit['browser']),
                'label' => Chart::browserName($browserLinkVisit['browser']),
                'value' => $browserLinkVisit['count'],
                'color' => Chart::browserColor($browserLinkVisit['browser'])
            ];
        }
        // Location report
        $locationLinkVisits = (new Analytics('location', $start, $end))->onlyUser()->notNull()->get();
        foreach ($locationLinkVisits as $locationLinkVisit) {
            $reports['locations'][Str::upper($locationLinkVisit['location'])] = $locationLinkVisit['count'];
        }
        // Referrer report
        $referrerLinkVisits = (new Analytics('referrer', $start, $end))->onlyUser()->limit(20)->get();
        foreach ($referrerLinkVisits as $referrerLinkVisit) {
            $reports['referrers'][] = [
                'code' => $referrerLinkVisit['referrer'],
                'label' => Chart::referrerLabel($referrerLinkVisit['referrer']),
                'value' => $referrerLinkVisit['count']
            ];
        }
        // Clicks by day report
        $userLinks = Link::where('user_id', auth('api')->id())->pluck('id');
        if ($request->get('range') == 'yearly') {
            for ($i = 1; $i <= 12; $i++) {
                $reports['clicks'][] = [
                    'date' => Carbon::createFromFormat('m', $i)->startOfMonth()->toISOString(),
                    'clicks' => LinkVisit::whereIn('link_id', $userLinks)->whereMonth('created_at', $i)->count(),
                    'color' => '#3a1143'
                ];
            }
        } else {
            if ($request->get('range') == 'monthly') {
                $period = CarbonPeriod::create(Carbon::now()->startOfMonth(), Carbon::now()->endOfMonth());
            } else {
                $period = CarbonPeriod::create(Carbon::now()->startOfWeek(), Carbon::now()->endOfWeek());
            }
            foreach ($period as $day) {
                $reports['clicks'][] = [
                    'date' => $day->toISOString(),
                    'clicks' => LinkVisit::whereIn('link_id', $userLinks)->whereDay('created_at', $day)->count(),
                    'color' => '#3a1143'
                ];
            }
        }
        return responder()->success($reports)->respond();
    }
}
