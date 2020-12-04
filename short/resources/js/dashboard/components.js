// Vue
import Vue from 'vue';
// Imports
import BaseApp from './components/App';
import Navbar from './components/modules/Navbar';
import Sidebar from './components/modules/Sidebar';
import Footer from './components/modules/Footer';
import NewLink from './components/modules/NewLink';
import BrowsersGraph from './components/widgets/BrowsersGraph';
import ClicksGraph from './components/widgets/ClicksGraph';
import DevicesGraph from './components/widgets/DevicesGraph';
import LocationsGraph from './components/widgets/LocationsGraph';
import PlatformsGraph from './components/widgets/PlatformsGraph';
import ReferrersGraph from './components/widgets/ReferrersGraph';

// Components
Vue.component('app', BaseApp);
Vue.component('navbar-module', Navbar);
Vue.component('sidebar-module', Sidebar);
Vue.component('footer-module', Footer);
Vue.component('new-link', NewLink);
// Widgets
Vue.component('browsers-graph', BrowsersGraph);
Vue.component('clicks-graph', ClicksGraph);
Vue.component('devices-graph', DevicesGraph);
Vue.component('locations-graph', LocationsGraph);
Vue.component('platforms-graph', PlatformsGraph);
Vue.component('referrers-graph', ReferrersGraph);
