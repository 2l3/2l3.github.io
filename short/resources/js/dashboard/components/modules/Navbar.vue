<template>
    <div class="header navbar">
        <div class="header-container">
            <div class="nav-logo">
                <router-link class="logo" to="/dashboard">
                    <img :src="$store.state.settings.logo" alt="logo">
                    <span>{{ $store.state.settings.name }}</span>
                </router-link>
            </div>
            <ul class="nav-left">
                <li>
                    <a @click.prevent="foldSidebar" class="sidenav-fold-toggler" href="#">
                        <i class="mdi mdi-menu"/>
                    </a>
                    <a @click.prevent="expandSidebar" class="sidenav-expand-toggler" href="#">
                        <i class="mdi mdi-menu"/>
                    </a>
                </li>
            </ul>
            <ul class="nav-right">
                <li class="user-profile dropdown dropdown-animated scale-left">
                    <a class="dropdown-toggle" data-toggle="dropdown" href="#">
                        <img :alt="$t('User avatar')" :src="$store.state.user.avatar" class="profile-img img-fluid" v-if="$store.state.user.avatar">
                    </a>
                    <ul class="dropdown-menu dropdown-md p-v-0">
                        <li>
                            <ul class="list-media">
                                <li class="list-item p-15">
                                    <div class="media-img">
                                        <img :alt="$t('User avatar')" :src="$store.state.user.avatar">
                                    </div>
                                    <div class="info">
                                        <span class="title text-semibold">{{ $store.state.user.full_name }}</span>
                                        <span class="sub-title">{{ $store.state.user.email }}</span>
                                    </div>
                                </li>
                            </ul>
                        </li>
                        <li class="divider" role="separator"></li>
                        <li>
                            <router-link to="/dashboard/account">
                                <i class="fas fa-user-circle p-r-10"/>
                                <span>{{ $t('My account') }}</span>
                            </router-link>
                        </li>
                        <li>
                            <a @click.prevent="logout" href="#">
                                <i class="fas fa-sign-out-alt p-r-10"/>
                                <span>{{ $t('Logout') }}</span>
                            </a>
                        </li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Navbar",
        data() {
            return {
                sidebarOpen: false,
            }
        },
        destroyed() {
            document.body.classList.add('sidebar-toggled');
        },
        methods: {
            foldSidebar() {
                document.querySelectorAll('.app')[0].classList.toggle('side-nav-folded');
            },
            expandSidebar() {
                let App = $('.app');
                let SideNavBackDrop = $('.side-nav-backdrop');
                if (SideNavBackDrop.length) {
                    SideNavBackDrop.remove();
                } else {
                    let sideNavBackdrop = '<div class="side-nav-backdrop"></div>';
                    App.append(sideNavBackdrop);
                }
                App.toggleClass("side-nav-expand");
                SideNavBackDrop.on('click', function (e) {
                    App.removeClass('side-nav-expand');
                    $(this).remove();
                });
            },
            logout() {
                this.$store.commit('logout');
                this.$router.push('/auth/login');
            }
        }
    }
</script>
