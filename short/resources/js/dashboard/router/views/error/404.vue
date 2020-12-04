<template>
    <div class="error-404-component">
        <div class="layout">
            <div class="container">
                <div class="row full-height align-items-center">
                    <div class="col-12">
                        <div class="text-center p-t-50">
                            <h1 class="font-size-170 text-secondary font-weight-light text-opacity ls-2">404</h1>
                            <h2 class="font-weight-light font-size-30">{{ $t('Whoops! Looks like you got lost') }}</h2>
                            <p class="lead">{{ $t('We couldnt find what you were looking for') }}.</p>
                            <router-link class="btn btn-primary btn-lg m-t-30" to="/dashboard">{{ $t('Go back') }}</router-link>
                        </div>
                    </div>
                    <div class="col-12 align-self-end ">
                        <div class="text-center p-b-20 font-size-13">
                            {{ $t('Created with') }} <i class="fas fa-heart" style="color: #ff4c4c"/> {{ $t('by') }} <a href="https://github.com/dacoto97" rel="nofollow" target="_blank">@dacoto97</a>
                            |
                            {{ $t('Powered by') }} <i class="fab fa-laravel" style="color: #f55247;"/> Laravel
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Error",
        metaInfo() {
            return {
                title: this.$i18n.t('Page not found')
            }
        }
    }
</script>

<style lang="scss">
    .error-404-component {
        .btn {
            -moz-transition: none;
            -webkit-transition: none;
            -o-transition: none;
            transition: none;
        }
    }
</style>
