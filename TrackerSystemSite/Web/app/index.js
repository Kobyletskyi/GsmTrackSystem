import React from 'react';
import { render } from 'react-dom';
import { Provider } from 'react-redux';
import { createStore, applyMiddleware, compose } from 'redux';
import thunkMiddleware from 'redux-thunk';
import { mainReducer } from './reducers/mainReducer';
import Root from './components/Root';

const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
//window.__REDUX_DEVTOOLS_EXTENSION__());

let store = createStore(mainReducer,
  composeEnhancers(
    applyMiddleware(thunkMiddleware)
  ));

render(
  <Root store={store} />,
  document.getElementById('root')
);