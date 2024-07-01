import { ActionTypes } from './actions';
import { SubmissionError } from 'redux-form';
import { get, post } from '../lib/request.js';
import urls from '../lib/urls';
import { DEFAULT_PAGESIZE } from '../lib/settings';
import format from 'string-format';

const trackRequest = () =>({type: ActionTypes.FETCH_TRACK_REQUEST});
const trackSuccess = (result) => ({type: ActionTypes.FETCH_TRACK_SUCCESS, result});
const trackPartSuccess = (result) =>({type: ActionTypes.FETCH_TRACKPART_SUCCESS, result});
const trackFailure = (error) =>({type: ActionTypes.FETCH_TRACK_FAILURE,error});

const devicesRequest = () =>({type: ActionTypes.FETCH_DEVICES_REQUEST});
const devicesSuccess = (devices) =>({type: ActionTypes.FETCH_DEVICES_SUCCESS, devices});
const devicesFailure = (error) =>({type: ActionTypes.FETCH_DEVICES_FAILURE,error});


function handleError(response) {
    if (response.status >= 200 || response.status < 300) return;
    var error = new Error(response.statusText);
    error.response = response;
    throw error;
}

export function fetchTrack(trackId, pageSize) {
    return dispatch => {
        dispatch(trackRequest());
        const url = format(urls.TRACK_POINTS, trackId, (pageSize || DEFAULT_PAGESIZE));
        return get(url)
        .then(resp =>{ 
                dispatch(trackSuccess(resp));
            })
            .catch(exception => {
            console.log(exception);
            dispatch(trackFailure(exception.message));
            throw new SubmissionError({ general: exception.message });
            });
    }
}
export function fetchTrackPart(url) {
    return dispatch => {
        dispatch(trackRequest());
        return get(url)
        .then(resp =>{
                dispatch(trackPartSuccess(resp));
            })
            .catch(exception => {
            console.log(exception);
            dispatch(trackFailure(exception.message));
            throw new SubmissionError({ general: exception.message });
            });
    }
}

export function fetchDevices() {
    return dispatch => {
        dispatch(devicesRequest());
        const url = urls.TRACKS;
        return get(url)
        .then(resp =>{ 
                dispatch(devicesSuccess(resp.data));
            })
            .catch(exception => {
            console.log(exception);
            dispatch(devicesFailure(exception.message));
            throw new SubmissionError({ general: exception.message });
            });
    }
}