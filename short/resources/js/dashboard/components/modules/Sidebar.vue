<template>
    <div class="side-nav expand-lg">
        <div class="side-nav-inner">
            <ul class="side-nav-menu scrollable">
                <li class="side-nav-header">
                    <span>{{ $t('General') }}</span>
                </li>
                <new-link v-if="$store.state.user.permissions && $store.state.user.permissions['LinkController']"/>
                <router-link active-class="" class="nav-item" exact-active-class="active" tag="li" to="/dashboard/home">
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-gauge"/>
                        </span>
                        <span class="title">{{ $t('Dashboard') }}</span>
                    </a>
                </router-link>
                <router-link
                    active-class="active"
                    class="nav-item"
                    exact-active-class="active"
                    tag="li"
                    to="/dashboard/links"
                    v-if="$store.state.user.permissions && $store.state.user.permissions['LinkController']"
                >
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-link-variant"/>
                        </span>
                        <span class="title">{{ $t('Links') }}</span>
                    </a>
                </router-link>
                <li
                    class="side-nav-header"
                    v-if="
                        $store.state.user.permissions && (
                            $store.state.user.permissions['AdminLinkController'] ||
                            $store.state.user.permissions['AdminDomainController'] ||
                            $store.state.user.permissions['UserController'] ||
                            $store.state.user.permissions['RoleController'] ||
                            $store.state.user.permissions['SettingController'] ||
                            $store.state.user.permissions['LanguageController']
                        )
                    "
                >
                    <span>{{ $t('Administration') }}</span>
                </li>
                <router-link
                    active-class="active"
                    class="nav-item"
                    exact-active-class="active"
                    tag="li"
                    to="/dashboard/admin/analytics"
                    v-if="$store.state.user.permissions && $store.state.user.permissions['AdminAnalyticsController']">
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-chart-areaspline"/>
                        </span>
                        <span class="title">{{ $t('Analytics') }}</span>
                    </a>
                </router-link>
                <router-link
                    active-class="active"
                    class="nav-item"
                    exact-active-class="active"
                    tag="li"
                    to="/dashboard/admin/links"
                    v-if="$store.state.user.permissions && $store.state.user.permissions['AdminLinkController']">
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-link-variant"/>
                        </span>
                        <span class="title">{{ $t('Links') }}</span>
                    </a>
                </router-link>
                <router-link
                    active-class="active"
                    class="nav-item"
                    exact-active-class="active"
                    tag="li"
                    to="/dashboard/admin/domains"
                    v-if="$store.state.user.permissions && $store.state.user.permissions['AdminDomainController']">
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-web"/>
                        </span>
                        <span class="title">{{ $t('Domains') }}</span>
                    </a>
                </router-link>
                <router-link
                    active-class="active"
                    class="nav-item"
                    exact-active-class="active"
                    tag="li"
                    to="/dashboard/admin/users"
                    v-if="$store.state.user.permissions && $store.state.user.permissions['UserController']">
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-account-supervisor-circle"/>
                        </span>
                        <span class="title">{{ $t('Users') }}</span>
                    </a>
                </router-link>
                <router-link
                    active-class="active"
                    class="nav-item"
                    exact-active-class="active"
                    tag="li"
                    to="/dashboard/admin/roles"
                    v-if="$store.state.user.permissions && $store.state.user.permissions['RoleController']"
                >
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-account-box-multiple-outline"/>
                        </span>
                        <span class="title">{{ $t('Roles') }}</span>
                    </a>
                </router-link>
                <router-link
                    active-class="active"
                    class="nav-item"
                    tag="li"
                    to="/dashboard/admin/settings"
                    v-if="$store.state.user.permissions && $store.state.user.permissions['SettingController']"
                >
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-cog-outline"/>
                        </span>
                        <span class="title">{{ $t('Settings') }}</span>
                    </a>
                </router-link>
                <router-link
                    active-class="active"
                    class="nav-item"
                    exact-active-class="active"
                    tag="li"
                    to="/dashboard/admin/translations"
                    v-if="$store.state.user.permissions && $store.state.user.permissions['LanguageController']"
                >
                    <a>
                        <span class="icon-holder">
                            <i class="mdi mdi-google-translate"/>
                        </span>
                        <span class="title">{{ $t('Translations') }}</span>
                    </a>
                </router-link>
            </ul>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Sidebar"
    }
</script>
