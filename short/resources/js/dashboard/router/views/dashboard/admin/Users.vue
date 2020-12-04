<template>
    <div class="list-users-component">
        <div class="row">
            <div class="col-12">
                <div class="d-flex align-items-center justify-content-between mb-4">
                    <h1 class="h3 mb-0 text-gray-800">{{ $t('Users') }}</h1>
                    <router-link class="btn btn-primary" to="/dashboard/admin/users/add">
                        <i class="fas fa-user-plus"/>
                        {{ $t('Add user') }}
                    </router-link>
                </div>
            </div>
            <div class="col-12">
                <div class="input-group input-search-group mb-3">
                    <input :placeholder="$t('Type to search')" class="form-control search-input" type="text" v-model="search" v-on:change="getUsers">
                    <i class="fas fa-search search-icon"/>
                </div>
                <b-card
                    class="mb-3"
                    no-body
                >
                    <loading-animation :active.sync="loading" :is-full-page="false" :opacity="1" color="#3a1143"/>
                    <b-table
                        :fields="[
                        {key: 'avatar', label: '', thStyle: {width: '80px'}},
                        {key: 'name', label: $t('Name'), thStyle: {minWidth: '160px'}},
                        {key: 'email', label: $t('Email'), thStyle: {minWidth: '140px'}},
                        {key: 'verified', label: $t('Verification'), thStyle: {minWidth: '120px'}},
                        {key: 'status', label: $t('Status'), thStyle: {minWidth: '120px'}},
                        {key: 'role', label: $t('Role'), thStyle: {minWidth: '80px'}},
                        {key: 'actions', label: '', thStyle: {width: '160px', minWidth: '95px'}},
                    ]"
                        :items="userList"
                        :responsive="true"
                        fixed
                        hover
                        show-empty
                        small
                        table-class="mb-0"
                    >
                        <template v-slot:cell(avatar)="data">
                            <b-img :src="data.item.avatar" center rounded="circle" width="43px"/>
                        </template>
                        <template v-slot:cell(name)="data">
                            {{ data.item.full_name }}
                        </template>
                        <template v-slot:cell(email)="data">
                            {{ data.item.email }}
                        </template>
                        <template v-slot:cell(verified)="data">
                            <template v-if="data.item.verified">
                                <span class="status success"/>
                                <span class="text-success text-semibold m-l-5">{{ $t('Verified') }}</span>
                            </template>
                            <template v-else>
                                <span class="status danger"/>
                                <span class="text-danger text-semibold m-l-5">{{ $t('Not verified') }}</span>
                            </template>
                        </template>
                        <template v-slot:cell(status)="data">
                            <template v-if="data.item.status">
                                <span class="status success"/>
                                <span class="text-success text-semibold m-l-5">{{ $t('Activated') }}</span>
                            </template>
                            <template v-else>
                                <span class="status danger"/>
                                <span class="text-danger text-semibold m-l-5">{{ $t('Disabled') }}</span>
                            </template>
                        </template>
                        <template v-slot:cell(role)="data">
                            {{ data.item.role_details.name }}
                        </template>
                        <template v-slot:cell()="data">
                            <b-button
                                :title="$t('Edit user')"
                                :to="'/dashboard/admin/users/edit/' + data.item.id"
                                class="px-2 mb-0"
                                size="xs"
                                type="button"
                                v-b-tooltip.hover
                                v-if="data.item.id !== $store.state.user.id"
                                variant="success"
                            >
                                <i class="fas fa-fw fa-edit"/>
                            </b-button>
                            <b-button
                                :title="$t('Edit my account')"
                                :to="'/dashboard/account'"
                                class="px-2 mb-0"
                                size="xs"
                                type="button"
                                v-b-tooltip.hover
                                v-if="data.item.id === $store.state.user.id"
                                variant="success"
                            >
                                <i class="fas fa-fw fa-edit"/>
                            </b-button>
                            <b-button
                                :title="$t('Delete user')"
                                @click="deleteUser(data.item.id)"
                                class="px-2 mb-0"
                                size="xs"
                                type="button"
                                v-b-tooltip.hover
                                v-if="data.item.id !== $store.state.user.id"
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
                            <select class="form-control" style="width: 70px;" v-model="perPage" v-on:change="getUsers">
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
    export default {
        name: "Users",
        metaInfo() {
            return {
                title: this.$i18n.t('Users')
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
                userList: [],
            }
        },
        mounted() {
            this.getUsers();
        },
        methods: {
            // Change the page
            changePage(page) {
                let self = this;
                if ((page > 0) && (page <= self.pagination.totalPages) && (page !== self.page)) {
                    self.page = page;
                    self.getUsers();
                }
            },
            // Get users list
            getUsers() {
                let self = this;
                self.loading = true;
                axios.get('api/dashboard/users', {
                    params: {
                        page: self.page,
                        search: self.search,
                        perPage: self.perPage
                    }
                }).then(function (response) {
                    self.userList = response.data.data;
                    self.pagination = response.data.pagination;
                    if (self.pagination.totalPages < self.pagination.currentPage) {
                        self.page = self.pagination.totalPages;
                        self.getUsers();
                    } else {
                        self.loading = false;
                    }
                }).catch(function () {
                    self.loading = false;
                });
            },
            // Delete user
            deleteUser(user) {
                let self = this;
                swal({
                    title: self.$t('Are you sure?'),
                    text: self.$t('Removing this user will cause you to lose access and all your data will be removed from the database'),
                    buttons: {
                        cancel: self.$t('Cancel'),
                        confirm: self.$t('Delete'),
                    },
                    icon: "warning",
                }).then((value) => {
                    if (value) {
                        self.loading = true;
                        axios.delete('api/dashboard/users/' + user).then(function (response) {
                            self.getUsers();
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
        },
    }
</script>

<style lang="scss">
    .list-users-component {
        table {
            min-width: 1000px !important;
        }
    }
</style>
