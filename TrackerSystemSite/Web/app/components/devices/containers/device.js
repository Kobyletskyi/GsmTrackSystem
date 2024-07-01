import { connectWithLifecycle } from 'react-lifecycle-component';
import Device from '../views/device';
import { fetchDevice } from '../../../actions/devices';

const mapStateToProps = (state, ownProps) => {
    const deviceId = Number(ownProps.params.id);
    const device = state.devices.find(d=>d.id===deviceId);
    return device ?
    {
        imei : device.imei,
        title : device.title,
        description : device.description,
    }:{}
};

const mapDispatchToProps = (dispatch, ownProps) => { 
    const deviceId = ownProps.params.id;
    return {
        componentWillMount: () => {
            dispatch(fetchDevice(deviceId));           
        }
    }
};

const DeviceContainer = connectWithLifecycle(
        mapStateToProps,
        mapDispatchToProps
    )(Device)

export default DeviceContainer;