import { connectWithLifecycle } from 'react-lifecycle-component';
import { reduxForm, change } from 'redux-form';
import TrackPage from '../views/selector';
import { fetchDevices, fetchTrack } from '../../../actions/track';

const formName = 'track';

const mapStateToProps = (state, ownProps) => {
    const form = state.form[formName];
    const trackId = form && form.values && form.values.trackId ? form.values.trackId : Number(ownProps.params.id);
    const deviceId = form && form.values && form.values.deviceId ? form.values.deviceId : null;
    const devices = state.track.devices
        ? state.track.devices.map(d => ({ id: d.imei, title: d.title, tracks: d.tracks.map(t => ({ id: t.id, title: t.name })) }))
        : [];
    const selectedDevice = devices.find(d => deviceId
        ? d.id === deviceId
        : d.tracks.some(t => t.id === trackId))
        || devices[0];
    const tracks = selectedDevice ? selectedDevice.tracks : []
    const selectedTrack = tracks.find(t => t.id === trackId) || tracks[0];
    const initialValues = selectedTrack ? { deviceId: selectedDevice.id, trackId: selectedTrack.id } : null;

    return {
        devices,
        tracks,
        initialValues
    };
};

const mapDispatchToProps = (dispatch, ownProps) => {
    return {
        componentWillMount: () => {
            dispatch(fetchDevices());
        },
        handleDeviceIdChange: (eventKey, event) => {
            const trackId = eventKey.tracks && eventKey.tracks[0] ? eventKey.tracks[0].id : null;
            dispatch(change(formName, 'deviceId', eventKey.id));
            dispatch(change(formName, 'trackId', trackId));
        },
        handleTrackIdChange: (eventKey, event) => {
            dispatch(change(formName, 'trackId', eventKey.id));
        }
    }
};

const TrackForm = reduxForm({
    form: formName
})(TrackPage);

const TrackPageContainer = connectWithLifecycle(
    mapStateToProps,
    mapDispatchToProps
)(TrackForm)

export default TrackPageContainer;