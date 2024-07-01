import { connectWithLifecycle } from 'react-lifecycle-component';
import Devices from '../views/devices';
import { fetchDevices } from '../../../actions/devices';

const mapStateToProps = (state, ownProps) => {
    const devices = state.devices || [];
    return {
        devices
    };
};

const mapDispatchToProps = (dispatch, ownProps) => { 
    return {
        componentWillMount: () => {
            dispatch(fetchDevices());           
        }
    }
};

const DevicesContainer = connectWithLifecycle(
        mapStateToProps,
        mapDispatchToProps
    )(Devices)

export default DevicesContainer;