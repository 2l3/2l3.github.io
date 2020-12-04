<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\Link\StoreRequest;
use File;
use Illuminate\Http\JsonResponse;

class HomeController extends Controller
{
    /**
     * @param  StoreRequest  $request
     * @return JsonResponse|void
     */
    public function shorten(StoreRequest $request)
    {
        if (!(bool) setting('homepage_enabled')) {
            return abort(404);
        }
        return (new LinkController())->store($request);
    }

    /**
     * Return the available languages list.
     *
     * @return JsonResponse
     */
    public function listLanguages()
    {
        return (new LanguageController())->index();
    }

    /**
     * Get the language strings.
     *
     * @param $locale
     * @return JsonResponse
     */
    public function getLanguageStrings($locale)
    {
        return responder()->success(json_decode(File::get(base_path().'/resources/lang/'.$locale.'.json'), true))->respond();
    }
}
