<template>
    <form @submit.prevent="$parent.updateSettings">
        <b-card>
            <h4 class="mb-0">{{ $t('General') }}</h4>
            <p class="text-muted">{{ $t('Configure general site settings') }}.</p>
            <div class="row mt-3">
                <div class="col-md-8">
                    <div class="form-group">
                        <label for="app_name">{{ $t('Site name') }}</label>
                        <input class="form-control" id="app_name" required type="text" v-model="$parent.settings.app_name">
                        <small class="form-text text-muted">{{ $t('Name of the application') }}.</small>
                    </div>
                    <div class="form-group">
                        <label for="app_keywords">{{ $t('Site keywords') }}</label>
                        <input class="form-control" id="app_keywords" required type="text" v-model="$parent.settings.app_keywords">
                        <small class="form-text text-muted">{{ $t('Keywords of the application (SEO)') }}.</small>
                    </div>
                    <div class="form-group mb-0">
                        <label for="app_description">{{ $t('Site description') }}</label>
                        <textarea class="form-control" id="app_description" required type="text" v-model="$parent.settings.app_description"/>
                        <small class="form-text text-muted">{{ $t('Description of the application (SEO)') }}.</small>
                    </div>
                </div>
            </div>
            <hr class="my-3">
            <h4 class="mb-0">{{ $t('Domain') }}</h4>
            <p class="text-muted">{{ $t('Select the default domain of the application') }}.</p>
            <div class="row mt-3">
                <div class="col-md-8">
                    <div class="form-group">
                        <label for="app_domain">{{ $t('Primary domain') }}</label>
                        <b-form-select
                            id="app_domain"
                            v-model="$parent.settings.app_domain"
                            :options="$parent.domains"
                            value-field="id"
                            text-field="domain"
                            plain
                        >
                            <template v-slot:first>
                                <b-form-select-option :value="null">{{ $t('All domains') }}</b-form-select-option>
                            </template>
                        </b-form-select>
                        <small class="form-text text-muted">{{ $t('Main domain of the system') }}.</small>
                    </div>
                    <div class="form-group">
                        <toggle-button
                            :sync="true"
                            color="#3a1143"
                            id="only_primary_domain"
                            v-model="$parent.settings.only_primary_domain"
                        />
                        <label class="ml-2" for="only_primary_domain">{{ $t('Deny access from other domains') }}</label>
                        <small class="form-text text-muted">{{ $t('Only allow access to the application from the main domain') }}.</small>
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
        name: "General",
    }
</script>
