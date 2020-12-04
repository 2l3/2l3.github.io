const mix = require('laravel-mix');
// Compile dashboard assets
mix.js('resources/js/dashboard/app.js', 'public/js/dashboard.js').sourceMaps();
mix.sass('resources/sass/dashboard/app.scss', 'public/css/dashboard.css');
// Compile frontend assets
mix.js('resources/js/frontend/app.js', 'public/js/frontend.js').sourceMaps();
mix.sass('resources/sass/frontend/app.scss', 'public/css/frontend.css');
// Mix version
mix.version();
// Disable notifications
mix.disableNotifications();
