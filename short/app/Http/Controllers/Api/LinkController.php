<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\Link\StoreRequest;
use App\Http\Requests\Link\UpdateRequest;
use App\Jobs\LinkMetaData;
use App\Models\Link;
use App\Models\LinkVisit;
use App\Support\Analytics;
use App\Support\Chart;
use App\Transformers\LinkTransformer;
use Carbon\Carbon;
use Carbon\CarbonPeriod;
use Exception;
use File;
use GuzzleHttp\Client;
use Illuminate\Contracts\Filesystem\FileNotFoundException as IlluminateFileNotFoundException;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use Storage;
use Str;

class LinkController extends Controller
{
    /**
     * Create a new AuthController instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('auth:api')->except(['store']);
    }

    /**
     * Display a listing of the resource.
     *
     * @param  Request  $request
     * @return JsonResponse
     */
    public function index(Request $request): JsonResponse
    {
        return responder()->success(Link::filter($request->all())->where('user_id', auth('api')->id())->orderByDesc('id')->paginate($request->get('perPage')))->respond();
    }

    /**
     * Store a newly created resource in storage.
     *
     * @param  StoreRequest  $request
     * @return JsonResponse
     */
    public function store(StoreRequest $request): JsonResponse
    {
        $request->validated();
        $link = new Link();
        $link->code = !empty($request->get('code')) ? $request->get('code') : Str::random(5);
        $link->url = $request->get('url');
        $link->type = $request->has('type') ? $request->get('type') : 1;
        $link->title = $request->get('title');
        $link->tags = json_encode($request->get('tags'));
        $link->description = $request->get('description');
        $link->password = $request->get('password');
        $link->archived = $request->has('archived') ? $request->get('archived') : false;
        $link->disabled = $request->has('disabled') ? $request->get('disabled') : false;
        $link->domain_id = $request->get('domain_id');
        $link->expires_at = !empty($request->get('expires_at')) ? Carbon::create($request->get('expires_at')) : null;
        if (auth('api')->check()) {
            $link->user_id = auth('api')->id();
        }
        if ($link->save()) {
            $this->dispatch(new LinkMetaData($link));
            return responder()->success(['message' => __('Link shortened correctly'), 'link' => (new LinkTransformer())->transform($link)])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Display the specified resource.
     *
     * @param  Link  $link
     * @return JsonResponse
     */
    public function show(Link $link): JsonResponse
    {
        if ($link->user_id != auth('api')->id()) {
            return responder()->error(__('Not found'))->respond(404);
        } else {
            return responder()->success($link)->respond();
        }
    }

    /**
     * Display analytics to the specified resource.
     *
     * @param  Link  $link
     * @param  Request  $request
     * @return JsonResponse
     */
    public function analytics(Link $link, Request $request): JsonResponse
    {
        if ($link->user_id != auth('api')->id()) {
            return responder()->error(__('Not found'))->respond(404);
        } else {
            $reports = [
                'link' => [],
                'platforms' => [],
                'devices' => [],
                'browsers' => [],
                'locations' => [],
                'referrers' => [],
                'clicks' => [],
            ];
            $reports['link'] = (new LinkTransformer())->transform($link);
            // Range
            if ($request->get('range') == 'yearly') {
                $start = Carbon::now()->subYear()->startOfYear();
                $end = Carbon::now()->endOfYear();
            } else {
                if ($request->get('range') == 'monthly') {
                    $start = Carbon::now()->subMonth()->startOfMonth();
                    $end = Carbon::now()->endOfMonth();
                } else {
                    $start = Carbon::now()->subWeek()->startOfWeek();
                    $end = Carbon::now()->endOfWeek();
                }
            }
            // Platform report
            $platformLinkVisits = (new Analytics('platform', $start, $end))->link($link->id)->notNull()->get();
            foreach ($platformLinkVisits as $platformLinkVisit) {
                $reports['platforms'][] = [
                    'code' => $platformLinkVisit['platform'],
                    'label' => Chart::platformName($platformLinkVisit['platform']),
                    'value' => $platformLinkVisit['count'],
                    'color' => Chart::platformColor($platformLinkVisit['platform'])
                ];
            }
            // Device report
            $deviceLinkVisits = (new Analytics('device', $start, $end))->link($link->id)->notNull()->get();
            foreach ($deviceLinkVisits as $deviceLinkVisit) {
                $reports['devices'][] = [
                    'label' => Str::ucfirst($deviceLinkVisit['device']),
                    'value' => $deviceLinkVisit['count']
                ];
            }
            // Browser report
            $browserLinkVisits = (new Analytics('browser', $start, $end))->link($link->id)->notNull()->get();
            foreach ($browserLinkVisits as $browserLinkVisit) {
                $reports['browsers'][] = [
                    'code' => Chart::browserCode($browserLinkVisit['browser']),
                    'label' => Chart::browserName($browserLinkVisit['browser']),
                    'value' => $browserLinkVisit['count'],
                    'color' => Chart::browserColor($browserLinkVisit['browser'])
                ];
            }
            // Location report
            $locationLinkVisits = (new Analytics('location', $start, $end))->link($link->id)->notNull()->get();
            foreach ($locationLinkVisits as $locationLinkVisit) {
                $reports['locations'][Str::upper($locationLinkVisit['location'])] = $locationLinkVisit['count'];
            }
            // Referrer report
            $referrerLinkVisits = (new Analytics('referrer', $start, $end))->link($link->id)->limit(20)->get();
            foreach ($referrerLinkVisits as $referrerLinkVisit) {
                $reports['referrers'][] = [
                    'code' => $referrerLinkVisit['referrer'],
                    'label' => Chart::referrerLabel($referrerLinkVisit['referrer']),
                    'value' => $referrerLinkVisit['count']
                ];
            }
            // Clicks by day report
            if ($request->get('range') == 'yearly') {
                for ($i = 1; $i <= 12; $i++) {
                    $reports['clicks'][] = [
                        'date' => Carbon::createFromFormat('m', $i)->startOfMonth()->toISOString(),
                        'clicks' => LinkVisit::where('link_id', $link->id)->whereMonth('created_at', $i)->count(),
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
                        'clicks' => LinkVisit::where('link_id', $link->id)->whereDay('created_at', $day)->count(),
                        'color' => '#3a1143'
                    ];
                }
            }
            return responder()->success($reports)->respond();
        }
    }

    /**
     * Display preview
     *
     * @param  Link  $link
     * @return JsonResponse
     * @throws IlluminateFileNotFoundException
     * @throws Exception
     */
    public function preview(Link $link): JsonResponse
    {
        if ($link->user_id != auth('api')->id()) {
            return responder()->error(__('Not found'))->respond(404);
        } else {
            $preview = 'links/'.md5($link->id);
            $generated = Storage::disk('private')->exists($preview);
            if ($generated && Carbon::now()->diffInDays(Carbon::create(date('c', Storage::disk('private')->lastModified($preview)))) >= 30) {
                $generated = false;
            }
            if (!$generated) {
                try {
                    $client = new Client();
                    $response = $client->get('https://www.googleapis.com/pagespeedonline/v5/runPagespeed?url='.$link->url.'&screenshot=true');
                    $content = json_decode($response->getBody()->getContents(), true);
                    if (isset($content['lighthouseResult']['audits']['final-screenshot']['details']['data'])) {
                        $image = explode(',', $content['lighthouseResult']['audits']['final-screenshot']['details']['data']);
                        $data = $image[1];
                        $data = str_replace(['_', '-'], ['/', '+'], $data);
                        $data = base64_decode($data);
                        Storage::disk('private')->put($preview, $data);
                        return responder()->success([
                            'mime' => Storage::disk('private')->getMimetype($preview),
                            'image' => base64_encode($data)
                        ])->respond();
                    }
                } catch (Exception $e) {
                    return responder()->success([
                        'mime' => File::mimeType('../public/img/graphics/default.png'),
                        'image' => base64_encode(File::get('../public/img/graphics/default.png'))
                    ])->respond();
                }
            } else {
                return responder()->success([
                    'mime' => Storage::disk('private')->getMimetype($preview),
                    'image' => base64_encode(Storage::disk('private')->get($preview))
                ])->respond();
            }
            return responder()->success([
                'mime' => File::mimeType('../public/img/graphics/default.png'),
                'image' => base64_encode(File::get('../public/img/graphics/default.png'))
            ])->respond();
        }
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  UpdateRequest  $request
     * @param  Link  $link
     * @return JsonResponse
     */
    public function update(UpdateRequest $request, Link $link): JsonResponse
    {
        if ($link->user_id != auth('api')->id()) {
            return responder()->error(__('Not found'))->respond(404);
        } else {
            $request->validated();
            $link->type = $request->has('type') ? $request->get('type') : 1;
            $link->title = $request->get('title');
            $link->tags = json_encode($request->get('tags'));
            $link->description = $request->get('description');
            $link->password = $request->get('password');
            $link->archived = $request->has('archived') ? $request->get('archived') : false;
            $link->disabled = $request->has('disabled') ? $request->get('disabled') : false;
            $link->domain_id = $request->get('domain_id');
            $link->expires_at = !empty($request->get('expires_at')) ? Carbon::create($request->get('expires_at')) : null;
            if ($link->save()) {
                return responder()->success(['message' => __('Data updated correctly')])->respond();
            } else {
                return responder()->error(__('An error occurred while saving the data'))->respond(500);
            }
        }
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  Link  $link
     * @return JsonResponse
     * @throws Exception
     */
    public function destroy(Link $link): JsonResponse
    {
        if ($link->user_id != auth('api')->id()) {
            return responder()->error(__('Not found'))->respond(404);
        } else {
            LinkVisit::where('link_id', $link->id)->delete();
            $link->delete();
            return responder()->success(['message' => __('The data has been successfully deleted')])->respond();
        }
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
