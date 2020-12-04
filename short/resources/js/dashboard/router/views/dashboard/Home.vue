<template>
    <div class="row">
        <div class="col-12">
            <div class="d-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-0 text-gray-800">{{ $t('Dashboard') }}</h1>
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
        name: "Home",
        metaInfo() {
            return {
                title: this.$i18n.t('Dashboard')
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
                axios.get('api/analytics/report?range=' + self.range).then(function (response) {
                    self.loading = false;
                    self.report = response.data.data;
                }).catch(function () {
                    self.loading = false;
                });
            }
        }
    }
</script>
