<!DOCTYPE html>
<html lang="{{ str_replace('_', '-', app()->getLocale()) }}">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="csrf-token" content="{{ csrf_token() }}">
    <title>{{ config('app.name', 'Laravel') }}</title>
    <link rel="shortcut icon" href="{{ Custom::getIcon() }}">
    <link href="{{ mix('css/dashboard.css') }}" rel="stylesheet">
</head>
<body>
<div id="app">
    <app></app>
</div>
<script>
    window.config = {
        'path': '{{ url('/') }}',
        'name': '{{ config('app.name') }}',
        'description': '{{ setting('app_description') }}',
        'register': {{ setting('app_register') ? 'true' : 'false' }},
        'unverified_user_alert': {{ setting('unverified_user_alert') ? 'true' : 'false' }},
        'logo': '{{ Custom::getIcon() }}',
        'background': '{{ Custom::getBackground() }}',
        'timezone': '{{ setting('app_timezone') }}',
        'date_locale': '{{ setting('app_date_locale') }}',
        'date_format': '{{ setting('app_date_format') }}',
        'sentry_dsn': '{{ env('SENTRY_VUE_DSN') }}'
    };
</script>
<script src="{{ mix('js/dashboard.js') }}"></script>
</body>
</html>
