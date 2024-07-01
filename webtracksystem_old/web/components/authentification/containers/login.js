import { connect } from 'react-redux';
import { withRouter } from 'react-router';
import { reduxForm } from 'redux-form';
import { loginUser } from '../../../actions/user';
import Login from '../views/login';
import { DEFAULT_PATH } from '../../../lib/settings';

const mapStateToProps = (state, ownProps) => {
    let dest = DEFAULT_PATH;
    if (ownProps.location && ownProps.location.state && ownProps.location.state.nextPathname) {
        dest = ownProps.location.state.nextPathname;
    }
    return {
        initialValues: { successRedirect: dest }
    };
};

const mapDispatchToProps = (dispatch) => {
    return { loginUser };
};

const validate = ({userName, password}) => {
    const errors = {}

    if (!userName) {
        errors.userName = 'Required';
    } //else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(userName)) {
    //    errors.userName = 'Invalid email address';
    //}
    if (!password) {
        errors.password = 'Required';
    }

    return errors
}

const LoginForm = reduxForm({
    form: 'login',
    validate
})(Login);

const LoginContainer = withRouter(
    connect(
        mapStateToProps,
        mapDispatchToProps
    )(LoginForm)
);

export default LoginContainer;