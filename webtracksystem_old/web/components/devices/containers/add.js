import { connect } from 'react-redux';
import { reduxForm } from 'redux-form';
import { addDevice } from '../../../actions/devices';
import NewDevice from '../views/add';
import messages from '../../../lib/messages';

const mapStateToProps = (state, ownProps) => ({ });

const mapDispatchToProps = (dispatch) => ({ addDevice });

const validate = ({imei, title, description}) => {
    const errors = {}

    if (!imei) {
        errors.imei = messages.REQUIRED;
    } 
    // else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(userName)) {
    //     errors.imei = messages.INVAL_IMEI;
    // }
    if (!title) {
        errors.title = messages.REQUIRED;
    }
    if (!description) {
        errors.description = messages.REQUIRED;
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