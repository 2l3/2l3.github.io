<!doctype html>
<html lang="{{ str_replace('_', '-', app()->getLocale()) }}">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <title>{{ config('app.name', 'Laravel') }}</title>
    <link rel="shortcut icon" href="{{ Custom::getIcon() }}">
    <link href="{{ mix('css/frontend.css') }}" rel="stylesheet">
</head>
<body class="bg-light">
<div id="app">
    <nav class="navbar navbar-light bg-light static-top">
        <div class="container">
            <a class="navbar-brand" href="{{ route('index') }}">
                <img src="{{ Custom::getIcon() }}" alt="{{ config('app.name', 'Laravel') }}" class="img-fluid inline-block pb-1" style="max-height: 25px">
                {{ config('app.name', 'Laravel') }}
            </a>
            <a class="btn btn-primary" href="{{ route('auth', 'login') }}">@lang('Log in')</a>
        </div>
    </nav>
    @yield('content')
    <footer class="footer bg-light">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 h-100 text-center text-lg-left my-auto">
                    <p class="text-muted small mb-4 mb-lg-0">&copy; {{ config('app.name', 'Laravel') }} {{ date('Y') }}. @lang('All Rights Reserved').</p>
                </div>
            </div>
        </div>
    </footer>
</div>
<script>
    window.config = {
        'path': '{{ url('/') }}',
        'timezone': '{{ setting('app_timezone') }}',
        'date_locale': '{{ setting('app_date_locale') }}',
        'date_format': '{{ setting('app_date_format') }}',
        'sentry_dsn': '{{ env('SENTRY_VUE_DSN') }}'
    };
</script>
<script src="{{ mix('js/frontend.js') }}"></script>
</body>
</html>
