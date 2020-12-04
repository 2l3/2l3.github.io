import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

const store = new Vuex.Store({
    state: {
        user: false,
        settings: false,
    },
    mutations: {
        login(state, response) {
            localStorage.setItem('token', response.access_token);
            window.axios.defaults.headers.common['Authorization'] = 'Bearer ' + response.access_token;
            axios.post('api/auth/user').then(function (response) {
                state.user = response.data.data;
            });
        },
        logout(state) {
            axios.post('api/auth/logout');
            delete window.axios.defaults.headers.common.Authorization;
            localStorage.removeItem('token');
            state.user = false;
        },
        setSettings(state, data) {
            state.settings = data;
        },
        setUser(state) {
            if (localStorage.getItem('token')) {
                axios.post('api/auth/user').then(function (response) {
                    state.user = response.data.data;
                });
            }
        },
    }
});

export default store;

