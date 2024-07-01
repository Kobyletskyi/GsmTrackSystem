import { connectWithLifecycle } from 'react-lifecycle-component';
import TrackMap from '../views/track';
import { fetchTrack } from '../../../actions/track';

const mapStateToProps = (state, ownProps) => {
    console.log(ownProps);
    const coords = state.track.coordinates ? state.track.coordinates
        .filter(c => c.navigationStatus !== 'NF')
        .map(c => { return { lat: c.latitude, lng: c.longitude }; }) : [];
    return {
        markers: coords,
        data: state.track.coordinates ? state.track.coordinates.filter(c => c.navigationStatus !== 'NF') : [],
    };
};

const mapDispatchToProps = (dispatch, ownProps) => {
    const trackId = ownProps.id;
    return {
        componentWillMount() {
            console.log('componentWillMount');
            dispatch(fetchTrack(trackId));
        },
        componentWillReceiveProps({ id }) {
            console.log('componentWillReceiveProps');
            if (trackId !== id) {
                console.log('fetch');
                dispatch(fetchTrack(id));
            }
        }
    }
};

const TrackContainer = connectWithLifecycle(
    mapStateToProps,
    mapDispatchToProps
)(TrackMap)

export default TrackContainer;