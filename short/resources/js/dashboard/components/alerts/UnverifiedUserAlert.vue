<template>
    <div class="unverified-user-alert-component">
        <b-alert :show="display" class="mb-0" variant="danger">
            <b class="text-uppercase">{{ $t('Unverified user') }}</b>
            <span class="float-right">{{ $t('Access your profile to verify the user\'s email') }}.</span>
        </b-alert>
    </div>
</template>

<script>
    export default {
        name: "UnverifiedUserAlert",
        props: ['display']
    }
</script>

<style lang="scss">
    .unverified-user-alert-component {
        .alert {
            position: fixed !important;
            width: 100% !important;
            z-index: 1000 !important;
            border: none !important;
            color: #ffffff !important;
            background-color: #dc3545 !important;
            -webkit-transition: all 0.2s ease;
        }
    }
</style>
