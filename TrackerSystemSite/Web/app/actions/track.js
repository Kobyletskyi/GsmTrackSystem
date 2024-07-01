import { ActionTypes } from './actions';
import { SubmissionError } from 'redux-form';
import { get, post } from '../lib/request.js';

const trackRequest = () =>{return {type: ActionTypes.FETCH_TRACK_REQUEST}};
const trackSuccess = (coordinates) =>{return  {type: ActionTypes.FETCH_TRACK_SUCCESS, coordinates}};
const trackFailure = (error) =>{return  {type: ActionTypes.FETCH_TRACK_FAILURE,error}};

const devicesRequest = () =>{return {type: ActionTypes.FETCH_DEVICES_REQUEST}};
const devicesSuccess = (devices) =>{return  {type: ActionTypes.FETCH_DEVICES_SUCCESS, devices}};
const devicesFailure = (error) =>{return  {type: ActionTypes.FETCH_DEVICES_FAILURE,error}};


function handleError(response) {
    if (response.status >= 200 || response.status < 300) return;
    var error = new Error(response.statusText);
    error.response = response;
    throw error;
}

export function fetchTrack(trackId) {
return dispatch => {
    dispatch(trackRequest());
    const url = `/api/track/GetPoints?trackId=${trackId}`;
    return get(url)
       .then(resp =>{ 
            dispatch(trackSuccess(resp.data));
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
    const url = `/api/track/get`;
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