'use strict';

import fetch from 'isomorphic-fetch';
import { browserHistory } from 'react-router';
import { LOGIN_PATH, PAGINATION_HEADER } from './settings';

const defaultOpt = {
    headers: { 'Content-Type': 'application/json' },
    mode: 'cors',
    credentials: 'include'
}

const request = (url, method, options) => {
        let opt = Object.assign({}, defaultOpt, options, { method: method });
        return fetch(url, opt)
        .then(response => {
                if (response.status === 401) {
                    browserHistory.push(LOGIN_PATH);
                    return ({ status: response.status});
                }
                return response.json()
                    .then(data => {
                        const pagingInfo = JSON.parse(response.headers.get(PAGINATION_HEADER));
                        return {...pagingInfo, data, status: response.status};
                    })
                    .catch(err=>({status: response.status}));
            });
}

module.exports = {
    get: (url, options) => {
        return request(url, 'GET', options);
    },

    post: (url, options) => {
        return request(url, 'POST', options);
    },

    put: (url, options) => {
        return request(url, 'PUT', options);
    },

    delete: (url, options) => {
        return request(url, 'DELETE', options);
    }
}