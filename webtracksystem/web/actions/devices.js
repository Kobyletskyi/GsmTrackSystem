import { ActionTypes } from './actions';
import { SubmissionError } from 'redux-form';
import { get, post, put } from '../lib/request.js';
import urls from '../lib/urls';
import format from 'string-format';

const deviceRequest = () => ({type: ActionTypes.GET_DEVICE_REQUEST});
const deviceSuccess = (device) => ({type: ActionTypes.GET_DEVICE_SUCCESS, device});
const deviceFailure = (error) => ({type: ActionTypes.GET_DEVICE_FAILURE,error});

const deviceCodeRequest = () => ({type: ActionTypes.GET_DEVICE_CODE_REQUEST});
const deviceCodeSuccess = (code) => ({type: ActionTypes.GET_DEVICE_CODE_SUCCESS, code});
const deviceCodeFailure = (error) => ({type: ActionTypes.GET_DEVICE_CODE_FAILURE,error});

const devicesRequest = () => ({type: ActionTypes.GET_DEVICES_REQUEST});
const devicesSuccess = (devices) => ({type: ActionTypes.GET_DEVICES_SUCCESS, devices});
const devicesFailure = (error) => ({type: ActionTypes.GET_DEVICES_FAILURE,error});

const addRequest = () => ({type: ActionTypes.ADD_DEVICE_REQUEST});
const addSuccess = (device) => ({type: ActionTypes.ADD_DEVICE_SUCCESS, device});
const addFailure = (error) => ({type: ActionTypes.ADD_DEVICE_FAILURE,error});

const editRequest = () => ({type: ActionTypes.EDIT_DEVICE_REQUEST});
const editSuccess = (device) => ({type: ActionTypes.EDIT_DEVICE_SUCCESS, device});
const editFailure = (error) => ({type: ActionTypes.EDIT_DEVICE_FAILURE,error});

export const fetchDevice = (deviceId) => {
    return (dispatch) => {
        dispatch(deviceRequest(deviceId));
        const url = format(urls.DEVICE, deviceId);
        return get(url)
            .then(resp => { 
                dispatch(deviceSuccess(resp.data));
            })
            .catch(exception => {
                dispatch(deviceFailure(exception.message));
            });
    }
}
export const fetchDeviceCode = (deviceId) => {
    return (dispatch) => {
        dispatch(deviceCodeRequest(deviceId));
        const url = format(urls.DEVICE_CODE, deviceId);
        return get(url)
            .then(resp => { 
                dispatch(deviceCodeSuccess(resp.data));
            })
            .catch(exception => {
                dispatch(deviceCodeFailure(exception.message));
            });
    }
}
export const fetchDevices = () => {
    return (dispatch, getState) => {
        const { devices } = getState();
        //if(!devices || !devices.length){
            dispatch(devicesRequest());
            const url = urls.DEVICES;
            return get(url)
                .then(resp => { 
                    dispatch(devicesSuccess(resp.data));
                })
                .catch(exception => {
                    dispatch(devicesFailure(exception.message));
                });
        //}
    }
}
export const addDevice = ({imei, title, description}, dispatch) => {
    dispatch(addRequest());
    const url = urls.NEW_DEVICE;
    const options = {
        body: JSON.stringify({
            IMEI: imei,
            Title: title,
            SoftwareVersion: "v0",
            Description: description })
    };
    return post(url, options)
        .then(resp => { 
            console.log(resp)
            dispatch(addSuccess({imei, title, description}));
        })
        .catch(exception => {
            console.log(exception)
            dispatch(addFailure(exception.message));
        });
}
export const updateDevice = ({title, description}, dispatch, {deviceId}) => {
    dispatch(editRequest());
    const url = format(urls.DEVICE, deviceId);
    const options = {
        body: JSON.stringify({
            Title: title,
            Description: description })
    };
    return put(url, options)
        .then(resp => { 
            dispatch(editSuccess({deviceId, imei, title, description}));
        })
        .catch(exception => {
            dispatch(editFailure(exception.message));
        });
}