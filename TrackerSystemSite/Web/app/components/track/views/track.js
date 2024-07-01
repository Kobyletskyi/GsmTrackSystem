import React, { PropTypes } from 'react';
import { Field, reduxForm, formValueSelector } from 'redux-form'
import {GoogleMapLoader, GoogleMap, Marker, Circle, Polyline} from "react-google-maps";
 
const TrackMap = ({markers, data, containerElementProps, onMapClick, onMarkerRightclick}) => (
  <div style={{height: "100%", width: "100%"}}>
    <div style={{height: "100%", width: "100%"}}>  
      {markers.length && <GoogleMapLoader
        containerElement={
          <div
            {...containerElementProps}
            style={{height: "100%", width: "100%"}}
          />
        }
        googleMapElement={
          <GoogleMap style={{height: "100%", width: "100%"}}
            /*ref={(map) => console.log(map)}*/
            defaultZoom={16}
            center={markers[markers.length-1]}
            onClick={onMapClick}>
              <Polyline path = { markers }/>
              {data.map((marker, index) => (
                <Marker key={index} icon ={{path: 1, scale: 3, rotation: marker.CourseOverGround}}
                 position= { { lat: marker.latitude, lng: marker.longitude } } onRightclick={() => onMarkerRightclick(index)} />
              ))}
              {data.map((marker, index) => (
                <Circle key={index} strokeColor= '#FF0000'
                    strokeOpacity={0.8}
                    strokeWeight={2}
                    fillColor='#FF0000'
                    fillOpacity={0.1}
                    center={{ lat: marker.latitude, lng: marker.longitude }}
                    radius={marker.horizontalAccuracy / 2} />
              ))}
          </GoogleMap>
        }
      />}
    </div>
  </div>
);

TrackMap.propTypes = {};

export default TrackMap;
