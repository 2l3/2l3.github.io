<template>
    <div class="list-admin-links-component">
        <div class="row">
            <div class="col-12">
                <div class="d-flex align-items-center justify-content-between mb-4">
                    <h1 class="h3 mb-0 text-gray-800">{{ $t('Links') }}</h1>
                </div>
            </div>
            <div class="col-12">
                <div class="input-group input-search-group mb-3">
                    <b-input :placeholder="$t('Type to search')" class="form-control search-input" type="text" v-model="search" v-on:change="getLinks"/>
                    <i class="fas fa-search search-icon"/>
                </div>
                <b-card
                    class="mb-3"
                    no-body
                >
                    <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
                    <b-table
                        :fields="[
                        {key: 'summary', label: $t('Summary'), thStyle: {minWidth: '280px'}},
                        {key: 'user', label: $t('User'), thStyle: {width: '150px'}},
                        {key: 'visits', label: $t('Visits'), thStyle: {width: '90px'}},
                        {key: 'type', label: $t('Type'), thStyle: {width: '90px'}},
                        {key: 'password', label: $t('Password'), thStyle: {width: '110px'}},
                        {key: 'expires_at', label: $t('Expires at'), thStyle: {width: '130px'}},
                        {key: 'updated_at', label: $t('Updated at'), thStyle: {width: '130px'}},
                        {key: 'actions', label: '', thStyle: {width: '160px', width: '200px'}},
                    ]"
                        :items="linksList"
                        :responsive="true"
                        fixed
                        hover
                        show-empty
                        small
                        table-class="mb-0"
                    >
                        <template v-slot:cell(summary)="data">
                            <p class="m-0 normal-line-height">
                                <b-link :href="data.item.url" target="_blank" class="d-inline-block text-truncate" style="max-width: 100%;">
                                    <b-img :src="data.item.favicon" width="16px"/>
                                    {{ data.item.url | clearPrintUrl }}
                                </b-link>
                                <br>
                                <small>
                                    <b-link :href="data.item.link" target="_blank">{{ data.item.link }}</b-link>
                                </small>
                            </p>
                        </template>
                        <template v-slot:cell(user)="data">
                            <p class="m-0 normal-line-height">
                                <span class="d-inline-block text-truncate align-middle" style="max-width: 100%;">
                                    <template v-if="data.item.user">
                                        <b-avatar variant="light" :src="data.item.user.avatar" class="mr-2" :size="28"/>
                                        <span>{{ data.item.user.full_name }}</span>
                                    </template>
                                    <template v-else>
                                        <b-avatar class="mr-2" :size="28"/>
                                        <span>{{ $t('Anonymous') }}</span>
                                    </template>
                                </span>
                            </p>
                        </template>
                        <template v-slot:cell(visits)="data">
                            {{ data.item.visits }}
                        </template>
                        <template v-slot:cell(type)="data">
                            <span v-if="data.item.type === 1">{{ $t('Direct') }}</span>
                        </template>
                        <template v-slot:cell(password)="data">
                            <template v-if="!data.item.has_password">
                                <span class="status success"/>
                                <span class="text-success text-semibold m-l-5">{{ $t('Public') }}</span>
                            </template>
                            <template v-else>
                                <span class="status danger"/>
                                <span class="text-danger text-semibold m-l-5">{{ $t('Protected') }}</span>
                            </template>
                        </template>
                        <template v-slot:cell(expires_at)="data">
                            <template v-if="data.item.expires_at">{{ data.item.expires_at | momentDateTime }}</template>
                            <template v-else>-</template>
                        </template>
                        <template v-slot:cell(updated_at)="data">
                            {{ data.item.updated_at | momentDateTime }}
                        </template>
                        <template v-slot:cell()="data">
                            <b-button
                                :title="$t('Link analytics')"
                                :to="'/dashboard/admin/links/analytics/' + data.item.id"
                                class="px-2 mb-0"
                                size="xs"
                                type="button"
                                v-b-tooltip.hover
                                variant="primary"
                            >
                                <i class="fas fa-fw fa-chart-area"/>
                            </b-button>
                            <b-button
                                :title="$t('Copy link')"
                                class="px-2 mb-0"
                                size="xs"
                                type="button"
                                v-b-tooltip.hover
                                variant="info"
                                v-clipboard:copy="data.item.link"
                                v-clipboard:success="linkCopied"
                            >
                                <i class="fas fa-fw fa-copy"/>
                            </b-button>
                            <b-button
                                :title="$t('Edit link')"
                                :to="'/dashboard/admin/links/edit/' + data.item.id"
                                class="px-2 mb-0"
                                size="xs"
                                type="button"
                                v-b-tooltip.hover
                                variant="success"
                            >
                                <i class="fas fa-fw fa-edit"/>
                            </b-button>
                            <b-button
                                :title="$t('Delete link')"
                                @click="deleteLink(data.item.id)"
                                class="px-2 mb-0"
                                size="xs"
                                type="button"
                                v-b-tooltip.hover
                                variant="danger"
                            >
                                <i class="fas fa-fw fa-trash"/>
                            </b-button>
                        </template>
                        <template v-slot:empty="scope">
                            <div class="text-center my-5">
                                <img
                                    :alt="$t('No results')"
                                    class="mb-5"
                                    src="/img/icons/robot.svg"
                                    style="max-height: 165px;"
                                >
                                <h3 class="bold">{{ $t('No records found') }}</h3>
                            </div>
                        </template>
                    </b-table>
                </b-card>
                <div class="row align-items-center">
                    <div class="col-lg-6 col-md-12">
                        <div class="form-group d-inline-flex mb-0">
                            <select class="form-control" style="width: 70px;" v-model="perPage" v-on:change="getLinks">
                                <option value="5">5</option>
                                <option value="10">10</option>
                                <option value="25">25</option>
                                <option value="50">50</option>
                            </select>
                        </div>
                        <div class="d-inline-flex ml-2">
                            <b-pagination
                                :per-page="perPage"
                                :total-rows="pagination.total"
                                @change="changePage"
                                v-model="page"
                            />
                        </div>
                    </div>
                    <div class="col-lg-6 col-md-12 mt-4 mt-lg-0">
                        <div class="float-lg-right">
                            {{ $t('Showing page') }} {{ pagination.currentPage }} {{ $t('of') }} {{ pagination.totalPages }} {{ $t('pages in') }} {{ pagination.total }} {{ $t('records') }}
                        </div>
                    </div>
                </div>
            </div>
            <router-view/>
        </div>
    </div>
</template>

<script>
    import store from "../../../../store";

    export default {
        name: "Links",
        metaInfo() {
            return {
                title: this.$i18n.t('Links')
            }
        },
        filters: {
            momentDateTime: function (value) {
                moment.locale(store.state.settings.date_locale);
                return moment(value).format(store.state.settings.date_format + ' HH:mm');
            },
            clearPrintUrl(value) {
                return value.replace(/(^\w+:|^)\/\//, '');
            }
        },
        data() {
            return {
                page: 1,
                search: '',
                perPage: 10,
                loading: true,
                pagination: {
                    currentPage: 0,
                    perPage: 0,
                    total: 0,
                    totalPages: 0
                },
                linksList: [],
            }
        },
        mounted() {
            let self = this;
            self.getLinks();
            self.$root.$on('updateLinksList', function () {
                self.getLinks()
            })
        },
        methods: {
            // Change the page
            changePage(page) {
                let self = this;
                if ((page > 0) && (page <= self.pagination.totalPages) && (page !== self.page)) {
                    self.page = page;
                    self.getLinks();
                }
            },
            // Get links list
            getLinks() {
                let self = this;
                self.loading = true;
                axios.get('api/dashboard/admin/links', {
                    params: {
                        page: self.page,
                        search: self.search,
                        perPage: self.perPage
                    }
                }).then(function (response) {
                    self.linksList = response.data.data;
                    self.pagination = response.data.pagination;
                    if (self.pagination.totalPages < self.pagination.currentPage) {
                        self.page = self.pagination.totalPages;
                        self.getLinks();
                    } else {
                        self.loading = false;
                    }
                }).catch(function () {
                    self.loading = false;
                });
            },
            // Delete link
            deleteLink(link) {
                let self = this;
                swal({
                    title: self.$t('Are you sure?'),
                    text: self.$t('By removing this link, you will not be able to access it from the link and the analytics and visits will be removed'),
                    buttons: {
                        cancel: self.$t('Cancel'),
                        confirm: self.$t('Delete'),
                    },
                    icon: "warning",
                }).then((value) => {
                    if (value) {
                        self.loading = true;
                        axios.delete('api/dashboard/admin/links/' + link).then(function (response) {
                            self.getLinks();
                            self.$snack.success({
                                button: false,
                                text: response.data.data.message
                            });
                        }).catch(function () {
                            self.loading = false;
                        });
                    }
                });
            },
            linkCopied() {
                this.$snack.success({button: false, text: this.$i18n.t('Link copied to clipboard')});
            },
        },
    }
</script>

<style lang="scss">
    .list-admin-links-component {
        table {
            min-width: 1200px !important;
        }
    }
</style>
