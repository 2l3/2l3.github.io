<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\Language\StoreRequest;
use App\Http\Requests\Language\UpdateRequest;
use App\Models\Language;
use Artisan;
use Exception;
use File;
use Illuminate\Contracts\Filesystem\FileNotFoundException;
use Illuminate\Http\JsonResponse;

class LanguageController extends Controller
{
    /**
     * LanguageController constructor.
     */
    public function __construct()
    {
        $this->middleware('auth:api');
        $this->middleware('demo', ['only' => ['store', 'update', 'destroy', 'sync']]);
    }

    /**
     * Display a listing of the resource.
     *
     * @return JsonResponse
     */
    public function index()
    {
        return responder()->success(Language::all())->respond();
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
        $lang = new Language();
        $lang->locale = $request->get('locale');
        $lang->name = $request->get('name');
        if ($lang->save()) {
            Artisan::call('translatable:export', ['lang' => $lang->locale]);
            return responder()->success(['message' => __('Data added correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Display the specified resource.
     *
     * @param  Language  $language
     * @return JsonResponse
     * @throws FileNotFoundException
     */
    public function show(Language $language)
    {
        if (file_exists(base_path().'/resources/lang/'.$language->locale.'.json')) {
            $data = [];
            foreach (json_decode(File::get(base_path().'/resources/lang/'.$language->locale.'.json'), true) as $key => $value) {
                $data[] = [
                    'key' => $key,
                    'value' => $value,
                ];
            }
            return responder()->success($data)->respond();
        } else {
            return responder()->error(__('No translations found for the selected language'))->respond(500);
        }
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  UpdateRequest  $request
     * @param  Language  $language
     * @return JsonResponse
     * @throws FileNotFoundException
     */
    public function update(UpdateRequest $request, Language $language)
    {
        $request->validated();
        // Set new name
        $language->name = $request->get('name');
        // Update strings
        $data = json_decode(File::get(base_path().'/resources/lang/'.$language->locale.'.json'), true);
        foreach ($request->get('strings') as $string) {
            $data[$string['key']] = $string['value'];
        }
        // Set new strings & update language name
        if (File::put(base_path().'/resources/lang/'.$language->locale.'.json', json_encode($data)) && $language->save()) {
            return responder()->success(['message' => __('Data updated correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  Language  $language
     * @return JsonResponse
     * @throws Exception
     */
    public function destroy(Language $language)
    {
        if ($language->locale !== config('app.locale')) {
            if (File::delete(base_path().'/resources/lang/'.$language->locale.'.json')) {
                $language->delete();
                return responder()->success(['message' => __('The data has been successfully deleted')])->respond();
            } else {
                return responder()->error(__('The file with the strings could not be removed'))->respond(406);
            }
        } else {
            return responder()->error(__('Can not delete the default language'))->respond(406);
        }
    }

    /**
     * Update all translation files.
     *
     * @return JsonResponse
     */
    public function sync()
    {
        try {
            $languages = Language::all();
            foreach ($languages as $index => $language) {
                @Artisan::call('translatable:export', ['lang' => $language->locale]);
            }
            return responder()->success(['message' => __('Translation files updated correctly')])->respond();
        } catch (Exception $e) {
            return responder()->error($e->getMessage())->respond(500);
        }
    }
}
