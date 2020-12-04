<template>
    <form @submit.prevent="$parent.updateSettings">
        <b-card>
            <h4 class="mb-0">{{ $t('Authentication') }}</h4>
            <p class="text-muted">{{ $t('Configure registration, social login and related 3rd party integrations') }}.</p>
            <div class="row mt-3">
                <div class="col-md-8">
                    <div class="form-group">
                        <toggle-button
                            :sync="true"
                            color="#3a1143"
                            id="app_register"
                            v-model="$parent.settings.app_register"
                        />
                        <label class="ml-2" for="app_register">{{ $t('Enable user registration') }}</label>
                        <small class="form-text text-muted">{{ $t('Allow users to register their account directly from the authentication page') }}.</small>
                    </div>
                    <div class="form-group">
                        <toggle-button
                            :sync="true"
                            color="#3a1143"
                            id="display_unverified_user_alert"
                            v-model="$parent.settings.unverified_user_alert"
                        />
                        <label class="ml-2" for="app_register">{{ $t('Display unverified user alert') }}</label>
                        <small class="form-text text-muted">{{ $t('Displays the alert that the user has not verified the account') }}.</small>
                    </div>
                    <div class="form-group">
                        <label>{{ $t('Default role') }}</label>
                        <b-form-select
                            :options="roles"
                            id="default_role"
                            plain
                            text-field="name"
                            v-model="$parent.settings.default_role"
                            value-field="id"
                        />
                        <small class="form-text text-muted">{{ $t('Role that is assigned to a user on the registration page') }}.</small>
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
        name: "Authentication",
        data() {
            return {
                roles: [],
            }
        },
        mounted() {
            this.getRoles();
        },
        methods: {
            // Get roles list
            getRoles() {
                let self = this;
                // Get all settings values
                axios.get('api/dashboard/settings/roles').then(function (response) {
                    self.roles = response.data.data;
                });
            },
        },
    }
</script>
