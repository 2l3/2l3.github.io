<template>
    <form @submit.prevent="$parent.updateSettings">
        <b-card>
            <h4 class="mb-0">{{ $t('Localization') }}</h4>
            <p class="text-muted">{{ $t('Manage localization settings for the site') }}.</p>
            <div class="row mt-3">
                <div class="col-md-8">
                    <div class="form-group">
                        <label for="app_timezone">{{ $t('Timezone') }}</label>
                        <input class="form-control" id="app_timezone" required type="text" v-model="$parent.settings.app_timezone">
                        <small class="form-text text-muted">{{ $t('List of supported timezones can be found') }} <a href="https://php.net/manual/en/timezones.php" target="_blank">{{ $t('here') }}</a>.</small>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <label for="app_timezone">{{ $t('Translations Locale') }}</label>
                        <select class="form-control" id="app_locale" required v-model="$parent.settings.app_locale">
                            <option :value="locale.locale" v-for="locale in locales">{{ locale.name }}</option>
                        </select>
                        <small class="form-text text-muted">{{ $t('Default translations locale that will be applied to new users') }}.</small>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <label for="app_date_locale">{{ $t('Date Locale') }}</label>
                        <input class="form-control" id="app_date_locale" required type="text" v-model="$parent.settings.app_date_locale">
                        <small class="form-text text-muted">{{ $t('Set locale used for date and time translation') }}. {{ $t('Selected locale must be installed on the operating system') }}.</small>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="form-group">
                        <label for="app_date_format">{{ $t('Date Format') }}</label>
                        <input class="form-control" id="app_date_format" required type="text" v-model="$parent.settings.app_date_format">
                        <small class="form-text text-muted">{{ $t('Default format for dates on the site') }}.</small>
                        <small class="form-text text-muted">{{ $t('Preview') }}: {{ this.$parent.settings | previewDate }}</small>
                    </div>
                </div>
            </div>
            <button class="btn btn-success btn-submit m-0" data-style="zoom-in" type="submit">
                <i class="fas fa-save"/>
                {{ $t('Update') }}
            </button>
        </b-card>
    </form>
</template>

<script>
    export default {
        name: "Localization",
        data() {
            return {
                locales: {},
                previewDate: null,
            }
        },
        mounted() {
            this.getLocales();
        },
        filters: {
            previewDate: function (value) {
                moment.locale(value.app_date_locale);
                return value.app_date_format != null ? moment().format(value.app_date_format) : null;
            }
        },
        methods: {
            // Get locales list
            getLocales() {
                let self = this;
                axios.get('api/lang').then(function (response) {
                    self.locales = response.data.data;
                });
            },
        },
    }
</script>
