import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux'
import { mainReducer, defaultState } from './reducers';
import thunk from 'redux-thunk';
import {createStore, applyMiddleware, compose} from 'redux'

import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';

// @ts-ignore
const composeEnhancers = window['__REDUX_DEVTOOLS_EXTENSION_COMPOSE__'] as typeof compose || compose;

const store = createStore(mainReducer, defaultState(), composeEnhancers( 
  applyMiddleware(thunk)
));

const root = document.getElementById('root');

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();

const load = async () => {
  ReactDOM.render(<Provider store={store}><App /></Provider>, root);
};

load();
