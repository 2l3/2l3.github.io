// vue & vue-i18n
import Vue from 'vue';
import VueI18n from 'vue-i18n';
// Notify to vue to use vue-i18n
Vue.use(VueI18n);
// Define VueI18n
const i18n = new VueI18n({
    locale: document.documentElement.lang,
    silentTranslationWarn: true
});
// Export i18n
export default i18n;
