<template>
    <div class="list-domains-component">
        <div class="row">
            <div class="col-12">
                <div class="d-flex align-items-center justify-content-between mb-4">
                    <h1 class="h3 mb-0 text-gray-800">{{ $t('Domain') }}</h1>
                    <router-link class="btn btn-primary" to="/dashboard/admin/domains/add">
                        <i class="fas fa-plus"/>
                        {{ $t('Add domain') }}
                    </router-link>
                </div>
            </div>
            <div class="col-12">
                <div class="input-group input-search-group mb-3">
                    <b-input :placeholder="$t('Type to search')" class="form-control search-input" type="text" v-model="search" v-on:change="getDomains"/>
                    <i class="fas fa-search search-icon"/>
                </div>
                <b-card
                    class="mb-3"
                    no-body
                >
                    <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
                    <b-table
                        :fields="[
                        {key: 'domain', label: $t('Domain'), thStyle: {minWidth: '160px'}},
                        {key: 'actions', label: '', thStyle: {width: '160px', minWidth: '95px'}},
                    ]"
                        :items="domainsList"
                        :responsive="true"
                        fixed
                        hover
                        show-empty
                        small
                        table-class="mb-0"
                    >
                        <template v-slot:cell(domain)="data">
                            {{ data.item.domain }}
                        </template>
                        <template v-slot:cell()="data">
                            <b-button
                                :title="$t('Edit domain')"
                                :to="'/dashboard/admin/domains/edit/' + data.item.id"
                                class="px-2 mb-0"
                                size="xs"
                                type="button"
                                v-b-tooltip.hover
                                variant="success"
                            >
                                <i class="fas fa-fw fa-edit"/>
                            </b-button>
                            <b-button
                                :title="$t('Delete domain')"
                                @click="deleteDomain(data.item.id)"
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
            </div>
            <router-view/>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Domains",
        metaInfo() {
            return {
                title: this.$i18n.t('Domains')
            }
        },
        data() {
            return {
                loading: true,
                search: '',
                domainsList: [],
            }
        },
        mounted() {
            this.getDomains();
        },
        methods: {
            getDomains() {
                let self = this;
                self.loading = true;
                axios.get('api/dashboard/admin/domains', {
                    params: {
                        search: self.search,
                    }
                }).then(function (response) {
                    self.domainsList = response.data.data;
                    self.loading = false;
                }).catch(function () {
                    self.loading = false;
                });
            },
            deleteDomain(domain) {
                let self = this;
                swal({
                    title: self.$t('Are you sure?'),
                    text: self.$t('By deleting this domain, no new links can be created with it, and existing links will use the base domain'),
                    buttons: {
                        cancel: self.$t('Cancel'),
                        confirm: self.$t('Delete'),
                    },
                    icon: "warning",
                }).then((value) => {
                    if (value) {
                        self.loading = true;
                        axios.delete('api/dashboard/admin/domains/' + domain).then(function (response) {
                            self.getDomains();
                            self.$snack.success({
                                button: false,
                                text: response.data.data.message
                            });
                        }).catch(function () {
                            self.loading = false;
                        });
                    }
                });
            }
        }
    }
</script>
