<template>
    <b-modal
        :hide-footer="true"
        :hide-header-close="true"
        :no-close-on-backdrop="true"
        :no-close-on-esc="true"
        :title="$t('Edit link')"
        centered
        id="edit-link"
        no-fade
        size="lg"
    >
        <template v-slot:default>
            <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
            <b-form-group class="mb-2" label-for="shortened-input">
                <template v-slot:label>{{ $t('Shortened link') }}</template>
                <b-input-group class="mb-2">
                    <b-form-input id="shortened-input" readonly v-model="link.link"/>
                    <template v-slot:append>
                        <b-button
                            :title="$t('Copy link')"
                            class="px-2 mb-0"
                            type="button"
                            v-b-tooltip.hover
                            variant="primary"
                            @click="copyLink('shortened-input')"
                        >
                            <i class="fas fa-fw fa-copy"/>
                            {{ $t('Copy link') }}
                        </b-button>
                    </template>
                </b-input-group>
            </b-form-group>
            <b-form-group class="mb-2" label-for="url-input">
                <template v-slot:label>{{ $t('Url') }}</template>
                <b-input-group class="mb-2">
                    <b-form-input id="url-input" readonly v-model="link.url"/>
                    <template v-slot:append>
                        <b-button
                            :title="$t('Copy link')"
                            class="px-2 mb-0"
                            type="button"
                            v-b-tooltip.hover
                            variant="primary"
                            @click="copyLink('url-input')"
                        >
                            <i class="fas fa-fw fa-copy"/>
                            {{ $t('Copy link') }}
                        </b-button>
                    </template>
                </b-input-group>
            </b-form-group>
            <form @submit.prevent="editLink" ref="editLink">
                <b-form-group class="mb-2" label-for="domain">
                    <template v-slot:label>{{ $t('Domain') }}</template>
                    <b-form-select
                        id="domain"
                        v-model="link.domain_id"
                        :options="domains"
                        value-field="id"
                        text-field="domain"
                        plain
                    >
                        <template v-slot:first>
                            <b-form-select-option :value="null">{{ $t('Default domain') }}</b-form-select-option>
                        </template>
                    </b-form-select>
                </b-form-group>
                <b-form-group class="mb-2" label-for="expires_at">
                    <template v-slot:label>{{ $t('Expires at') }}</template>
                    <b-input-group class="form-group-datetime">
                        <datetime
                            :format="{ year: 'numeric', month: '2-digit', day: '2-digit', hour: 'numeric', minute: '2-digit'}"
                            :phrases="{ok: $t('Ok'), cancel: $t('Cancel')}"
                            :value-zone="$store.state.settings.timezone"
                            auto
                            input-class="form-control"
                            input-id="expires_at"
                            type="datetime"
                            v-model="link.expires_at"
                        />
                        <template v-slot:append>
                            <b-button variant="outline-danger" @click="link.expires_at = null"><i class="fas fa-times"></i></b-button>
                        </template>
                    </b-input-group>
                </b-form-group>
                <b-form-group class="mb-2" label-for="password">
                    <template v-slot:label>{{ $t('Password') }}</template>
                    <b-form-input type="password" id="password" v-model="link.password"/>
                </b-form-group>
                <hr class="my-3">
                <p>
                    <b>{{ $t('Tags and Description') }}</b>
                    <br>
                    <small>{{ $t('Add tags and description to easily find your links') }}.</small>
                </p>
                <b-form-group class="mb-2" label-for="title">
                    <template v-slot:label>{{ $t('Title') }}</template>
                    <b-form-input id="title" v-model="link.title"/>
                </b-form-group>
                <b-form-group class="mb-2" label-for="tags">
                    <template v-slot:label>{{ $t('Tags') }}</template>
                    <b-form-tags input-id="tags" v-model="link.tags" :disableAddButton="true"/>
                </b-form-group>
                <b-form-group class="mb-2" label-for="description">
                    <template v-slot:label>{{ $t('Description') }}</template>
                    <b-textarea id="description" v-model="link.description"/>
                </b-form-group>
                <div class="float-right mt-3">
                    <router-link class="btn btn-danger" to="/dashboard/links">
                        <i class="fas fa-times"/>
                        {{ $t('Cancel') }}
                    </router-link>
                    <button class="btn btn-success btn-submit" data-style="zoom-in" type="submit">
                        <i class="fas fa-save"/>
                        {{ $t('Update') }}
                    </button>
                </div>
            </form>
        </template>
    </b-modal>
</template>

<script>
    import {Datetime} from 'vue-datetime';

    export default {
        name: "Edit",
        components: {
            datetime: Datetime
        },
        data() {
            return {
                loading: true,
                link: {
                    id: null,
                    link: null,
                    domain_id: null,
                    expires_at: null,
                    password: null,
                    title: null,
                    tags: [],
                    description: null,
                },
                domains: [],
                linkTypes: {
                    1: {
                        title: this.$i18n.t('Direct'),
                        description: this.$i18n.t('Redirect user to url instantly.')
                    },
                },
            }
        },
        mounted() {
            this.$bvModal.show('edit-link');
            this.getDomains();
        },
        methods: {
            copyLink(elementId) {
                let copyText = document.getElementById(elementId)
                copyText.select();
                copyText.setSelectionRange(0, 99999)
                document.execCommand("copy");
                this.$snack.success({button: false, text: this.$i18n.t('Link copied to clipboard')});
            },
            getDomains() {
                let self = this;
                self.loading = true;
                axios.get('api/dashboard/links/domains').then(function (response) {
                    self.getLink();
                    self.domains = response.data.data;
                }).catch(function () {
                    self.$router.push('/dashboard/links');
                });
            },
            getLink() {
                let self = this;
                axios.get('api/dashboard/links/' + self.$route.params.id).then(function (response) {
                    self.link = response.data.data;
                    self.loading = false;
                }).catch(function () {
                    self.$router.push('/dashboard/links');
                });
            },
            editLink() {
                let self = this;
                let ladda = Ladda.create(document.querySelector('.btn-submit'));
                ladda.start();
                axios.put('api/dashboard/links/' + self.link.id, self.link).then(function (response) {
                    ladda.stop();
                    self.$snack.success({
                        button: false,
                        text: response.data.data.message
                    });
                    self.$router.push('/dashboard/links');
                    self.$parent.getLinks();
                });
            },
        },
    }
</script>
