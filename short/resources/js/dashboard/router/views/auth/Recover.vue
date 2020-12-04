<template>
    <div>
        <p class="m-t-15 font-size-13">{{ $t('Enter the email associated with the account') }}</p>
        <div class="mb-3">
            <form @submit.prevent="recover" class="submit">
                <div class="form-group">
                    <input :placeholder="$t('Email')" autofocus class="form-control" id="email" required type="email" v-model="recoverForm.email">
                </div>
                <div class="form-group text-right">
                    <button class="btn btn-primary btn-submit pl-5 pr-5" data-style="zoom-in" type="submit">{{ $t('Send account recovery email') }}</button>
                </div>
            </form>
        </div>
        <div class="m-0">
            <p class="m-0">
                {{ $t('Do you already have access to your account?') }}
                <router-link to="/auth/login">{{ $t('Login') }}</router-link>
            </p>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Recover",
        metaInfo() {
            return {
                title: this.$i18n.t('Recover account')
            }
        },
        data() {
            return {
                recoverForm: {
                    email: null
                }
            }
        },
        methods: {
            recover() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                axios.post('api/auth/recover', this.recoverForm).then(function (response) {
                    swal(self.$t('Ok'), response.data.data.message, 'success');
                    self.$router.push('/auth/login');
                    ladda.stop();
                }).catch(function () {
                    self.recoverForm.email = null;
                });
            }
        }
    }
</script>
