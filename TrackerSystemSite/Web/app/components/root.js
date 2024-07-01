import React, { PropTypes } from 'react';
import { Provider } from 'react-redux';
import { Router, Route, IndexRedirect, browserHistory } from 'react-router';
import App from './app';
import Logout from './logout';
import LoginContainer from './authentification/containers/login';
import RegisterContainer from './authentification/containers/register';
import TrackPageContainer from './track/containers/selector';
import { logout } from '../actions/user';
import AddDeviceContainer from './devices/containers/add';
import EditDeviceContainer from './devices/containers/edit';
import DeviceContainer from './devices/containers/device';
import DevicesContainer from './devices/containers/devices';
import DeviceCodeContainer from './devices/containers/code';

const redirectUnauthorized = (store) => {
  return (nextState, replace) => {
    console.log('enter');
    const { user: { Id = null } } = store.getState();
    if (!Id) {
      const { location } = nextState;
      replace({
        pathname: '/login',
        state: { nextPathname: location.pathname }
      })
    }
  }
};

const logoutClick = (store) => {
  return (nextState, replace) => {
    store.dispatch(logout());
  }
};

const Root = ({ store }) => (
  <Provider store={store}>
    <Router history={browserHistory}>
      <Route path="/" component={App} onEnter={redirectUnauthorized(store)}>
        <IndexRedirect to="/track" />
        <Route path="/track(/:id)" component={TrackPageContainer} />

        <Route path="/trackers/new" component={AddDeviceContainer} />
        <Route path="/trackers/edit/:id" component={EditDeviceContainer} />
        <Route path="/trackers/:id" component={DeviceContainer} />
        <Route path="/trackers" component={DevicesContainer} />
        <Route path="/trackers/:id/code" component={DeviceCodeContainer} />

      </Route>
      <Route path="/" component={Logout} >
        <Route path="/login" component={LoginContainer} />
        <Route path="/register" component={RegisterContainer} />
        <Route path="/logout" onEnter={logoutClick(store)} />
      </Route>
    </Router>
  </Provider>
);

Root.propTypes = {
  store: PropTypes.object.isRequired,
};

export default Root;