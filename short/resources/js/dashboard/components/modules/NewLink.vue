<template>
    <li class="nav-item active">
        <a href="#" @click.prevent="onOpenModal">
            <span class="icon-holder">
                <i class="mdi mdi-link-variant-plus"/>
            </span>
            <span class="title">{{ $t('Create link') }}</span>
        </a>
        <b-modal
            :hide-footer="true"
            :title="$t('Create link')"
            @hidden="clearLink"
            centered
            id="create-link"
            no-fade
            size="lg"
        >
            <template v-slot:default>
                <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
                <template v-if="shortened">
                    <b-row cols="1">
                        <b-col>
                            <b-input-group class="mb-2">
                                <b-form-input id="shortened-input" readonly v-model="shortened"/>
                                <template v-slot:append>
                                    <b-button
                                        :title="$t('Copy link')"
                                        class="px-2 mb-0"
                                        type="button"
                                        v-b-tooltip.hover
                                        variant="primary"
                                        @click="copyLink"
                                    >
                                        <i class="fas fa-fw fa-copy"/>
                                        {{ $t('Copy link') }}
                                    </b-button>
                                </template>
                            </b-input-group>
                        </b-col>
                        <b-col>
                            <div class="float-right mt-3">
                                <b-button variant="danger" @click="$bvModal.hide('create-link')">
                                    <i class="fas fa-times"/>
                                    {{ $t('Close') }}
                                </b-button>
                                <b-button type="button" variant="success" @click="shortened = null">
                                    <i class="fas fa-plus"/>
                                    {{ $t('Shorten another link') }}
                                </b-button>
                            </div>
                        </b-col>
                    </b-row>
                </template>
                <template v-else>
                    <form @submit.prevent="addLink" ref="addLink">
                        <b-row cols="1">
                            <b-col>
                                <b-input-group class="mb-2">
                                    <b-form-input
                                        id="url"
                                        required
                                        type="url"
                                        v-model="link.url"
                                        :placeholder="$t('Enter long url') + '...'"
                                    />
                                    <template v-slot:append>
                                        <b-dropdown
                                            variant="light"
                                            :text="linkTypes[link.type].title"
                                        >
                                            <template v-for="(item, index) in  linkTypes">
                                                <b-dropdown-item @click="link.type = index">
                                                    {{ item.title }}<br><small>{{ item.description }}</small>
                                                </b-dropdown-item>
                                            </template>
                                        </b-dropdown>
                                    </template>
                                </b-input-group>
                            </b-col>
                            <b-col>
                                <b-button @click="advancedOptions = !advancedOptions" variant="light">
                                    <i class="mdi mdi-cog"></i>
                                    {{ $t('Advanced options') }}
                                </b-button>
                                <b-collapse :visible="advancedOptions" id="advanced_options">
                                    <hr class="mt-0 mb-3">
                                    <b-row cols="2">
                                        <b-col>
                                            <b-form-group class="mb-2" label-for="code">
                                                <template v-slot:label>{{ $t('Code') }}</template>
                                                <b-form-input id="code" v-model="link.code" max="50"/>
                                            </b-form-group>
                                        </b-col>
                                        <b-col>
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
                                        </b-col>
                                        <b-col>
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
                                        </b-col>
                                        <b-col>
                                            <b-form-group class="mb-2" label-for="password">
                                                <template v-slot:label>{{ $t('Password') }}</template>
                                                <b-form-input type="password" id="password" v-model="link.password"/>
                                            </b-form-group>
                                        </b-col>
                                    </b-row>
                                    <hr class="my-3">
                                    <b-row cols="1">
                                        <b-col>
                                            <p>
                                                <b>{{ $t('Tags and Description') }}</b>
                                                <br>
                                                <small>{{ $t('Add tags and description to easily find your links') }}.</small>
                                            </p>
                                        </b-col>
                                        <b-col>
                                            <b-form-group class="mb-2" label-for="title">
                                                <template v-slot:label>{{ $t('Title') }}</template>
                                                <b-form-input id="title" v-model="link.title"/>
                                            </b-form-group>
                                        </b-col>
                                        <b-col>
                                            <b-form-group class="mb-2" label-for="tags">
                                                <template v-slot:label>{{ $t('Tags') }}</template>
                                                <b-form-tags input-id="tags" v-model="link.tags" :disableAddButton="true"/>
                                            </b-form-group>
                                        </b-col>
                                        <b-col>
                                            <b-form-group class="mb-2" label-for="description">
                                                <template v-slot:label>{{ $t('Description') }}</template>
                                                <b-textarea id="description" v-model="link.description"/>
                                            </b-form-group>
                                        </b-col>
                                    </b-row>
                                </b-collapse>
                            </b-col>
                            <b-col>
                                <div class="float-right mt-3">
                                    <b-button variant="danger" @click="$bvModal.hide('create-link')">
                                        <i class="fas fa-times"/>
                                        {{ $t('Close') }}
                                    </b-button>
                                    <b-button type="submit" variant="success" class="btn-short-link" data-style="zoom-in">
                                        <i class="fas fa-plus"/>
                                        {{ $t('Shorten link') }}
                                    </b-button>
                                </div>
                            </b-col>
                        </b-row>
                    </form>
                </template>
            </template>
        </b-modal>
    </li>
</template>

<script>
    import {Datetime} from 'vue-datetime';

    export default {
        name: "NewLink",
        components: {
            datetime: Datetime
        },
        data() {
            return {
                loading: false,
                advancedOptions: false,
                link: {
                    type: 1,
                    url: null,
                    code: null,
                    disabled: false,
                    archived: false,
                    password: null,
                    domain_id: null,
                    expires_at: null,
                    title: null,
                    tags: [],
                    description: null,
                },
                shortened: null,
                linkTypes: {
                    1: {
                        title: this.$i18n.t('Direct'),
                        description: this.$i18n.t('Redirect user to url instantly.')
                    },
                },
                domains: [],
            }
        },
        methods: {
            onOpenModal() {
                this.advancedOptions = false;
                this.$bvModal.show('create-link');
                this.getDomains();
            },
            clearLink() {
                this.link = {
                    type: 1,
                    url: null,
                    code: null,
                    disabled: false,
                    archived: false,
                    password: null,
                    domain_id: null,
                    expires_at: null,
                    title: null,
                    tags: [],
                    description: null,
                };
                this.shortened = null;
                this.advancedOptions = false;
            },
            addLink() {
                let self = this;
                self.loading = true;
                axios.post('api/dashboard/links', self.link).then(function (response) {
                    self.clearLink();
                    self.loading = false;
                    self.$root.$emit('updateLinksList');
                    self.shortened = response.data.data.link.link;
                }).catch(function () {
                    self.loading = false;
                });
            },
            copyLink() {
                let copyText = document.getElementById('shortened-input')
                copyText.select();
                copyText.setSelectionRange(0, 99999)
                document.execCommand("copy");
                this.$snack.success({button: false, text: this.$i18n.t('Link copied to clipboard')});
            },
            getDomains() {
                let self = this;
                self.loading = true;
                axios.get('api/dashboard/links/domains').then(function (response) {
                    self.loading = false;
                    self.domains = response.data.data;
                }).catch(function () {
                    self.$bvModal.hide('create-link');
                });
            },
        }
    }
</script>
