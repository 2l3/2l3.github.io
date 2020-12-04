<template>
    <b-card class="mb-4" no-body>
        <template v-slot:header>
            <p class="m-0 p-0 normal-line-height">
                {{ $t('Top locations') }}
                <br>
                <small>{{ $t('Click count by country') }}</small>
            </p>
        </template>
        <b-card-body class="p-0 map-chart">
            <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
            <div class="map-chart-graph">
                <div id="world-map" class="jvmap-smart"></div>
            </div>
        </b-card-body>
    </b-card>
</template>

<script>
    import 'jvectormap';
    import '../../../libs/jquery-jvectormap-world-mill';

    export default {
        name: "LocationsGraph",
        props: {
            data: [Object, Array],
            loading: {
                type: Boolean,
                default: true
            }
        },
        mounted() {
            this.drawMap();
        },
        watch: {
            data: function () {
                this.drawMap();
            }
        },
        methods: {
            drawMap() {
                let self = this;
                let map = $('#world-map');
                map.empty();
                map.vectorMap({
                    map: 'world_mill',
                    backgroundColor: 'transparent',
                    zoomOnScroll: false,
                    regionStyle: {
                        initial: {
                            fill: '#dde4ee',
                            'fill-opacity': .9,
                            stroke: 'none',
                            'stroke-width': 0,
                            'stroke-opacity': 0
                        }
                    },
                    series: {
                        regions: [{
                            values: self.data,
                            scale: ['#b096b4', '#b096b4', '#75537b', '#3a1143'],
                            normalizeFunction: 'polynomial'
                        }]
                    },
                    onRegionTipShow: function (e, el, code) {
                        el.html(el.html() + ' (' + (self.data[code] ? self.data[code] : 0) + ')');
                    }
                });
            }
        }
    }
</script>
