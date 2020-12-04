<template>
    <b-modal
        :hide-footer="true"
        :hide-header-close="true"
        :no-close-on-backdrop="true"
        :no-close-on-esc="true"
        :title="$t('New localization')"
        centered
        id="new-localization"
        no-fade
    >
        <template v-slot:default>
            <form @submit.prevent="addLocalization" ref="addLocalization">
                <b-form-group :label="$t('Localization code')" class="mb-2" label-for="localization-locale">
                    <b-form-input
                        id="localization-locale"
                        maxlength="5"
                        minlength="2"
                        required
                        v-model="localization.locale"
                    />
                </b-form-group>
                <b-form-group :label="$t('Localization name')" label-for="localization-name">
                    <b-form-input
                        id="localization-name"
                        required
                        v-model="localization.name"
                    />
                </b-form-group>
                <div class="float-right">
                    <router-link class="btn btn-danger" to="/dashboard/admin/translations">
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
        name: "NewLocalization",
        mounted() {
            this.$bvModal.show('new-localization')
        },
        data() {
            return {
                localization: {
                    locale: null,
                    name: null
                }
            }
        },
        methods: {
            addLocalization() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                // Send request
                axios.post('api/dashboard/language', self.localization).then(function (response) {
                    ladda.stop();
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                    self.$router.push('/dashboard/admin/translations');
                    self.$parent.getLocales();
                });
            }
        },
    }
</script>
