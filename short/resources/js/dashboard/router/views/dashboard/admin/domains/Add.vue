<template>
    <b-modal
        :hide-footer="true"
        :hide-header-close="true"
        :no-close-on-backdrop="true"
        :no-close-on-esc="true"
        :title="$t('Add domain')"
        centered
        id="add-domain"
        no-fade
    >
        <template v-slot:default>
            <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
            <form @submit.prevent="addDomain" ref="addDomain">
                <b-form-group class="mb-2" label-for="name">
                    <template v-slot:label>{{ $t('Domain') }} <span class="text-danger">*</span></template>
                    <b-form-input id="name" type="url" required v-model="domain.domain"/>
                </b-form-group>
                <b-form-group class="mb-2" label-for="redirect">
                    <template v-slot:label>{{ $t('Redirect') }}</template>
                    <b-form-input id="redirect" type="url" v-model="domain.redirect"/>
                </b-form-group>
                <div class="float-right mt-3">
                    <router-link class="btn btn-danger" to="/dashboard/admin/domains">
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
                loading: false,
                domain: {
                    domain: null,
                    redirect: null,
                }
            }
        },
        mounted() {
            this.$bvModal.show('add-domain');
        },
        methods: {
            addDomain() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                // Send request
                axios.post('api/dashboard/admin/domains', self.domain).then(function (response) {
                    ladda.stop();
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                    self.$router.push('/dashboard/admin/domains');
                    self.$parent.getDomains();
                });
            }
        }
    }
</script>
