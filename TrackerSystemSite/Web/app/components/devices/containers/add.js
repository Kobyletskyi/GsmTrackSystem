import { connect } from 'react-redux';
import { reduxForm } from 'redux-form';
import { addDevice } from '../../../actions/devices';
import NewDevice from '../views/add';

const mapStateToProps = (state, ownProps) => {
    return { };
};

const mapDispatchToProps = (dispatch) => {
    return { addDevice };
};

const validate = ({imei, title, description}) => {
    const errors = {}

    if (!imei) {
        errors.imei = 'Required';
    } 
    // else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(userName)) {
    //     errors.userName = 'Invalid email address';
    // }
    if (!title) {
        errors.title = 'Required';
    }
    if (!description) {
        errors.description = 'Required';
    }
    return errors
}

const NewForm = reduxForm({
    form: 'newDevice',
    validate
})(NewDevice);

const NewContainer = connect(
        mapStateToProps,
        mapDispatchToProps
    )(NewForm);

export default NewContainer;