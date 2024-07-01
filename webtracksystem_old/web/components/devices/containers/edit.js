import { connectWithLifecycle } from 'react-lifecycle-component';
import { reduxForm } from 'redux-form';
import { fetchDevice, updateDevice } from '../../../actions/devices';
import EditDevice from '../views/edit';
import messages from '../../../lib/messages';

const mapStateToProps = (state, ownProps) => {
    const deviceId = Number(ownProps.params.id);
    const device = state.devices.find(d=>d.id===deviceId);
    if(device){
        const initialValues = {
            imei : device.imei,
            title : device.title,
            description : device.description,
            softwareVersion: device.softwareVersion
        };
        return { initialValues };
    }
    return {};
};

const mapDispatchToProps = (dispatch, ownProps) => {
    const deviceId = ownProps.params.id;
    return {
        componentWillMount: () => { dispatch(fetchDevice(deviceId));}, 
        updateDevice 
    };
};

const validate = ({title, description}) => {
    const errors = {}

    if (!title) {
        errors.title = messages.REQUIRED;
    }
    if (!description) {
        errors.description = messages.REQUIRED;
    }
    return errors
}

const EditForm = reduxForm({
    form: 'editDevice',
    validate
})(EditDevice);

const EditContainer = connectWithLifecycle(
        mapStateToProps,
        mapDispatchToProps
    )(EditForm);

export default EditContainer;