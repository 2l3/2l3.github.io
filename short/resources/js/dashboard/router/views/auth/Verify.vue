<template>
    <div>
        <p class="m-t-15 font-size-13">{{ $t('Enter the account details to verify the email') }}</p>
        <div class="mb-3">
            <form @submit.prevent="verify" class="submit">
                <div class="form-group mb-3">
                    <input :placeholder="$t('Password')" class="form-control" id="password" required type="password" v-model="verifyForm.password">
                </div>
                <div class="form-group text-right">
                    <button class="btn btn-primary btn-submit pl-5 pr-5" data-style="zoom-in" type="submit">{{ $t('Verify email') }}</button>
                </div>
            </form>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Verify",
        metaInfo() {
            return {
                title: this.$i18n.t('Verify email')
            }
        },
        data() {
            return {
                verifyForm: {
                    token: null,
                    password: null
                }
            }
        },
        mounted() {
            if (this.$store.state.user) {
                this.$store.commit('logout');
            }
            this.verifyForm.token = this.$route.params.token;
        },
        methods: {
            verify() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                axios.post('api/auth/verify', this.verifyForm).then(function (response) {
                    swal(self.$t('Ok'), self.$t('Your email has been verified correctly'), 'success').then(() => {
                        self.$store.commit('login', response.data.data);
                        self.$router.push('/dashboard/home');
                    });
                }).catch(function () {
                    self.verifyForm.password = null;
                });
            }
        }
    }
</script>
