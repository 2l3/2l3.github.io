<template>
    <div class="row">
        <div class="col-12">
            <div class="d-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-0 text-gray-800">{{ $t('Link analytics') }}</h1>
                <b-button-group>
                    <b-button variant="outline-dark" :class="range === 'weekly' ? 'active' : ''" @click="updateAnalytics('weekly')">
                        <i class="fas fa-calendar-week"></i>
                        {{ $t('Weekly') }}
                    </b-button>
                    <b-button variant="outline-dark" :class="range === 'monthly' ? 'active' : ''" @click="updateAnalytics('monthly')">
                        <i class="fas fa-calendar-alt"></i>
                        {{ $t('Monthly') }}
                    </b-button>
                    <b-button variant="outline-dark" :class="range === 'yearly' ? 'active' : ''" @click="updateAnalytics('yearly')">
                        <i class="fas fa-calendar"></i>
                        {{ $t('Yearly') }}
                    </b-button>
                </b-button-group>
            </div>
        </div>
        <div class="col-12">
            <b-card no-body>
                <b-card-body>
                    <div class="row">
                        <div class="col-xl-4">
                            <loading-animation :active.sync="loading.preview" :is-full-page="false" :opacity="1" color="#3a1143"/>
                            <div class="text-center" style="height: 179px;">
                                <img height="100%" :alt="$t('Preview')" :src="preview.image ? 'data:' + preview.mime + ';base64,' + preview.image : null"/>
                            </div>
                        </div>
                        <div class="col-xl-8">
                            <loading-animation :active.sync="loading.report && !report.link.title" :is-full-page="false" :opacity="1" color="#3a1143"/>
                            <div class="row">
                                <div class="col-md-7">
                                    <h4 class="pt-2 m-0">{{ report.link.title }}</h4>
                                    <p class="m-0">{{ report.link.description }}</p>
                                    <small>
                                        <b-badge
                                            v-for="tag in report.link.tags"
                                            :key="tag"
                                            class="mr-1"
                                            variant="primary"
                                        >
                                            {{ tag }}
                                        </b-badge>
                                    </small>
                                </div>
                                <div class="col-md-5 text-right">
                                    <p>{{ report.link.created_at | momentFormatAgo }}</p>
                                    <p class="align-middle">
                                        <b-button
                                            v-b-tooltip.hover
                                            :title="$t('Copy link')"
                                            class="p-0 m-0"
                                            variant="link"
                                            @click="toClipboard(report.link.link)"
                                        >
                                            <i class="fas fa-fw fa-link"></i>
                                            {{ report.link.link | clearPrintUrl }}
                                        </b-button>
                                    </p>
                                    <p class="align-middle">
                                        <b-button
                                            v-b-tooltip.hover
                                            :title="$t('Copy link')"
                                            class="p-0 m-0"
                                            variant="link"
                                            @click="toClipboard(report.link.qr)"
                                        >
                                            <i class="fas fa-fw fa-qrcode"></i>
                                            {{ report.link.qr | clearPrintUrl }}
                                        </b-button>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                </b-card-body>
            </b-card>
        </div>
        <div class="col-md-8">
            <clicks-graph :loading="loading.report" :data="report.clicks" :range="range"/>
        </div>
        <div class="col-md-4">
            <devices-graph :loading="loading.report" :data="report.devices"/>
        </div>
        <div class="col-md-6">
            <referrers-graph :loading="loading.report" :data="report.referrers"/>
        </div>
        <div class="col-md-6">
            <locations-graph :loading="loading.report" :data="report.locations"/>
        </div>
        <div class="col-md-4">
            <browsers-graph :loading="loading.report" :data="report.browsers"/>
        </div>
        <div class="col-md-4">
            <platforms-graph :loading="loading.report" :data="report.platforms"/>
        </div>
    </div>
</template>

<script>
    import store from "../../../../store";

    export default {
        name: "Analytics",
        metaInfo() {
            return {
                title: this.$i18n.t('Link analytics')
            }
        },
        mounted() {
            this.getAnalytics();
            this.getPreview();
        },
        filters: {
            momentDateTime: function (value) {
                moment.locale(store.state.settings.date_locale);
                return moment(value).format(store.state.settings.date_format + ' HH:mm');
            },
            momentFormatAgo: function (value) {
                moment.locale(store.state.settings.date_locale);
                return moment(value).fromNow();
            },
            clearPrintUrl(value) {
                return value ? value.replace(/(^\w+:|^)\/\//, '') : null;
            }
        },
        data() {
            return {
                loading: {
                    report: true,
                    preview: true,
                },
                range: 'weekly',
                preview: [],
                report: {
                    link: {},
                    platforms: [],
                    devices: [],
                    browsers: [],
                    locations: {},
                    referrers: [],
                    clicks: [],
                },
            }
        },
        methods: {
            updateAnalytics(range) {
                if (this.range !== range) {
                    this.range = range;
                    this.getAnalytics();
                }
            },
            getAnalytics() {
                let self = this;
                self.loading.report = true;
                axios.get('api/dashboard/links/' + self.$route.params.id + '/analytics?range=' + self.range).then(function (response) {
                    self.loading.report = false;
                    self.report = response.data.data;
                }).catch(function () {
                    self.$router.push('/dashboard/links');
                });
            },
            getPreview() {
                let self = this;
                self.loading.preview = true;
                axios.get('api/dashboard/links/' + self.$route.params.id + '/preview').then(function (response) {
                    self.loading.preview = false;
                    self.preview = response.data.data;
                }).catch(function () {
                    self.loading.preview = false;
                });
            },
            toClipboard(text) {
                let self = this;
                this.$copyText(text).then(function (e) {
                    self.$snack.success({button: false, text: self.$i18n.t('Link copied to clipboard')});
                });
            },
        }
    }
</script>
