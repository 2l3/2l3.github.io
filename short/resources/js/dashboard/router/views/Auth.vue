<template>
    <div :style="{'background-image': 'url(' + $store.state.settings.background + ')'}" class="layout" style="background-position: center; background-size: cover; background-repeat: no-repeat;">
        <div class="container">
            <div class="row full-height align-items-center">
                <div class="col-md-7 col-lg-5 mx-auto">
                    <div class="card mt-4">
                        <div class="card-body">
                            <div class="p-15">
                                <a :href="$store.state.settings.path">
                                    <div class="m-b-30 text-center">
                                        <img class="img-fluid inline-block" style="max-height: 45px" :src="$store.state.settings.logo" alt="App logo">
                                        <h2 class="inline-block pull-right m-0 align-middle pl-3 text-uppercase">{{ $store.state.settings.name }}</h2>
                                        <hr>
                                    </div>
                                </a>
                                <router-view/>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Auth",
        metaInfo() {
            return {
                title: this.$i18n.t('Authentication')
            }
        }
    }
</script>
