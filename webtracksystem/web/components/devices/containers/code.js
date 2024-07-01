import { connectWithLifecycle } from 'react-lifecycle-component';
import DeviceCode from '../views/code';
import { fetchDeviceCode } from '../../../actions/devices';

const mapStateToProps = (state, ownProps) => {
    const deviceId = Number(ownProps.params.id);
    const device = state.devices.find(d=>d.id===deviceId);
    console.log(device);
    return {
        device
    };
};

const mapDispatchToProps = (dispatch, ownProps) => { 
    const deviceId = ownProps.params.id;
    return {
        componentWillMount: () => {
            dispatch(fetchDeviceCode(deviceId));           
        }
    }
};

const DeviceCodeContainer = connectWithLifecycle(
        mapStateToProps,
        mapDispatchToProps
    )(DeviceCode)

export default DeviceCodeContainer;