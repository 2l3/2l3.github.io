<template>
    <div :class="displayUnverifiedUserAlert ? 'display-alert' : ''" class="app header-primary">
        <unverified-user-alert :display="displayUnverifiedUserAlert"/>
        <div class="layout">
            <navbar-module/>
            <sidebar-module/>
            <div class="page-container">
                <div class="main-content">
                    <div class="container-fluid">
                        <router-view/>
                    </div>
                </div>
                <footer-module/>
            </div>
        </div>
    </div>
</template>

<script>
    import UnverifiedUserAlert from "../../components/alerts/UnverifiedUserAlert";

    export default {
        name: "Dashboard",
        components: {UnverifiedUserAlert},
        metaInfo() {
            return {
                title: this.$i18n.t('Dashboard')
            }
        },
        computed: {
            displayUnverifiedUserAlert() {
                if (this.$store.state.settings.unverified_user_alert && this.$store.state.user) {
                    return !this.$store.state.user.verified;
                }
                return false;
            }
        },
    }
</script>
