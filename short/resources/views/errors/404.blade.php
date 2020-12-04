<!DOCTYPE html>
<html lang="{{ str_replace('_', '-', app()->getLocale()) }}">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <title>@lang('Page not found') | {{ config('app.name', 'Laravel') }}</title>
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
            <div class="col-12">
                <div class="text-center p-t-50">
                    <h1 class="font-size-170 text-secondary font-weight-light text-opacity ls-2">404</h1>
                    <h2 class="font-weight-light font-size-30"> @lang('Whoops! Looks like you got lost')</h2>
                    <p class="lead"> @lang('We couldnt find what you were looking for').</p>
                    <a href="{{ route('index') }}" class="btn btn-primary btn-lg m-t-30 active">
                        @lang('Go back')
                    </a>
                </div>
            </div>
            <div class="col-12 align-self-end ">
                <div class="text-center p-b-20 font-size-13">
                    @lang('Created with') <i style="color: #ff4c4c" class="fas fa-heart"></i> @lang('by') <a href="https://github.com/dacoto97" target="_blank" rel="nofollow">@dacoto97</a> |
                    @lang('Powered by') <i style="color: #f55247;" class="fab fa-laravel"></i> Laravel
                </div>
            </div>
        </div>
    </div>
</div>
</body>
</html>
