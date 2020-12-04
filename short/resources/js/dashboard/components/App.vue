<template>
    <router-view/>
</template>

<script>
    export default {
        name: "App",
        metaInfo() {
            return {
                title: this.$i18n.t('Dashboard'),
                titleTemplate: '%s - ' + this.$store.state.settings.name
            }
        },
    }
</script>
