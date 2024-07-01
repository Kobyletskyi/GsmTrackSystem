import React, { PropTypes } from 'react';
import { Field, reduxForm, formValueSelector } from 'redux-form'
import Button from 'react-bootstrap/lib/Button';
import ButtonToolbar from 'react-bootstrap/lib/ButtonToolbar';
import GoogleMap from '../../../lib/googleMapComponents';
 
const TrackMap = ({ data, dataCount, totalCount, nextPageLink, containerElementProps, onMapClick, onMarkerRightclick,
  loadMorePoints, loadFullTrack}) => {
    const loadMap = (node)=>{
      if(node && data.length){
        var map = new google.maps.Map(node, 
          { 
            zoom: 14,
            mapTypeControlOptions: {
              style: google.maps.MapTypeControlStyle.HORIZONTAL_BAR,
              position: google.maps.ControlPosition.BOTTOM_CENTER
            },
            fullscreenControlOptions: {
              position: google.maps.ControlPosition.LEFT_BOTTOM
          },
        });
        GoogleMap.fitBounds(data, map);
        GoogleMap.setPolyline(data, map);
        GoogleMap.setCircles(data, map);
        GoogleMap.setDirectionMarkers(data, map);
      }
    }
    return (<div style={{height: "100%", width: "100%"}}>
      <div style={{height: "100%", width: "100%"}}>
        <div id='map' ref={loadMap} style={{height: "100%", width: "100%"}}/>
        
        <ButtonToolbar style={{ position: "absolute", top: "10px", left: "10px", zIndex: "5" }}>
          {nextPageLink && <Button bsStyle="primary" bsSize="xsmall" onClick={()=>loadMorePoints(nextPageLink)}>Load more</Button>}
          {dataCount < totalCount && <Button bsSize="xsmall"onClick={()=>loadFullTrack(totalCount)}>Full track</Button>}
        </ButtonToolbar>
      </div>
    </div>);
  }

TrackMap.propTypes = {};

export default TrackMap;
