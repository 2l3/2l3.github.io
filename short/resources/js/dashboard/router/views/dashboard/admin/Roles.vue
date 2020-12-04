<template>
    <div class="row">
        <div class="col-12">
            <div class="d-flex align-items-center justify-content-between mb-4">
                <h1 class="h3 mb-0 text-gray-800">{{ $t('Roles') }}</h1>
                <router-link class="btn btn-primary" to="/dashboard/admin/roles/add">
                    <i class="fas fa-plus-square"/>
                    {{ $t('Add role') }}
                </router-link>
            </div>
        </div>
        <div class="col-12">
            <div class="input-group input-search-group mb-3">
                <input :placeholder="$t('Type to search')" class="form-control search-input" type="text" v-model="search" v-on:change="getRoles">
                <i class="fas fa-search search-icon"/>
            </div>
            <b-card
                class="mb-3"
                no-body
            >
                <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
                <b-table
                    :fields="[
                        {key: 'name', label: $t('Name'), thStyle: {minWidth: '160px'}},
                        {key: 'users', label: $t('Nº Users'), thStyle: {minWidth: '100px'}},
                        {key: 'actions', label: '', thStyle: {width: '160px', minWidth: '95px'}},
                    ]"
                    :items="rolesList"
                    :responsive="true"
                    fixed
                    hover
                    show-empty
                    small
                    table-class="mb-0"
                >
                    <template v-slot:cell(name)="data">
                        {{ data.value }}
                    </template>
                    <template v-slot:cell(users)="data">
                        {{ data.value }}
                    </template>
                    <template v-slot:cell()="data">
                        <b-button
                            :title="$t('Edit role')"
                            :to="'/dashboard/admin/roles/edit/' + data.item.id"
                            class="px-2 mb-0"
                            size="xs"
                            type="button"
                            v-b-tooltip.hover
                            v-if="data.item.type !== 1"
                            variant="success"
                        >
                            <i class="fas fa-fw fa-edit"/>
                        </b-button>
                        <b-button
                            :title="$t('Delete role')"
                            @click="deleteRole(data.item.id)"
                            class="px-2 mb-0"
                            size="xs"
                            type="button"
                            v-b-tooltip.hover
                            v-if="data.item.type !== 1"
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
                    <template v-slot:emptyfiltered="scope">
                        <div class="text-center my-5">
                            <img
                                :alt="$t('No results')"
                                class="mb-5"
                                src="/img/icons/robot.svg"
                                style="max-height: 165px;"
                            >
                            <h3>{{ $t('No records match the entered search') }}</h3>
                        </div>
                    </template>
                </b-table>
            </b-card>
            <div class="row align-items-center">
                <div class="col-lg-6 col-md-12">
                    <div class="form-group d-inline-flex mb-0">
                        <select class="form-control" style="width: 70px;" v-model="perPage" v-on:change="getRoles">
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
</template>

<script>
    export default {
        name: "Roles",
        metaInfo() {
            return {
                title: this.$i18n.t('Roles')
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
                rolesList: [],
            }
        },
        mounted() {
            this.getRoles();
        },
        methods: {
            // Change the page
            changePage(page) {
                let self = this;
                if ((page > 0) && (page <= self.pagination.totalPages) && (page !== self.page)) {
                    self.page = page;
                    self.getRoles();
                }
            },
            // Get roles list
            getRoles() {
                let self = this;
                self.loading = true;
                axios.get('api/dashboard/roles', {
                    params: {
                        page: self.page,
                        search: self.search,
                        perPage: self.perPage
                    }
                }).then(function (response) {
                    self.rolesList = response.data.data;
                    self.pagination = response.data.pagination;
                    if (self.pagination.totalPages < self.pagination.currentPage) {
                        self.page = self.pagination.totalPages;
                        self.getRoles();
                    } else {
                        self.loading = false;
                    }
                }).catch(function () {
                    self.loading = false;
                });
            },
            // Delete role
            deleteRole(role) {
                let self = this;
                swal({
                    title: self.$t('Are you sure?'),
                    text: self.$t('Removing this role will cause you to lose access and all your data will be removed from the database, and users with this role will be moved to the default assigned'),
                    buttons: {
                        cancel: self.$t('Cancel'),
                        confirm: self.$t('Delete'),
                    },
                    icon: "warning",
                }).then((value) => {
                    if (value) {
                        self.loading = true;
                        axios.delete('api/dashboard/roles/' + role).then(function (response) {
                            self.getRoles();
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
        }
    }
</script>
