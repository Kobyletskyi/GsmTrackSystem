import initialState from '../initialState'
import { ActionTypes } from '../../actions/actions';
import { createReducer } from '../mainReducer';

const actionHandlers = {
  [ActionTypes.FETCH_TRACK_REQUEST]: (state, action) => state,
  [ActionTypes.FETCH_TRACK_SUCCESS]: (state, action) => {
    action.result.data = action.result.data.sort((a,b)=>a.id-b.id);
    return Object.assign({}, state, {coordinates:action.result})
  },
  [ActionTypes.FETCH_TRACKPART_SUCCESS]: (state, action) =>{
    action.result.data = action.result.data
      .concat(state.coordinates.data)
      .sort((a,b)=>a.id-b.id);
    return  Object.assign({}, state, {coordinates:action.result})
  },
  [ActionTypes.FETCH_TRACK_FAILURE]: (state, action) => state,

  [ActionTypes.FETCH_DEVICES_REQUEST]: (state, action) => state,
  [ActionTypes.FETCH_DEVICES_SUCCESS]: (state, action) => Object.assign({}, state, {devices:action.devices}),
  [ActionTypes.FETCH_DEVICES_FAILURE]: (state, action) => state
};

const track = createReducer(initialState.track, actionHandlers);

export default track;