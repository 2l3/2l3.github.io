<template>
    <div class="translations-component">
        <div class="row">
            <div class="col-md-3 mb-4">
                <router-link class="btn btn-success btn-block mb-0" to="/dashboard/admin/translations/add">
                    <i class="fas fa-plus float-left mt-1"/>
                    {{ $t('New localization') }}
                </router-link>
                <button @click="syncFiles" class="btn btn-secondary btn-block" type="button">
                    <i class="fas fa-sync float-left mt-1"/>
                    {{ $t('Synchronize translations') }}
                </button>
                <hr class="m-3">
                <b-list-group>
                    <b-list-group-item
                        :class="localeData.locale === locale.locale ? 'pointer active' : 'pointer'"
                        :key="localeData.locale"
                        @click="editLanguage(localeData)"
                        v-for="localeData in locales"
                    >
                        {{ localeData.name }}
                    </b-list-group-item>
                </b-list-group>
            </div>
            <div class="col-md-9">
                <list :localeKey="locale" ref="localeList"/>
            </div>
            <router-view/>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Translations",
        metaInfo() {
            return {
                title: this.$i18n.t('Translations')
            }
        },
        components: {
            'list': require('./translations/List').default,
        },
        data() {
            return {
                locale: {},
                locales: {},
                defaultLocales: {},
            }
        },
        mounted() {
            this.getLocales();
        },
        methods: {
            // Get locales list
            getLocales(assign = true) {
                let self = this;
                axios.get('api/dashboard/language').then(function (response) {
                    if (assign) {
                        response.data.data.forEach(function (obj) {
                            if (obj.locale === document.documentElement.lang) {
                                self.locale = obj;
                            }
                        });
                    }
                    self.locales = response.data.data;
                    self.defaultLocales = JSON.parse(JSON.stringify(response.data.data));
                });
            },
            // Edit language
            editLanguage(localeData) {
                this.locales = Object.assign({}, this.defaultLocales);
                this.locale = localeData;
            },
            // Sync translations file
            syncFiles() {
                let self = this;
                self.$refs.localeList.loading = true;
                axios.post('api/dashboard/language/sync').then(function (response) {
                    self.$refs.localeList.getLocale();
                    self.$snack.success({
                        button: false,
                        text: self.$t('Translation files updated correctly')
                    });
                });
            },
        },
    }
</script>

<style lang="scss">
    .translations-component {
        .dropdown-toggle::after {
            display: none;
        }
    }
</style>
