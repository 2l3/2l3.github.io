<template>
    <div>
        <form @submit.prevent="shortLink">
            <template v-if="shortened">
                <div class="form-row">
                    <div class="col-12 col-md-9 mb-2 mb-md-0">
                        <input id="shortened-input" readonly class="form-control form-control-lg" v-model="shortened">
                    </div>
                    <div class="col-12 col-md-3">
                        <button @click.prevent="copyLink" class="btn btn-block btn-lg btn-primary" type="button">
                            <i class="fas fa-copy"></i>
                            {{ $t('Copy') }}
                        </button>
                    </div>
                </div>
            </template>
            <template v-else>
                <div class="form-row">
                    <div class="col-12 col-md-9 mb-2 mb-md-0">
                        <input type="url" class="form-control form-control-lg" :placeholder="$t('Paste a long url')" v-model="link">
                    </div>
                    <div class="col-12 col-md-3">
                        <button class="btn btn-block btn-lg btn-primary btn-submit" data-style="zoom-in" type="submit">
                            <i class="fas fa-link"></i>
                            {{ $t('Shorten') }}
                        </button>
                    </div>
                </div>
            </template>
        </form>
    </div>
</template>

<script>
    export default {
        name: "ShortLink",
        data() {
            return {
                link: null,
                shortened: null,
            }
        },
        methods: {
            shortLink() {
                let self = this;
                if (self.link) {
                    let ladda = Ladda.create(document.querySelector('.btn-submit'));
                    ladda.start();
                    axios.post('api/shorten', {url: self.link}).then(function (response) {
                        ladda.remove();
                        self.link = null;
                        self.shortened = response.data.data.link.link;
                    });
                }
            },
            copyLink() {
                let copyText = document.getElementById('shortened-input')
                copyText.select();
                copyText.setSelectionRange(0, 99999)
                document.execCommand("copy");
                this.$snack.success({button: false, text: this.$i18n.t('Link copied to clipboard')});
                this.shortened = null;
            }
        }
    }
</script>
