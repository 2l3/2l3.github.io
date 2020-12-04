<template>
    <div class="row">
        <div class="col-md-3 mb-4">
            <b-list-group>
                <b-list-group-item @click="changeSection" to="/dashboard/admin/settings/general">{{ $t('General') }}</b-list-group-item>
                <b-list-group-item @click="changeSection" to="/dashboard/admin/settings/appearance">{{ $t('Appearance') }}</b-list-group-item>
                <b-list-group-item @click="changeSection" to="/dashboard/admin/settings/localization">{{ $t('Localization') }}</b-list-group-item>
                <b-list-group-item @click="changeSection" to="/dashboard/admin/settings/authentication">{{ $t('Authentication') }}</b-list-group-item>
                <b-list-group-item @click="changeSection" to="/dashboard/admin/settings/mail">{{ $t('Mail') }}</b-list-group-item>
            </b-list-group>
        </div>
        <div class="col-md-9">
            <router-view/>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Settings",
        metaInfo() {
            return {
                title: this.$i18n.t('Settings')
            }
        },
        mounted() {
            this.getSettings();
            this.getDomains();
        },
        data() {
            return {
                settings: {
                    app_name: null,
                    app_keywords: null,
                    app_description: null,
                    app_domain: null,
                    only_primary_domain: false,
                    app_icon: null,
                    app_background: null,
                    homepage_enabled: null,
                    homepage_description: null,
                    app_timezone: null,
                    app_locale: null,
                    app_date_locale: null,
                    app_date_format: null,
                    app_register: null,
                    unverified_user_alert: false,
                    mail_mailer: 'log',
                    mail_encryption: null,
                    mail_from_address: null,
                    mail_from_name: null,
                    mail_host: null,
                    mail_password: null,
                    mail_port: null,
                    mail_username: null,
                },
                dataSettings: {},
                domains: [],
            }
        },
        methods: {
            // Get settings data
            getSettings() {
                let self = this;
                // Get all settings values
                axios.get('api/dashboard/settings').then(function (response) {
                    response.data.data.forEach(function (obj) {
                        self.settings[obj.key] = obj.value;
                        self.dataSettings[obj.key] = obj.value;
                    });
                });
            },
            getDomains() {
                let self = this;
                axios.get('api/dashboard/settings/domains').then(function (response) {
                    self.domains = response.data.data;
                });
            },
            // Save settings
            updateSettings() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                // Send new settings
                axios.post('api/dashboard/settings', {
                    type: 'settings', settings: self.settings
                }).then(function (response) {
                    // Reload new settings
                    self.getSettings();
                    // Update window.config data
                    window.config.name = self.settings.app_name;
                    window.config.description = self.settings.app_description;
                    window.config.register = self.settings.app_register;
                    window.config.date_locale = self.settings.app_date_locale;
                    window.config.date_format = self.settings.app_date_format;
                    window.config.unverified_user_alert = self.settings.unverified_user_alert;
                    // Change app language
                    if (document.documentElement.lang !== self.settings.app_locale) {
                        document.documentElement.lang = self.settings.app_locale;
                        self.$i18n.locale = self.settings.app_locale;
                        axios.get('api/lang/' + self.$i18n.locale).then(function (response) {
                            self.$i18n.setLocaleMessage(self.$i18n.locale, response.data.data);
                        });
                    }
                    // Update vuex
                    self.$store.commit('setSettings', window.config);
                    // Send success state
                    ladda.stop();
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                });
            },
            // Set default data on change page
            changeSection() {
                this.settings = Object.assign({}, this.dataSettings);
            },
        }
    }
</script>
