<template>
    <b-modal
        :hide-footer="true"
        :hide-header-close="true"
        :no-close-on-backdrop="true"
        :no-close-on-esc="true"
        :title="$t('Edit user')"
        centered
        id="edit-user"
        no-fade
    >
        <template v-slot:default>
            <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
            <form @submit.prevent="editUser" ref="editUser">
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
                <b-form-group :label="$t('Email')" class="mb-2" label-for="email">
                    <b-form-input
                        id="email"
                        type="email"
                        v-model="user.email"
                    />
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
                <b-form-group class="mb-1" label-for="status">
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
                        {{ $t('Update') }}
                    </button>
                </div>
            </form>
        </template>
    </b-modal>
</template>

<script>
    export default {
        name: "Edit",
        data() {
            return {
                loading: true,
                user: {
                    id: null,
                    name: null,
                    surname: null,
                    email: null,
                    status: true,
                    role: null,
                },
                roles: [],
            }
        },
        mounted() {
            this.$bvModal.show('edit-user');
            this.getUser();
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
            // Get user details
            getUser() {
                let self = this;
                // Get user details
                axios.get('api/dashboard/users/' + self.$route.params.id).then(function (response) {
                    self.user = response.data.data;
                    self.loading = false;
                }).catch(function () {
                    self.$router.push('/dashboard/admin/users');
                });
            },
            // Edit user
            editUser() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                // Send request
                axios.put('api/dashboard/users/' + self.user.id, self.user).then(function (response) {
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
