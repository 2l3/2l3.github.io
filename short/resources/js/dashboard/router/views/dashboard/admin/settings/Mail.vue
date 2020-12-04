<template>
    <form @submit.prevent="$parent.updateSettings">
        <b-card>
            <h4 class="mb-0">{{ $t('Mail') }}</h4>
            <p class="text-muted">{{ $t('Change incoming and outgoing email handlers, email credentials and more') }}.</p>
            <div class="row mt-3">
                <div class="col-md-8">
                    <div class="form-group">
                        <div class="image-radio">
                            <label class="mt-2" for="mailgun">
                                <input id="mailgun" name="mail_mailer" type="radio" v-model="$parent.settings.mail_mailer" value="mailgun">
                                <div class="text-center">
                                    <img :src="$store.state.settings.path + '/img/icons/mailgun.svg'" class="img-fluid">
                                    <h5 class="mt-2"><b>Mailgun</b></h5>
                                </div>
                            </label>
                        </div>
                        <div class="image-radio">
                            <label class="mt-2" for="postmark">
                                <input id="postmark" name="mail_mailer" type="radio" v-model="$parent.settings.mail_mailer" value="postmark">
                                <div class="text-center">
                                    <img :src="$store.state.settings.path + '/img/icons/postmark.svg'" class="img-fluid">
                                    <h5 class="mt-2"><b>Postmark</b></h5>
                                </div>
                            </label>
                        </div>
                        <div class="image-radio">
                            <label class="mt-2" for="ses">
                                <input id="ses" name="mail_mailer" type="radio" v-model="$parent.settings.mail_mailer" value="ses">
                                <div class="text-center">
                                    <img :src="$store.state.settings.path + '/img/icons/aws-ses.svg'" class="img-fluid">
                                    <h5 class="mt-2"><b>AWS SES</b></h5>
                                </div>
                            </label>
                        </div>
                        <div class="image-radio">
                            <label class="mt-2" for="smtp">
                                <input id="smtp" name="mail_mailer" type="radio" v-model="$parent.settings.mail_mailer" value="smtp">
                                <div class="text-center">
                                    <img :src="$store.state.settings.path + '/img/icons/server.svg'" class="img-fluid">
                                    <h5 class="mt-2"><b>SMTP</b></h5>
                                </div>
                            </label>
                        </div>
                        <div class="image-radio">
                            <label class="mt-2" for="log">
                                <input id="log" name="mail_mailer" type="radio" v-model="$parent.settings.mail_mailer" value="log">
                                <div class="text-center">
                                    <img :src="$store.state.settings.path + '/img/icons/file.svg'" class="img-fluid">
                                    <h5 class="mt-2"><b>{{ $t('Log File') }}</b></h5>
                                </div>
                            </label>
                        </div>
                        <small class="form-text text-muted">{{ $t('Select the method of output of the emails') }}.</small>
                    </div>
                    <div v-if="$parent.settings.mail_mailer === 'mailgun'">
                        <div class="form-group">
                            <label for="mailgun_domain">{{ $t('Mailgun domain') }}</label>
                            <input class="form-control" id="mailgun_domain" type="text" v-model="$parent.settings.mailgun_domain">
                        </div>
                        <div class="form-group">
                            <label for="mailgun_secret">{{ $t('Mailgun secret') }}</label>
                            <input class="form-control" id="mailgun_secret" type="text" v-model="$parent.settings.mailgun_secret">
                        </div>
                        <div class="form-group mb-0">
                            <label for="mailgun_endpoint">{{ $t('Mailgun endpoint') }}</label>
                            <input class="form-control" id="mailgun_endpoint" type="text" v-model="$parent.settings.mailgun_endpoint">
                        </div>
                    </div>
                    <div v-if="$parent.settings.mail_mailer === 'postmark'">
                        <div class="form-group">
                            <label for="postmark_token">{{ $t('Postmark token') }}</label>
                            <input class="form-control" id="postmark_token" type="text" v-model="$parent.settings.postmark_token">
                        </div>
                    </div>
                    <div v-if="$parent.settings.mail_mailer === 'ses'">
                        <div class="form-group">
                            <label for="aws_access_key_id">{{ $t('AWS SES key') }}</label>
                            <input class="form-control" id="aws_access_key_id" type="text" v-model="$parent.settings.aws_access_key_id">
                        </div>
                        <div class="form-group">
                            <label for="aws_secret_access_key">{{ $t('AWS SES secret') }}</label>
                            <input class="form-control" id="aws_secret_access_key" type="text" v-model="$parent.settings.aws_secret_access_key">
                        </div>
                        <div class="form-group mb-0">
                            <label for="aws_default_region">{{ $t('AWS SES region') }}</label>
                            <input class="form-control" id="aws_default_region" type="text" v-model="$parent.settings.aws_default_region">
                        </div>
                    </div>
                    <div v-else-if="$parent.settings.mail_mailer === 'smtp'">
                        <div class="form-group">
                            <label for="smtp_host">{{ $t('Mail host') }}</label>
                            <input class="form-control" id="smtp_host" type="text" v-model="$parent.settings.mail_host">
                        </div>
                        <div class="form-group">
                            <label for="smtp_port">{{ $t('Mail port') }}</label>
                            <input class="form-control" id="smtp_port" type="text" v-model="$parent.settings.mail_port">
                        </div>
                        <div class="form-group">
                            <label for="smtp_username">{{ $t('Mail username') }}</label>
                            <input class="form-control" id="smtp_username" type="text" v-model="$parent.settings.mail_username">
                        </div>
                        <div class="form-group">
                            <label for="smtp_password">{{ $t('Mail password') }}</label>
                            <input class="form-control" id="smtp_password" type="text" v-model="$parent.settings.mail_password">
                        </div>
                        <div class="form-group mb-0">
                            <label for="smtp_encryption">{{ $t('Mail encryption') }}</label>
                            <input class="form-control" id="smtp_encryption" type="text" v-model="$parent.settings.mail_encryption">
                        </div>
                    </div>
                    <div v-else-if="$parent.settings.mail_mailer === 'log'">
                        <div class="alert alert-success">
                            {{ $t('All emails will be kept in a file in the folder :path with the date and its contents').replace(':path', '/storage/logs') }}.
                        </div>
                    </div>
                    <hr class="my-3">
                    <div class="form-group">
                        <label for="from_address">{{ $t('From address') }}</label>
                        <input class="form-control" id="from_address" type="text" v-model="$parent.settings.mail_from_address">
                    </div>
                    <div class="form-group">
                        <label for="from_name">{{ $t('From name') }}</label>
                        <input class="form-control" id="from_name" type="text" v-model="$parent.settings.mail_from_name">
                    </div>
                </div>
            </div>
            <button class="btn btn-success btn-submit m-0" data-style="zoom-in" type="submit">
                <i class="fas fa-save"/>
                {{ $t('Update') }}
            </button>
        </b-card>
    </form>
</template>

<script>
    export default {
        name: "Mail"
    }
</script>
