<template>
    <div>
        <p class="m-t-15 font-size-13">{{ $t('Enter your account information and the new password') }}</p>
        <div class="mb-3">
            <form @submit.prevent="reset" class="submit">
                <div class="form-group mb-3">
                    <input :placeholder="$t('Email')" class="form-control" id="email" required type="email" v-model="resetForm.email">
                </div>
                <div class="form-group mb-3">
                    <input :placeholder="$t('Password')" class="form-control" id="password" required type="password" v-model="resetForm.password">
                </div>
                <div class="form-group">
                    <input :placeholder="$t('Confirm password')" class="form-control" id="password_confirmation" required type="password" v-model="resetForm.password_confirmation">
                </div>
                <div class="form-group text-right">
                    <button class="btn btn-primary btn-submit pl-5 pr-5" data-style="zoom-in" type="submit">{{ $t('Reset password') }}</button>
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
        name: "Reset",
        metaInfo() {
            return {
                title: this.$i18n.t('Recover account')
            }
        },
        data() {
            return {
                resetForm: {
                    token: null,
                    email: null,
                    password: null,
                    password_confirmation: null
                }
            }
        },
        mounted() {
            this.resetForm.token = this.$route.params.token;
        },
        methods: {
            reset() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                axios.post('api/auth/reset', this.resetForm).then(function (response) {
                    self.$store.commit('login', response.data.data);
                    self.$router.push('/dashboard/home');
                    ladda.stop();
                }).catch(function () {
                    self.resetForm.password = null;
                    self.resetForm.password_confirmation = null;
                });
            }
        }
    }
</script>
