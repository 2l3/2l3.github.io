<template>
    <b-card class="mb-4" no-body>
        <template v-slot:header>
            <p class="m-0 p-0 normal-line-height">
                {{ $t('Referrers') }}
                <br>
                <small>{{ $t('Click count by referrer') }}</small>
            </p>
        </template>
        <b-card-body body-class="p-0 list-referrer">
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
                <div class="overflow-auto pt-1 pb-2 px-3" style="max-height: 100%">
                    <ul class="list-unstyled">
                        <template v-for="referrer in data">
                            <li class="py-1">
                                <span class="d-inline-block text-truncate" style="max-width: calc(100% - 3em);">
                                    <a :href="referrer.label" v-if="referrer.code">{{ referrer.label }}</a>
                                    <span v-else>{{ referrer.label }}</span>
                                </span>
                                <span class="badge badge-primary float-right">{{ referrer.value }}</span>
                            </li>
                        </template>
                    </ul>
                </div>
            </template>
        </b-card-body>
    </b-card>
</template>

<script>
    export default {
        name: "ReferrersGraph",
        props: {
            data: Array,
            loading: {
                type: Boolean,
                default: true
            }
        }
    }
</script>
