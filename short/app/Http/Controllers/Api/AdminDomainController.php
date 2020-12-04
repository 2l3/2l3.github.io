<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use App\Http\Requests\Domain\StoreRequest;
use App\Http\Requests\Domain\UpdateRequest;
use App\Models\Domain;
use App\Models\Link;
use Exception;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;

class AdminDomainController extends Controller
{
    /**
     * Create a new AuthController instance.
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
    public function index(Request $request): JsonResponse
    {
        return responder()->success(Domain::filter($request->all()))->respond();
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
        $domain = new Domain();
        $domain->domain = $request->get('domain');
        $domain->redirect = $request->get('redirect');
        $domain->status = $request->exists('status') ? $request->get('status') : true;
        if ($domain->save()) {
            return responder()->success(['message' => __('Data added correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Display the specified resource.
     *
     * @param  Domain  $domain
     * @return JsonResponse
     */
    public function show(Domain $domain): JsonResponse
    {
        return responder()->success($domain)->respond();
    }

    /**
     * Update the specified resource in storage.
     *
     * @param  UpdateRequest  $request
     * @param  Domain  $domain
     * @return JsonResponse
     */
    public function update(UpdateRequest $request, Domain $domain): JsonResponse
    {
        $request->validated();
        $domain->domain = $request->get('domain');
        $domain->redirect = $request->get('redirect');
        $domain->status = $request->exists('status') ? $request->get('status') : true;
        if ($domain->save()) {
            return responder()->success(['message' => __('Data updated correctly')])->respond();
        } else {
            return responder()->error(__('An error occurred while saving the data'))->respond(500);
        }
    }

    /**
     * Remove the specified resource from storage.
     *
     * @param  Domain  $domain
     * @return JsonResponse
     * @throws Exception
     */
    public function destroy(Domain $domain): JsonResponse
    {
        Link::where('domain_id', $domain->id)->update(['domain_id' => null]);
        $domain->delete();
        return responder()->success(['message' => __('The data has been successfully deleted')])->respond();
    }
}
