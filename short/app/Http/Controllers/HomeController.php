<?php

namespace App\Http\Controllers;

use Illuminate\Contracts\Foundation\Application;
use Illuminate\Contracts\View\Factory;
use Illuminate\Http\RedirectResponse;
use Illuminate\Routing\Redirector;
use Illuminate\View\View;

class HomeController extends Controller
{
    /**
     * Create a new instance.
     *
     * @return void
     */
    public function __construct()
    {
        $this->middleware('domain');
    }

    /**
     * Show the home page.
     *
     * @return Application|Factory|RedirectResponse|Redirector|View
     */
    public function index()
    {
        if (!setting('homepage_enabled', true)) {
            return redirect()->route('auth', 'login');
        }
        return view('frontend.index');
    }

    /**
     * Show the application auth pages.
     *
     * @return Factory|View
     */
    public function auth()
    {
        return view('dashboard.index');
    }

    /**
     * Show the application auth dashboard.
     *
     * @return Factory|View
     */
    public function dashboard()
    {
        return view('dashboard.index');
    }
}
