<template>
    <div class="translations-list-component">
        <b-card class="mb-0" style="min-height: 100vh;">
            <loading-animation
                :active.sync="loading"
                :is-full-page="false"
                :opacity="1"
                color="#3a1143"
            />
            <div class="row" v-show="!loading">
                <div class="col-md-6">
                    <div class="input-group mb-3">
                        <input
                            class="form-control focus-editable"
                            style="font-weight: 500;"
                            v-model="locale.name"
                        >
                        <div class="input-group-append">
                            <button
                                @click="removeLocale(locale.locale)"
                                class="btn btn-outline-danger"
                                style="padding-bottom: 6px;"
                                type="button"
                                v-if="locale.locale !== defaultLocale"
                            >
                                <i class="fas fa-trash"/>
                            </button>
                            <button
                                @click="savelocale"
                                class="btn btn-success"
                                style="padding-bottom: 6px;"
                                type="button"
                            >
                                <i class="fas fa-save"/>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-xl-4 ml-auto">
                    <input
                        :placeholder="$t('Search translations')"
                        class="form-control mb-3"
                        type="text"
                        v-model="search"
                    >
                </div>
                <div class="col-12">
                    <b-table
                        :fields="[
                        {key: 'key', label: $t('Source text'), class: 'fixed-50 bt-0'},
                        {key: 'value', label: $t('Translation'), class: 'fixed-50 bt-0'}
                    ]"
                        :filter="search"
                        :items="strings"
                        fixed
                        hover
                        show-empty
                        small
                        table-class="mb-0"
                    >
                        <template v-slot:cell(key)="data">
                            {{ data.item.key }}
                        </template>
                        <template v-slot:cell(value)="data">
                            <textarea-autosize
                                :class="data.item.value !== data.item.key ? 'translation-edited' : ''"
                                :placeholder="$t('Enter the translated string')"
                                class="form-control focus-editable"
                                rows="1"
                                v-model="data.item.value"
                            />
                        </template>
                        <template v-slot:empty="scope">
                            <div class="text-center my-5">
                                <img
                                    :alt="$t('No results')"
                                    class="mb-5"
                                    src="/img/icons/robot.svg"
                                    style="max-height: 165px;"
                                >
                                <h3 class="bold">{{ $t('No records found') }}</h3>
                            </div>
                        </template>
                        <template v-slot:emptyfiltered="scope">
                            <div class="text-center my-5">
                                <img
                                    :alt="$t('No results')"
                                    class="mb-5"
                                    src="/img/icons/robot.svg"
                                    style="max-height: 165px;"
                                >
                                <h3>{{ $t('No records match the entered search') }}</h3>
                            </div>
                        </template>
                    </b-table>
                </div>
            </div>
        </b-card>
    </div>
</template>

<script>
    export default {
        name: "List",
        props: ['localeKey'],
        data() {
            return {
                loading: true,
                defaultLocale: document.documentElement.lang,
                locale: [],
                strings: [],
                search: null,
            }
        },
        watch: {
            localeKey() {
                this.loading = true;
                this.getLocale();
            },
        },
        methods: {
            // Get locale strings
            getLocale() {
                let self = this;
                self.loading = true;
                self.locale = Object.assign({}, self.localeKey);
                axios.get('api/dashboard/language/' + self.locale.id).then(function (response) {
                    self.strings = response.data.data;
                    self.loading = false;
                }).catch(function () {
                    self.loading = false;
                });
            },
            // Update locale
            savelocale() {
                let self = this;
                self.loading = true;
                axios.put('api/dashboard/language/' + self.locale.id, {
                    name: self.locale.name,
                    strings: self.strings
                }).then(function (response) {
                    if (self.locale.locale === self.defaultLocale) {
                        axios.get('api/lang/' + self.$i18n.locale).then(function (response) {
                            self.$i18n.setLocaleMessage(self.$i18n.locale, response.data.data);
                        });
                    }
                    self.$parent.getLocales(false);
                    self.loading = false;
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                }).catch(function () {
                    self.loading = false;
                });
            },
            // Delete locale
            removeLocale() {
                let self = this;
                self.loading = true;
                axios.delete('/api/dashboard/language/' + self.locale.id).then(function (response) {
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                    self.$parent.getLocales();
                }).catch(function () {
                    self.loading = false;
                });
            },
        },
    }
</script>

<style lang="scss">
    .translations-list-component {
        .focus-editable {
            line-height: 1.8;

            &:not(:focus) {
                border-color: transparent;
            }
        }

        .fixed-50 {
            width: 50%;
        }

        th.bt-0 {
            border-top: none !important;
        }

        .translation-edited, .translation-edited:focus {
            background-color: #fdf9ee;
        }
    }
</style>
