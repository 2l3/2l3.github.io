// Vendor plugins
require('../plugins');
// Import vue
import Vue from 'vue';
import i18n from '../dashboard/language';
import VueSnackbar from 'vue-snack';
import VueClipboard from 'vue-clipboard2';
import Loading from 'vue-loading-overlay';
// Import Sentry
import * as Sentry from '@sentry/browser';
import * as Integrations from '@sentry/integrations';
// Vue components
import ShortLink from "./components/ShortLink";

// Configure Sentry
if (window.config.sentry_dsn) {
    Sentry.init({
        dsn: window.config.sentry_dsn,
        integrations: [new Integrations.Vue({Vue, attachProps: true})],
    });
}

// Vue declarations
Vue.use(VueClipboard);
Vue.component('loading-animation', Loading);
Vue.use(VueSnackbar, {position: 'bottom', time: 2000});

// Define vue
new Vue({
    el: '#app',
    i18n,
    components: {
        'short-link': ShortLink
    },
    mounted() {
        this.initi18n();
    },
    methods: {
        // Start the translations vue-i18n
        initi18n() {
            this.$i18n.locale = document.documentElement.lang;
            this.loadTranslations();
        },
        // Get translations of a specific language
        loadTranslations() {
            let self = this;
            axios.get('api/lang/' + self.$i18n.locale).then(function (response) {
                self.$i18n.setLocaleMessage(self.$i18n.locale, response.data.data);
            });
        },
    }
});
