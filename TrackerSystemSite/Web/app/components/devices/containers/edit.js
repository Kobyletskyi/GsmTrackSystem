import { connect } from 'react-redux';
import { reduxForm } from 'redux-form';
import { updateDevice } from '../../../actions/devices';
import EditDevice from '../views/edit';

const mapStateToProps = (state, ownProps) => {
    console.log('sfhg')
    return { };
};

const mapDispatchToProps = (dispatch) => {
    return { updateDevice };
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

const EditForm = reduxForm({
    form: 'editDevice',
    validate
})(EditDevice);

const EditContainer = connect(
        mapStateToProps,
        mapDispatchToProps
    )(EditForm);

export default EditContainer;