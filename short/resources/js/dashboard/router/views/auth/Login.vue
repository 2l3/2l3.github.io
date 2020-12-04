<template>
    <div>
        <p class="m-t-15 font-size-13">{{ $t('Please enter your email and password to login') }}</p>
        <div class="mb-3">
            <form @submit.prevent="login" class="submit">
                <div class="form-group mb-3">
                    <input :placeholder="$t('Email')" autofocus class="form-control" id="email" required type="email" v-model="loginForm.email">
                </div>
                <div class="form-group">
                    <input :placeholder="$t('Password')" class="form-control" id="password" required type="password" v-model="loginForm.password">
                </div>
                <div class="form-group text-right">
                    <button class="btn btn-primary btn-submit px-5" data-style="zoom-in" type="submit">{{ $t('Login') }}</button>
                </div>
            </form>
        </div>
        <div class="m-0">
            <p>
                {{ $t('You can not access your account?') }}
                <router-link to="/auth/recover">{{ $t('Recover account') }}</router-link>
            </p>
            <p class="m-0" v-if="$store.state.settings.register">
                {{ $t('Don\'t have an account?') }}
                <router-link to="/auth/register">{{ $t('Create account') }}</router-link>
            </p>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Login",
        metaInfo() {
            return {
                title: this.$i18n.t('Login')
            }
        },
        data() {
            return {
                loginForm: {
                    email: null,
                    password: null
                }
            }
        },
        methods: {
            login() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                axios.post('api/auth/login', this.loginForm).then(function (response) {
                    self.$store.commit('login', response.data.data);
                    self.$router.push('/dashboard/home');
                    ladda.stop();
                }).catch(function () {
                    self.loginForm.password = null;
                });
            }
        }
    }
</script>
