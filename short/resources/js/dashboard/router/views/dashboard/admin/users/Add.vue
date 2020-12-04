<template>
    <b-modal
        :hide-footer="true"
        :hide-header-close="true"
        :no-close-on-backdrop="true"
        :no-close-on-esc="true"
        :title="$t('Add user')"
        centered
        id="add-user"
        no-fade
    >
        <template v-slot:default>
            <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
            <form @submit.prevent="addUser" ref="addUser">
                <b-form-group class="mb-2" label-for="name">
                    <template v-slot:label>{{ $t('Name') }} <span class="text-danger">*</span></template>
                    <b-form-input
                        id="name"
                        required
                        v-model="user.name"
                    />
                </b-form-group>
                <b-form-group :label="$t('Surname')" class="mb-2" label-for="surname">
                    <b-form-input
                        id="surname"
                        v-model="user.surname"
                    />
                </b-form-group>
                <b-form-group class="mb-2" label-for="email">
                    <template v-slot:label>{{ $t('Email') }} <span class="text-danger">*</span></template>
                    <b-form-input
                        id="email"
                        required
                        type="email"
                        v-model="user.email"
                    />
                </b-form-group>
                <b-form-group class="mb-2" label-for="password">
                    <template v-slot:label>{{ $t('Password') }} <span class="text-danger">*</span></template>
                    <b-input-group>
                        <b-form-input
                            :type="passwordInput"
                            id="password"
                            required
                            v-model="user.password"
                        />
                        <b-input-group-append style="height: 35px;">
                            <b-button :title="$t('Show/Hide password')" @click="showPassword" class="px-2" type="button" v-b-tooltip.hover variant="primary">
                                <i class="fas fa-fw fa-eye" v-if="passwordInput === 'password'"/>
                                <i class="fas fa-fw fa-eye-slash" v-else/>
                            </b-button>
                            <b-button :title="$t('Generate new password')" @click="generatePassword" class="px-2" type="button" v-b-tooltip.hover variant="secondary">
                                <i class="fas fa-fw fa-sync"/>
                            </b-button>
                        </b-input-group-append>
                    </b-input-group>
                </b-form-group>
                <b-form-group class="mb-2" label-for="role">
                    <template v-slot:label>{{ $t('Role') }} <span class="text-danger">*</span></template>
                    <b-form-select
                        :options="roles"
                        id="default_role"
                        plain
                        required
                        text-field="name"
                        v-model="user.role"
                        value-field="id"
                    />
                </b-form-group>
                <b-form-group class="mb-1" label-for="email">
                    <template v-slot:label>{{ $t('Status') }} <span class="text-danger">*</span></template>
                    <div class="row">
                        <div class="col-auto">
                            <toggle-button :sync="true" color="#3a1143" id="status" name="status" v-model="user.status"/>
                        </div>
                        <div class="col ml-n2">
                            <small class="text-muted" v-if="user.status">{{ $t('The user is activated') }}</small>
                            <small class="text-muted" v-else>{{ $t('The user is disabled') }}</small>
                        </div>
                    </div>
                    <small class="form-text text-muted mb-2">
                        {{ $t('When the user is deactivated, the registry is created in the system, so the email is reserved, but can not login until it is activated again') }}.
                    </small>
                </b-form-group>
                <div class="float-right mt-3">
                    <router-link class="btn btn-danger" to="/dashboard/admin/users">
                        <i class="fas fa-times"/>
                        {{ $t('Cancel') }}
                    </router-link>
                    <button class="btn btn-success btn-submit" data-style="zoom-in" type="submit">
                        <i class="fas fa-save"/>
                        {{ $t('Create') }}
                    </button>
                </div>
            </form>
        </template>
    </b-modal>
</template>

<script>
    export default {
        name: "Add",
        data() {
            return {
                loading: true,
                passwordInput: 'password',
                user: {
                    name: null,
                    surname: null,
                    email: null,
                    status: true,
                    role: null,
                    password: null,
                },
                roles: [],
            }
        },
        mounted() {
            this.$bvModal.show('add-user');
            this.getRoles();
        },
        methods: {
            // Show the password input
            showPassword() {
                if (this.passwordInput === 'password') {
                    this.passwordInput = 'text';
                } else {
                    this.passwordInput = 'password';
                }
            },
            // Generate new password
            generatePassword() {
                this.user.password = window.GeneratePassword.generate({
                    length: 8,
                    numbers: true,
                    symbols: false,
                    excludeSimilarCharacters: true,
                    strict: true,
                });
            },
            // Get roles list
            getRoles() {
                let self = this;
                // Get all settings values
                axios.get('api/dashboard/users/roles').then(function (response) {
                    self.roles = response.data.data;
                    self.loading = false;
                });
            },
            // Add user
            addUser() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                // Send request
                axios.post('api/dashboard/users', self.user).then(function (response) {
                    ladda.stop();
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                    self.$router.push('/dashboard/admin/users');
                    self.$parent.getUsers();
                });
            },
        },
    }
</script>
