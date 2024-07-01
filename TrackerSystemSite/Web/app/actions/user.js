import { ActionTypes } from './actions';
import { SubmissionError } from 'redux-form';
import { get, post } from '../lib/request.js';
import { browserHistory } from 'react-router';
import cookie from 'react-cookie';

const loginRequest = (userName) => ({type: ActionTypes.LOGIN_REQUEST,userName});
const loginSuccess = (user) => ({type: ActionTypes.LOGIN_SUCCESS,user});
const loginFailure = (error) => ({type: ActionTypes.LOGIN_FAILURE,error});

const registerRequest = (userName) => ({type: ActionTypes.REGISTER_REQUEST,userName});
const registerSuccess = (user) => ({ type: ActionTypes.REGISTER_SUCCESS, user });
const registerFailure = (error) => ({type: ActionTypes.REGISTER_FAILURE,error});

const logoutRequest = () => ({type: ActionTypes.LOGOUT_REQUEST});
const logoutSuccess = () => ({type: ActionTypes.LOGOUT_SUCCESS});
const logoutFailure = (error) => ({type: ActionTypes.LOGOUT_FAILURE,error});

export function logout() {
    return (dispatch)=>{
        dispatch(logoutRequest());
        cookie.remove('userId');
        const url = '/api/account/signout';
    
        return get(url)
            .then(res => {
                if(res.statusCode === 200){
                    dispatch(logoutSuccess());
                    //browserHistory.push(successRedirect);
                }else{
                    throw new Error();
                }
        }).catch(exception => {
            dispatch(logoutFailure(exception));
        });
    }
}

export function loginUser({userName, password, remember, successRedirect}, dispatch) {
    console.log(remember)
    dispatch(loginRequest(userName));

    const url = '/api/account/signin';
    const options = {
        body: JSON.stringify({ userName, password, remember: true })
    }
    return post(url, options)
        .then(res => {
            if(res.status === 200 || res.status === 304){
                //cookie.save('userId', res.data.id);
                dispatch(loginSuccess(res.data));
                browserHistory.push(successRedirect);
                console.log(successRedirect);
            }else{
                throw new Error();
            }
        })
        .catch(exception => {
            dispatch(loginFailure());
            throw new SubmissionError({ general: exception.message });
        });
}

export function registerUser({userName, password, successRedirect}, dispatch) {
    dispatch(registerRequest(userName));
    const options = {
        body: JSON.stringify({ userName, password })
    }
    const url = '/api/account/signup';

    return post(url, options)
        .then(res => {
            if(res.status === 201){
                //cookie.save('userId', res.data.id);
                dispatch(registerSuccess(res.data));
                browserHistory.push(successRedirect);
                console.log(successRedirect);
            }else{
                throw new Error();
            }
        })
        .catch(exception => {
            dispatch(registerFailure());
            throw new SubmissionError({ general: exception.message });
        });
}
    