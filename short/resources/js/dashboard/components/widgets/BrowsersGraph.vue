<template>
    <b-card class="mb-4" no-body>
        <template v-slot:header>
            <div class="d-flex align-items-center justify-content-between">
                <p class="m-0 p-0 normal-line-height">
                    {{ $t('Top browsers') }}
                    <br>
                    <small>{{ $t('Click count by browser') }}</small>
                </p>
                <div class="float-right">
                    <b-button-group>
                        <b-button variant="outline-dark" class="mb-0" :class="display === 'graph' ? 'active' : ''" @click="display = 'graph'">
                            <i class="fas fa-chart-bar"></i>
                        </b-button>
                        <b-button variant="outline-dark" class="mb-0" :class="display === 'list' ? 'active' : ''" @click="display = 'list'">
                            <i class="fas fa-bars"></i>
                        </b-button>
                    </b-button-group>
                </div>
            </div>
        </template>
        <b-card-body class="p-0 donut-chart">
            <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
            <template v-if="Object.keys(data).length < 1">
                <div class="row align-items-center text-center empty-graph">
                    <div class="col">
                        <img
                            :alt="$t('No results')"
                            class="mb-3"
                            src="/img/graphics/statistic_chart.svg"
                            style="max-height: 100px;"
                        >
                        <h5>{{ $t('No records available') }}</h5>
                    </div>
                </div>
            </template>
            <template v-else>
                <template v-if="display === 'graph'">
                    <donut-chart
                        id="top_browsers"
                        :data="data"
                        resize="true"
                        class="donut-chart-graph"
                    />
                </template>
                <template v-else>
                    <div class="overflow-auto pt-1 pb-2 px-3" style="max-height: 100%">
                        <ul class="list-unstyled">
                            <template v-for="referrer in data">
                                <li class="py-1">
                                    <span class="d-inline-block text-truncate" style="max-width: 100%;">
                                        <img :src="$store.state.settings.path + '/img/icons/browsers/' + referrer.code + '.svg'" width="16px" class="mr-1">
                                        {{ referrer.label }}
                                    </span>
                                    <span class="badge badge-primary float-right">{{ referrer.value }}</span>
                                </li>
                            </template>
                        </ul>
                    </div>
                </template>
            </template>
        </b-card-body>
    </b-card>
</template>

<script>
    import Raphael from 'raphael/raphael';
    import {DonutChart} from 'vue-morris'

    global.Raphael = Raphael;

    export default {
        name: "BrowsersGraph",
        components: {
            DonutChart
        },
        props: {
            data: Array,
            loading: {
                type: Boolean,
                default: true
            }
        },
        data() {
            return {
                display: 'graph'
            }
        }
    }
</script>
