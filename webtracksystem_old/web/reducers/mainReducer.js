import { combineReducers } from 'redux';
import user from './user/index';
import track from './track/index';
import devices from './devices/index';
import { reducer as formReducer } from 'redux-form';

export function createReducer(initialState, handlers) {
    return function reducer(state = initialState, action) {
        if (handlers.hasOwnProperty(action.type)) {
            return handlers[action.type](state, action);
        } else {
            return state
        }
    }
    };

export const mainReducer = combineReducers({
    form: formReducer,
    user,
    track,
    devices
});