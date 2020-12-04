<?php

// Home route
Route::get('/', 'HomeController@index')->name('index');

// Auth routes
Route::get('/auth/{any}', 'HomeController@auth')
    ->name('auth')
    ->middleware('guest')
    ->where('any', '.*');

// Dashboard SPA
Route::get('/dashboard/{any}', 'HomeController@dashboard')
    ->name('dashboard')
    ->where('any', '.*');

// Link route
Route::any('{code}', 'LinkController@visit')->where('all', '.*');
Route::get('{code}/qr', 'LinkController@qr')->where('all', '.*');
