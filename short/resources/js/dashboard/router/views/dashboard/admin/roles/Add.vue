<template>
    <b-modal
        :hide-footer="true"
        :hide-header-close="true"
        :no-close-on-backdrop="true"
        :no-close-on-esc="true"
        :title="$t('Add role')"
        centered
        id="add-role"
        no-fade
    >
        <template v-slot:default>
            <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
            <form @submit.prevent="addRole" ref="addRole">
                <b-form-group class="mb-2" label-for="name">
                    <template v-slot:label>{{ $t('Name') }} <span class="text-danger">*</span></template>
                    <b-form-input
                        id="name"
                        required
                        v-model="role.name"
                    />
                </b-form-group>
                <label>{{ $t('Permissions') }} <span class="text-danger">*</span></label>
                <ul class="list-group">
                    <li class="list-group-item" v-for="(key, index) in role.permissions">
                        {{ permissionsLabels[index] }}
                        <div class="float-right">
                            <toggle-button :sync="true" color="#3a1143" id="status" name="status" v-model="role.permissions[index]"/>
                        </div>
                    </li>
                </ul>
                <div class="float-right mt-3">
                    <router-link class="btn btn-danger" to="/dashboard/admin/roles">
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
        mounted() {
            this.$bvModal.show('add-role');
            this.getPermissions();
        },
        data() {
            return {
                loading: true,
                role: {
                    name: null,
                    permissions: [],
                },
                permissionsLabels: [],
            }
        },
        methods: {
            // Get permissions list
            getPermissions() {
                let self = this;
                // Get all settings values
                axios.get('api/dashboard/roles/permissions').then(function (response) {
                    self.role.permissions = response.data.data.controllers;
                    self.permissionsLabels = response.data.data.controllerLabels;
                    self.loading = false;
                });
            },
            // Add role
            addRole() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                // Send request
                axios.post('api/dashboard/roles', self.role).then(function (response) {
                    ladda.stop();
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                    self.$router.push('/dashboard/admin/roles');
                    self.$parent.getRoles();
                });
            },
        },
    }
</script>
