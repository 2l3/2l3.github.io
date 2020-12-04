<?php

Route::group(['namespace' => 'Api'], function () {
    // Homepage routes
    Route::post('shorten', 'HomeController@shorten');

    // Translations routes
    Route::get('lang', 'HomeController@listLanguages');
    Route::get('lang/{lang}', 'HomeController@getLanguageStrings');

    // Authentication routes
    Route::group(['prefix' => 'auth'], function ($router) {
        Route::post('login', 'AuthController@login');
        Route::post('register', 'AuthController@register');
        Route::post('recover', 'AuthController@recover');
        Route::post('reset', 'AuthController@reset');
        Route::post('verify', 'AuthController@verify');
        Route::post('logout', 'AuthController@logout');
        Route::post('user', 'AuthController@user');
        Route::post('check', 'AuthController@check');
    });

    // Account routes
    Route::group(['prefix' => 'account'], function () {
        Route::post('details', 'AccountController@details')->name('account.details');
        Route::post('verification', 'AccountController@verification')->name('account.verification');
        Route::post('password', 'AccountController@password')->name('account.password');
        Route::post('avatar', 'AccountController@avatar')->name('account.avatar');
    });

    // Analytics
    Route::get('analytics/report', 'DashboardController@analytics')->name('analytics.report');
    // Dashboard Routes
    Route::group(['prefix' => 'dashboard'], function () {
        // Link manager
        Route::get('links/domains', 'LinkController@domains')->name('links.domains');
        Route::get('links/{link}/analytics', 'LinkController@analytics')->name('links.analytics');
        Route::get('links/{link}/preview', 'LinkController@preview')->name('links.preview');
        Route::apiResource('links', 'LinkController');
        // Admin analytics
        Route::get('admin/analytics/report', 'AdminAnalyticsController@analytics')->name('admin.analytics');
        // Admin link manager
        Route::get('admin/links/domains', 'AdminLinkController@domains')->name('admin.links.domains');
        Route::get('admin/links/{link}/analytics', 'AdminLinkController@analytics')->name('admin.links.analytics');
        Route::get('admin/links/{link}/preview', 'AdminLinkController@preview')->name('admin.links.preview');
        Route::apiResource('admin/links', 'AdminLinkController')->names('admin.links')->except('store');
        // Admin domains manager
        Route::apiResource('admin/domains', 'AdminDomainController');
        // Users manager
        Route::get('users/roles', 'UserController@roles')->name('users.roles');
        Route::apiResource('users', 'UserController');
        // Roles manager
        Route::get('roles/permissions', 'RoleController@permissions')->name('roles.permissions');
        Route::apiResource('roles', 'RoleController');
        // Settings manager
        Route::get('settings/roles', 'SettingController@roles')->name('settings.roles');
        Route::get('settings/domains', 'SettingController@domains')->name('settings.domains');
        Route::get('settings/permissions', 'SettingController@permissions')->name('settings.permissions');
        Route::apiResource('settings', 'SettingController')->only(['index', 'store']);
        // Language manager
        Route::post('language/sync', 'LanguageController@sync')->name('lang.sync');
        Route::apiResource('language', 'LanguageController');
    });
});
