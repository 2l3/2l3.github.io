// Vendor plugins
require('../plugins');
// Import vue
import Vue from 'vue';
import Vuex from 'vuex';
import Meta from 'vue-meta';
import store from './store';
import router from './router';
import i18n from './language';
import VueSnackbar from 'vue-snack';
import BootstrapVue from 'bootstrap-vue';
import VueClipboard from 'vue-clipboard2';
import Loading from 'vue-loading-overlay';
import ToggleButton from 'vue-js-toggle-button';
import VueTextareaAutosize from 'vue-textarea-autosize'
// Import Sentry
import * as Sentry from '@sentry/browser';
import * as Integrations from '@sentry/integrations';

// Configure Sentry
if (window.config.sentry_dsn) {
    Sentry.init({
        dsn: window.config.sentry_dsn,
        integrations: [new Integrations.Vue({Vue, attachProps: true})],
    });
}

// Vue declarations
Vue.use(Vuex);
Vue.use(Meta);
Vue.use(BootstrapVue);
Vue.use(ToggleButton);
Vue.use(VueClipboard);
Vue.use(VueTextareaAutosize);
Vue.component('loading-animation', Loading);
Vue.use(VueSnackbar, {position: 'bottom', time: 2000});
// Vue components
require('./components');
// Define vue
new Vue({
    el: '#app',
    router,
    store,
    i18n,
    mounted() {
        this.initi18n();
        this.$store.commit('setUser');
        this.$store.commit('setSettings', window.config);
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
