<template>
    <footer class="content-footer">
        <div class="footer">
            <div class="copyright">
                <span class="go-right mb-3">
                    {{ $t('Created with') }} <i class="fas fa-heart" style="color: #ff4c4c"/> {{ $t('by') }} <a href="https://github.com/dacoto97" rel="nofollow" target="_blank">@dacoto97</a> |
                    {{ $t('Powered by') }} <i class="fab fa-laravel" style="color: #f55247;"/> Laravel
                </span>
            </div>
        </div>
    </footer>
</template>

<script>
    export default {
        name: "Footer"
    }
</script>
