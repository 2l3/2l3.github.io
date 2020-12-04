<template>
    <b-card>
        <form @submit.prevent="$parent.updateSettings">
            <h4 class="mb-0">{{ $t('Home page') }}</h4>
            <p class="text-muted">{{ $t('Activate and personalize the home page') }}.</p>
            <div class="form-group">
                <toggle-button
                    :sync="true"
                    color="#3a1143"
                    id="homepage_enabled"
                    v-model="$parent.settings.homepage_enabled"
                />
                <label class="ml-2" for="homepage_enabled">{{ $t('Enable home page') }}</label>
                <small class="form-text text-muted">{{ $t('Enable the home page for anonymous users to shorten links') }}.</small>
            </div>
            <div class="form-group">
                <label for="homepage_description">{{ $t('Description home page') }}</label>
                <input class="form-control" id="homepage_description" required type="text" v-model="$parent.settings.homepage_description">
                <small class="form-text text-muted">{{ $t('Text that appears at the top of the form to shorten links on the home page') }}.</small>
            </div>
            <button class="btn btn-success btn-submit" data-style="zoom-in" type="submit">
                <i class="fas fa-save"/>
                {{ $t('Update') }}
            </button>
        </form>
        <hr class="my-3">
        <h4 class="mb-0">{{ $t('Appearance') }}</h4>
        <p class="text-muted">{{ $t('Customize the site with your own corporate image') }}.</p>
        <div class="row mt-3">
            <div class="col-md-8">
                <div class="form-group">
                    <label>{{ $t('App icon') }}</label>
                    <b-img
                        :alt="$t('App icon')"
                        :src="($parent.settings.app_icon) ? $parent.settings.app_icon.image : null"
                        center
                        class="mb-3 img-settings"
                    />
                    <b-form-file
                        :browse-text="$t('Browse')"
                        :placeholder="$t('Choose a file') + '...'"
                        @change="changeIcon"
                        accept=".jpg, .png"
                        class="pointer"
                        id="app_icon"
                        no-drop
                        ref="app_icon"
                    />
                    <small class="form-text text-muted">{{ $t('App icon') }}.</small>
                </div>
                <div class="form-group">
                    <label>{{ $t('App background') }}</label>
                    <b-img
                        :alt="$t('App background')"
                        :src="($parent.settings.app_background) ? $parent.settings.app_background.image : null"
                        center
                        class="mb-3 img-settings"
                    />
                    <b-form-file
                        :browse-text="$t('Browse')"
                        :placeholder="$t('Choose a file') + '...'"
                        @change="changeBackground"
                        accept=".jpg, .png"
                        class="pointer"
                        id="app_background"
                        no-drop
                        ref="app_background"
                    />
                    <small class="form-text text-muted">{{ $t('Background image of the authentication pages') }}.</small>
                </div>
            </div>
        </div>
    </b-card>
</template>

<script>
    export default {
        name: "Appearance",
        methods: {
            // Chage icon image event
            changeIcon(e) {
                this.uploadImage(e, 'icon');
            },
            // Chage background image event
            changeBackground(e) {
                this.uploadImage(e, 'background');
            },
            // Upload & save image
            uploadImage(e, type) {
                let self = this;
                // Set formdata
                let formData = new FormData();
                formData.append('type', 'images');
                formData.append('image', type);
                formData.append('file', e.target.files[0]);
                // Update image
                axios.post('api/dashboard/settings',
                    formData,
                    {header: {'Content-Type': 'multipart/form-data'}}
                ).then(function (response) {
                    if (response.data.success === true) {
                        // Reload new settings
                        self.$parent.getSettings();
                        // Update window.config data
                        if (type === 'icon') {
                            window.config.logo = URL.createObjectURL(e.target.files[0]);
                            document.querySelector("link[rel*='icon']").href = window.config.logo;
                            self.$refs['app_icon'].reset();
                        } else if (type === 'background') {
                            window.config.background = URL.createObjectURL(e.target.files[0]);
                            self.$refs['app_background'].reset();
                        }
                        // Update vuex
                        self.$store.commit('setSettings', window.config);
                        // Send success state
                        self.$snack.success({
                            button: false,
                            text: response.data.data.message
                        });
                    } else {
                        swal(self.$t('Error'), response.data.error.message, 'error');
                    }
                });
            },
        }
    }
</script>
