<!DOCTYPE html>
<html lang="{{ str_replace('_', '-', app()->getLocale()) }}">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <title>@lang('Service unavailable') | {{ config('app.name', 'Laravel') }}</title>
    <link rel="shortcut icon" href="{{ Custom::getIcon() }}">
    <link href="{{ mix('css/dashboard.css') }}" rel="stylesheet">
    <style>
        .btn {
            -moz-transition: none;
            -webkit-transition: none;
            -o-transition: none;
            transition: none;
        }
    </style>
</head>
<body>
<div class="layout">
    <div class="container">
        <div class="row full-height align-items-center">
            <div class="col-md-7">
                <div class="m-t-15 m-l-20">
                    <h1 class="font-size-55 text-semibold">500</h1>
                    <h2 class="font-weight-light font-size-35">@lang('Service unavailable')</h2>
                    <p class="width-70 text-opacity m-t-25 font-size-16">@lang('We are working so that you can access as soon as possible, try to reload the page').</p>
                    <div class="m-t-15">
                        <a href="{{ url()->current() }}" class="btn btn-primary btn-lg m-t-30">
                            @lang('Reload')
                        </a>
                    </div>
                </div>
            </div>
            <div class="col-md-5 d-none d-md-block">
                <img class="img-fluid" src="{{ asset('img/graphics/server.svg') }}" alt="@lang('Error')">
            </div>
        </div>
    </div>
</div>
</body>
</html>
