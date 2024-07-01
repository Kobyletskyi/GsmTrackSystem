import initialState from '../initialState'
import { ActionTypes } from '../../actions/actions';
import { createReducer } from '../mainReducer';

const actionHandlers = {
  [ActionTypes.LOGIN_REQUEST]: (state, action) => state,
  [ActionTypes.LOGIN_SUCCESS]: (state, action) => Object.assign({}, state, action.user),
  [ActionTypes.LOGIN_FAILURE]: (state, action) => state,
  [ActionTypes.LOGOUT]: (state, action) => state,
  [ActionTypes.REGISTER_REQUEST]: (state, action) => state,
  [ActionTypes.REGISTER_SUCCESS]: (state, action) => Object.assign({}, state, action.user),
  [ActionTypes.REGISTER_FAILURE]: (state, action) => state
};

const user = createReducer(initialState.user, actionHandlers);

export default user;