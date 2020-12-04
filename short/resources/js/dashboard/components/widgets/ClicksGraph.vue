<template>
    <b-card class="mb-4" no-body>
        <template v-slot:header>
            <p class="m-0 p-0 normal-line-height">
                {{ $t('Clicks') }}
                <br>
                <small>{{ $t('Total clicks by date') }}</small>
            </p>
        </template>
        <b-card-body class="p-0 area-chart">
            <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
            <area-chart
                :data="data"
                :labels='[$t("Clicks")]'
                :xLabelFormat="dateFormat"
                class="area-chart-graph"
                grid="true"
                grid-text-weight="bold"
                id="area-chart"
                line-colors='["#3a1143"]'
                parseTime="false"
                resize="true"
                xkey="date"
                ykeys='["clicks"]'
            />
        </b-card-body>
    </b-card>
</template>

<script>
    import Raphael from 'raphael/raphael';
    import {AreaChart} from 'vue-morris'

    global.Raphael = Raphael;

    export default {
        name: "ClicksGraph",
        components: {
            AreaChart
        },
        props: {
            data: Array,
            range: {
                type: String,
                default: 'weekly'
            },
            loading: {
                type: Boolean,
                default: true
            }
        },
        methods: {
            dateFormat(d) {
                moment.locale(window.config.date_locale);
                return moment(d.label).format(this.range === 'yearly' ? 'MMMM' : this.range === 'monthly' ? 'DD, MMMM' : 'DD, dddd');
            },
        }
    }
</script>
