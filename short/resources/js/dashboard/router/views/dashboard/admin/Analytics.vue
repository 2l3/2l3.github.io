<template>
    <div class="row">
        <div class="col-12">
            <div class="d-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-0 text-gray-800">{{ $t('Analytics') }}</h1>
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
            <div class="row">
                <div class="col-lg-3 col-md-6">
                    <b-card body-class="pb-2">
                        <loading-animation :active.sync="report.totalLinks == null" :height="50" :is-full-page="false" :opacity="1" :width="50" color="#3a1143"/>
                        <div class="media">
                            <div class="align-self-center">
                                <i class="mdi mdi-link-variant font-size-50 text-primary"/>
                            </div>
                            <div class="m-l-20">
                                <p class="m-b-0">{{ $t('Total links') }}</p>
                                <h2 class="font-weight-light m-b-0">{{ report.totalLinks }}</h2>
                            </div>
                        </div>
                    </b-card>
                </div>
                <div class="col-lg-3 col-md-6">
                    <b-card body-class="pb-2">
                        <loading-animation :active.sync="report.totalClicks == null" :height="50" :is-full-page="false" :opacity="1" :width="50" color="#3a1143"/>
                        <div class="media">
                            <div class="align-self-center">
                                <i class="mdi mdi-cursor-default-click-outline font-size-50 text-primary"/>
                            </div>
                            <div class="m-l-20">
                                <p class="m-b-0">{{ $t('Total clicks') }}</p>
                                <h2 class="font-weight-light m-b-0">{{ report.totalClicks }}</h2>
                            </div>
                        </div>
                    </b-card>
                </div>
                <div class="col-lg-3 col-md-6">
                    <b-card body-class="pb-2">
                        <loading-animation :active.sync="report.totalUsers == null" :height="50" :is-full-page="false" :opacity="1" :width="50" color="#3a1143"/>
                        <div class="media">
                            <div class="align-self-center">
                                <i class="mdi mdi-account-group-outline font-size-50 text-primary"/>
                            </div>
                            <div class="m-l-20">
                                <p class="m-b-0">{{ $t('Total users') }}</p>
                                <h2 class="font-weight-light m-b-0">{{ report.totalUsers }}</h2>
                            </div>
                        </div>
                    </b-card>
                </div>
                <div class="col-lg-3 col-md-6">
                    <b-card body-class="pb-2">
                        <loading-animation :active.sync="report.totalDomains == null" :height="50" :is-full-page="false" :opacity="1" :width="50" color="#3a1143"/>
                        <div class="media">
                            <div class="align-self-center">
                                <i class="mdi mdi-web font-size-50 text-primary"/>
                            </div>
                            <div class="m-l-20">
                                <p class="m-b-0">{{ $t('Total domains') }}</p>
                                <h2 class="font-weight-light m-b-0">{{ report.totalDomains }}</h2>
                            </div>
                        </div>
                    </b-card>
                </div>
            </div>
        </div>
        <div class="col-md-8">
            <clicks-graph :loading="loading" :data="report.clicks" :range="range"/>
        </div>
        <div class="col-md-4">
            <devices-graph :loading="loading" :data="report.devices"/>
        </div>
        <div class="col-md-6">
            <referrers-graph :loading="loading" :data="report.referrers"/>
        </div>
        <div class="col-md-6">
            <locations-graph :loading="loading" :data="report.locations"/>
        </div>
        <div class="col-md-4">
            <browsers-graph :loading="loading" :data="report.browsers"/>
        </div>
        <div class="col-md-4">
            <platforms-graph :loading="loading" :data="report.platforms"/>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Analytics",
        metaInfo() {
            return {
                title: this.$i18n.t('Analytics')
            }
        },
        mounted() {
            this.getAnalytics()
        },
        data() {
            return {
                loading: true,
                range: 'weekly',
                report: {
                    platforms: [],
                    devices: [],
                    browsers: [],
                    locations: {},
                    referrers: [],
                    clicks: [],
                    totalUsers: null,
                    totalLinks: null,
                    totalClicks: null,
                    totalDomains: null,
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
                self.loading = true;
                axios.get('api/dashboard/admin/analytics/report?range=' + self.range).then(function (response) {
                    self.loading = false;
                    self.report = response.data.data;
                }).catch(function () {
                    self.loading = false;
                });
            }
        }
    }
</script>
