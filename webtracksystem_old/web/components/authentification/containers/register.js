import { connect } from 'react-redux';
import { withRouter } from 'react-router';
import { reduxForm } from 'redux-form';
import { registerUser } from '../../../actions/user';
import Register from '../views/register';

const mapStateToProps = (state, ownProps) => {
    let dest = '/track';
    if (ownProps.location && ownProps.location.state && ownProps.location.state.nextPathname) {
        dest = ownProps.location.state.nextPathname;
    }
    return {
        initialValues: { successRedirect: dest }
    };
};

const mapDispatchToProps = (dispatch) => {
    return { registerUser };
};


const validate = (values) => {
    const errors = {}

    if (!values.userName) {
        errors.userName = 'Required';
    } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.userName)) {
        errors.userName = 'Invalid email address';
    }
    if (!values.password) {
        errors.password = 'Required';
    }
    if (!values.confirm) {
        errors.confirm = 'Required';
    }
    if (values.password !== values.confirm) {
        errors.confirm = 'Passwords should be equal';
    }
    if (!values.agreed) {
        errors.general = 'You should agree to the Terms & Conditions';
    }

    return errors
}

const RegisterForm = reduxForm({
    form: 'register',
    validate
})(Register);

const RegisterContainer = withRouter(
    connect(
        mapStateToProps,
        mapDispatchToProps
    )(RegisterForm)
);

export default RegisterContainer;