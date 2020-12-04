<template>
    <div>
        <p class="m-t-15 font-size-13">{{ $t('Please enter your information to create your account') }}</p>
        <div class="mb-3">
            <form @submit.prevent="register" class="submit">
                <div class="form-group mb-3">
                    <input :placeholder="$t('Name')" autofocus class="form-control" id="name" required type="text" v-model="registerForm.name">
                </div>
                <div class="form-group mb-3">
                    <input :placeholder="$t('Email')" class="form-control" id="email" required type="email" v-model="registerForm.email">
                </div>
                <div class="form-group mb-3">
                    <input :placeholder="$t('Password')" class="form-control" id="password" required type="password" v-model="registerForm.password">
                </div>
                <div class="form-group">
                    <input :placeholder="$t('Password')" class="form-control" id="password_confirmation" required type="password" v-model="registerForm.password_confirmation">
                </div>
                <div class="form-group text-right">
                    <button class="btn btn-primary btn-submit pl-5 pr-5" data-style="zoom-in" type="submit">{{ $t('Register') }}</button>
                </div>
            </form>
        </div>
        <div class="m-0">
            <p class="m-0">
                {{ $t('Do you already have an account?') }}
                <router-link to="/auth/login">{{ $t('Login') }}</router-link>
            </p>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Register",
        metaInfo() {
            return {
                title: this.$i18n.t('Create account')
            }
        },
        data() {
            return {
                registerForm: {
                    name: null,
                    email: null,
                    password: null,
                    password_confirmation: null
                }
            }
        },
        methods: {
            register() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                axios.post('api/auth/register', this.registerForm).then(function (response) {
                    swal('Ok', self.$t('Your user account has been created correctly'), 'success').then(() => {
                        self.$router.push('/auth/login');
                    });
                }).catch(function () {
                    self.registerForm.password = null;
                    self.registerForm.password_confirmation = null;
                })
            }
        }
    }
</script>
