import initialState from '../initialState'
import { ActionTypes } from '../../actions/actions';
import { createReducer } from '../mainReducer';

const actionHandlers = {
  [ActionTypes.ADD_DEVICE_REQUEST]: (state, action) => state,
  [ActionTypes.ADD_DEVICE_SUCCESS]: (state, action) => Object.assign([], state, [action.device]),
  [ActionTypes.ADD_DEVICE_FAILURE]: (state, action) => state,
  [ActionTypes.GET_DEVICE_REQUEST]: (state, action) => state,
  [ActionTypes.GET_DEVICE_SUCCESS]: (state, action) => Object.assign([], state, [action.device]),
  [ActionTypes.GET_DEVICE_FAILURE]: (state, action) => state,
  [ActionTypes.GET_DEVICE_CODE_REQUEST]: (state, action) => state,
  [ActionTypes.GET_DEVICE_CODE_SUCCESS]: (state, action) => Object.assign([], state, [action.code]),
  [ActionTypes.GET_DEVICE_CODE_FAILURE]: (state, action) => state,
  [ActionTypes.GET_DEVICES_REQUEST]: (state, action) => state,
  [ActionTypes.GET_DEVICES_SUCCESS]: (state, action) => Object.assign([], state, action.devices),
  [ActionTypes.GET_DEVICES_FAILURE]: (state, action) => state,
  [ActionTypes.EDIT_DEVICE_REQUEST]: (state, action) => state,
  [ActionTypes.EDIT_DEVICE_SUCCESS]: (state, action) => Object.assign([], state, [action.device]),
  [ActionTypes.EDIT_DEVICE_FAILURE]: (state, action) => state,
};

const devices = createReducer(initialState.devices, actionHandlers);

export default devices;