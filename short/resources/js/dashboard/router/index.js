// Includes
import Vue from 'vue';
import NProgress from 'nprogress';
import VueRouter from 'vue-router';
// Notify to vue to use vue-router
Vue.use(VueRouter);
// Define dashboard routes
const routes = [
    // Auth pages
    {
        path: '/auth', component: require('./views/Auth').default, redirect: '/auth/login',
        children: [
            {path: 'login', component: require('./views/auth/Login').default, meta: {middleware: 'guest'}},
            {path: 'register', component: require('./views/auth/Register').default, meta: {middleware: ['guest', 'register']}},
            {path: 'recover', component: require('./views/auth/Recover').default, meta: {middleware: 'guest'}},
            {path: 'reset/:token', component: require('./views/auth/Reset').default, meta: {middleware: 'guest'}},
            {path: 'verify/:token', component: require('./views/auth/Verify').default},
        ]
    },
    // Dashboard pages
    {
        path: '/dashboard', component: require('./views/Dashboard').default, redirect: '/dashboard/home',
        children: [
            {path: 'home', component: require('./views/dashboard/Home').default, meta: {middleware: 'auth'}},
            {
                path: 'links', component: require('./views/dashboard/Links').default, meta: {middleware: 'auth', controller: 'LinkController'},
                children: [
                    {path: 'edit/:id', component: require('./views/dashboard/links/Edit').default, meta: {middleware: 'auth', controller: 'LinkController'}},
                ]
            },
            {path: 'links/analytics/:id', component: require('./views/dashboard/links/Analytics').default, meta: {middleware: 'auth', controller: 'LinkController'}},
            {path: 'account', component: require('./views/dashboard/Account').default, meta: {middleware: 'auth'}},
            // Admin pages
            {path: 'admin/analytics', component: require('./views/dashboard/admin/Analytics').default, meta: {middleware: 'auth', controller: 'AnalyticsController'}},
            {
                path: 'admin/links', component: require('./views/dashboard/admin/Links').default, meta: {middleware: 'auth', controller: 'AdminLinkController'},
                children: [
                    {path: 'edit/:id', component: require('./views/dashboard/admin/links/Edit').default, meta: {middleware: 'auth', controller: 'AdminLinkController'}},
                ]
            },
            {path: 'admin/links/analytics/:id', component: require('./views/dashboard/admin/links/Analytics').default, meta: {middleware: 'auth', controller: 'AdminLinkController'}},
            {
                path: 'admin/domains', component: require('./views/dashboard/admin/Domains').default, meta: {middleware: 'auth', controller: 'AdminDomainController'},
                children: [
                    {path: 'add', component: require('./views/dashboard/admin/domains/Add').default, meta: {middleware: 'auth', controller: 'AdminDomainController'}},
                    {path: 'edit/:id', component: require('./views/dashboard/admin/domains/Edit').default, meta: {middleware: 'auth', controller: 'AdminDomainController'}},
                ]
            },
            {
                path: 'admin/users', component: require('./views/dashboard/admin/Users').default, meta: {middleware: 'auth', controller: 'UserController'},
                children: [
                    {path: 'add', component: require('./views/dashboard/admin/users/Add').default, meta: {middleware: 'auth', controller: 'UserController'}},
                    {path: 'edit/:id', component: require('./views/dashboard/admin/users/Edit').default, meta: {middleware: 'auth', controller: 'UserController'}},
                ]
            },
            {
                path: 'admin/roles', component: require('./views/dashboard/admin/Roles').default, meta: {middleware: 'auth', controller: 'RoleController'},
                children: [
                    {path: 'add', component: require('./views/dashboard/admin/roles/Add').default, meta: {middleware: 'auth', controller: 'RoleController'}},
                    {path: 'edit/:id', component: require('./views/dashboard/admin/roles/Edit').default, meta: {middleware: 'auth', controller: 'RoleController'}},
                ]
            },
            {
                path: 'admin/settings',
                component: require('./views/dashboard/admin/Settings').default,
                redirect: '/dashboard/admin/settings/general',
                meta: {middleware: 'auth', controller: 'SettingController'},
                children: [
                    {path: 'general', component: require('./views/dashboard/admin/settings/General').default, meta: {middleware: 'auth', controller: 'SettingController'}},
                    {path: 'appearance', component: require('./views/dashboard/admin/settings/Appearance').default, meta: {middleware: 'auth', controller: 'SettingController'}},
                    {path: 'localization', component: require('./views/dashboard/admin/settings/Localization').default, meta: {middleware: 'auth', controller: 'SettingController'}},
                    {path: 'authentication', component: require('./views/dashboard/admin/settings/Authentication').default, meta: {middleware: 'auth', controller: 'SettingController'}},
                    {path: 'mail', component: require('./views/dashboard/admin/settings/Mail').default, meta: {middleware: 'auth', controller: 'SettingController'}},
                ]
            },
            {
                path: 'admin/translations', component: require('./views/dashboard/admin/Translations').default, meta: {middleware: 'auth', controller: 'LanguageController'},
                children: [
                    {path: 'add', component: require('./views/dashboard/admin/translations/NewLocalization').default, meta: {middleware: 'auth', controller: 'LanguageController'}},
                ]
            },
        ]
    },
    // Error pages
    {path: '/*', component: require('./views/error/404').default},
];

// Set the routes in vue-router
const router = new VueRouter({
    routes,
    linkActiveClass: "active",
    mode: "history"
});

// Router auth middleware
router.beforeResolve((to, from, next) => {
    if (to.meta.middleware || to.meta.controller) {
        NProgress.start();
        if (!localStorage.getItem('token') && to.meta.middleware.includes('auth')) {
            next('/auth/login');
        } else if (localStorage.getItem('token') && to.meta.middleware.includes('guest')) {
            next('/dashboard/home');
        } else if (!window.config.register && to.meta.middleware.includes('register')) {
            next('/auth/login');
        } else if (localStorage.getItem('token') && to.meta.middleware.includes('auth')) {
            axios.post('api/auth/check', {controller: to.meta.controller}).then(function (response) {
                response.data.data.access ? next() : to.meta.controller ? next('/dashboard/home') : next('/auth/login');
            });
        } else {
            next();
        }
    } else {
        next();
    }
});

router.afterEach((to, from) => {
    NProgress.done();
});

// Export router
export default router;
