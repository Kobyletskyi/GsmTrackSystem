import { connectWithLifecycle } from 'react-lifecycle-component';
import TrackMap from '../views/track';
import { fetchTrack, fetchTrackPart } from '../../../actions/track';

const mapStateToProps = (state, ownProps) => {
    const data = state.track.coordinates.data || [] ;
    const fixed = data.filter(c => c.navigationStatus !== 'NF')
    return {
        dataCount: data.length,
        totalCount: state.track.coordinates.totalCount,
        nextPageLink: state.track.coordinates.nextPageLink,
        data: fixed
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
        },
        loadMorePoints: (nextPageLink)=>{
            dispatch(fetchTrackPart(nextPageLink));
        },
        loadFullTrack: (totalCount)=>{
            dispatch(fetchTrack(trackId, totalCount));            
        }
    }
};

const TrackContainer = connectWithLifecycle(
    mapStateToProps,
    mapDispatchToProps
)(TrackMap)

export default TrackContainer;