import axios from 'axios';
// Axios base path
axios.defaults.baseURL = window.config.path;
// Axios default headers
axios.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
axios.defaults.headers.common['X-CSRF-TOKEN'] = document.head.querySelector('meta[name="csrf-token"]').content;
// Axios jwt token by default
if (localStorage.getItem('token')) {
    axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('token');
}
// Axios error listener
axios.interceptors.response.use(function (response) {
    return response;
}, function (error) {
    if (error.response.status === 401) {
        // If error 401 redirect to login
        window.location.href = window.config.path + '/auth/login';
        delete window.axios.defaults.headers.common.Authorization;
        localStorage.removeItem('token');
        throw new Error('Unauthorized');
    } else if (error.response.data.message) {
        // If it is a notification error, display message
        Ladda.stopAll();
        let message = document.createElement('div');
        if (error.response.data.errors) {
            let content = '<ul class="text-left">';
            Object.keys(error.response.data.errors).forEach(function (element) {
                error.response.data.errors[element].forEach(function (item) {
                    content += '<li>' + item + '</li>';
                });
            });
            content += '</ul>';
            message.innerHTML = content;
        } else {
            message.innerHTML = error.response.data.message;
        }
        swal({title: 'Error', content: message, icon: 'error'});
        return Promise.reject(error.response.data.message);
    } else {
        // If it is an uncontrolled error, display http status
        Ladda.stopAll();
        swal('Error ' + error.response.status, error.response.statusText, 'error');
        return Promise.reject(error);
    }
});

/**
 * Set global so you don't have to import it
 */
window.axios = axios;
