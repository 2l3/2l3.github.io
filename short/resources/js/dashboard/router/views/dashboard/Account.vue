<template>
    <div class="account-component">
        <div class="row">
            <div class="col-12">
                <div class="d-flex align-items-center justify-content-between mb-4">
                    <h1 class="h3 mb-0 text-gray-800">{{ $t('Account') }}</h1>
                </div>
            </div>
            <div class="col-12">
                <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
                <div class="row">
                    <div class="col-12" v-if="!user.verified">
                        <b-card bg-variant="danger" class="card-verify" header-tag="header">
                            <template v-slot:header>
                                {{ $t('Verify email') }}
                            </template>
                            {{ $t('Your email has not been verified, so your account may be restricted until verified') }}.
                            <br>
                            {{ $t('You can request account verification in the following button, where we will send you a link to your email to verify your account') }}.
                            <form @submit.prevent="requestVerify" class="mt-2">
                                <button class="btn btn-primary btn-submit-verify mb-0" data-style="zoom-in" type="submit">
                                    <i class="fas fa-envelope-open-text"></i>
                                    {{ $t('Request email verification') }}
                                </button>
                            </form>
                        </b-card>
                    </div>
                    <div class="col-lg-6 col-md-12">
                        <b-card header-tag="header">
                            <template v-slot:header>
                                {{ $t('Personal information') }}
                            </template>
                            <form @submit.prevent="updateDetails">
                                <div class="form-group mb-3">
                                    <label for="name">{{ $t('Name') }} <span class="text-danger">*</span></label>
                                    <input class="form-control" id="name" required type="text" v-model="user.name">
                                </div>
                                <div class="form-group mb-3">
                                    <label for="surname">{{ $t('Surname') }}</label>
                                    <input class="form-control" id="surname" type="text" v-model="user.surname">
                                </div>
                                <div class="form-group">
                                    <label for="email">{{ $t('Email') }}</label>
                                    <input class="form-control" id="email" type="text" v-model="user.email">
                                </div>
                                <button class="btn btn-success btn-submit-details mb-0 float-right" data-style="zoom-in" type="submit">
                                    <i class="fas fa-save"/>
                                    {{ $t('Save changes') }}
                                </button>
                            </form>
                        </b-card>
                    </div>
                    <div class="col-lg-6 col-md-12">
                        <b-card header-tag="header">
                            <template v-slot:header>
                                {{ $t('Profile image') }}
                            </template>
                            <div class="form-group mb-0">
                                <b-img
                                    :alt="$t('User avatar')"
                                    :src="(user.avatar) ? user.avatar : $store.state.user.avatar"
                                    center
                                    class="mb-3 img-settings"
                                />
                                <b-form-file
                                    :browse-text="$t('Browse')"
                                    :placeholder="$t('Choose a file') + '...'"
                                    @change="changeAvatar"
                                    accept=".jpg, .png"
                                    class="pointer"
                                    id="user_avatar"
                                    no-drop
                                    ref="user_avatar"
                                />
                                <small>{{ $t('The image must have a 1:1 aspect ratio and a size of less than 1MB') }}</small>
                            </div>
                        </b-card>
                    </div>
                    <div class="col-lg-6 col-md-12">
                        <b-card header-tag="header">
                            <template v-slot:header>
                                {{ $t('Change password') }}
                            </template>
                            <form @submit.prevent="changePassword">
                                <div class="form-group mb-3">
                                    <label for="current_password">{{ $t('Current password') }} <span class="text-danger">*</span></label>
                                    <input class="form-control" id="current_password" required type="password" v-model="password.current_password">
                                </div>
                                <div class="form-group mb-3">
                                    <label for="password">{{ $t('New password') }} <span class="text-danger">*</span></label>
                                    <input class="form-control" id="password" required type="password" v-model="password.password">
                                </div>
                                <div class="form-group">
                                    <label for="password_confirmation">{{ $t('Confirm password') }} <span class="text-danger">*</span></label>
                                    <input class="form-control" id="password_confirmation" required type="password" v-model="password.password_confirmation">
                                </div>
                                <button class="btn btn-success btn-submit-password mb-0 float-right" data-style="zoom-in" type="submit">
                                    <i class="fas fa-save"/>
                                    {{ $t('Change password') }}
                                </button>
                            </form>
                        </b-card>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Account",
        metaInfo() {
            return {
                title: this.$i18n.t('Account')
            }
        },
        mounted() {
            this.getUser()
        },
        data() {
            return {
                loading: true,
                user: {
                    name: null,
                    surname: null,
                    email: null,
                    verified: true,
                    avatar: null,
                },
                password: {
                    current_password: null,
                    password: null,
                    password_confirmation: null,
                },
            }
        },
        methods: {
            // Get user details
            getUser() {
                let self = this;
                axios.post('api/auth/user').then(function (response) {
                    self.user = response.data.data;
                    self.loading = false;
                }).catch(function () {
                    self.loading = false;
                });
            },
            // Change user account details
            updateDetails() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit-details'));
                ladda.start();
                axios.post('api/account/details', self.user).then(function (response) {
                    self.$store.commit('setUser');
                    self.user = response.data.data.user;
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                    ladda.stop();
                });
            },
            requestVerify() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit-verify'));
                ladda.start();
                axios.post('api/account/verification').then(function (response) {
                    ladda.stop();
                    swal(self.$t('Ok'), response.data.data.message, 'success');
                });
            },
            // Change user password
            changePassword() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit-password'));
                ladda.start();
                axios.post('api/account/password', self.password).then(function (response) {
                    self.password = {
                        current_password: null,
                        password: null,
                        password_confirmation: null,
                    };
                    ladda.stop();
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                }).catch(function () {
                    self.password = {
                        password_confirmation: null,
                    };
                })
            },
            // Change user avatar
            changeAvatar(e) {
                let self = this;
                // Set formData
                let formData = new FormData();
                formData.append('file', e.target.files[0]);
                // Update image
                axios.post('api/account/avatar',
                    formData,
                    {header: {'Content-Type': 'multipart/form-data'}}
                ).then(function (response) {
                    self.user.avatar = URL.createObjectURL(e.target.files[0]);
                    self.$refs['user_avatar'].reset();
                    self.$store.commit('setUser');
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                });
            },
        },
    }
</script>

<style lang="scss">
    .account-component {
        .card-verify {
            -webkit-transition: all 0.2s ease !important;
        }
    }
</style>
