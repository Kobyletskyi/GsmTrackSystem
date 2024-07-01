import React, { PropTypes } from 'react';
import { Field, reduxForm, formValueSelector } from 'redux-form';
import Track from '../containers/track';
import { renderSelect } from '../../shared/inputs';


const TrackPage = ({ devices, tracks, handleDeviceIdChange, handleTrackIdChange, handleOnSubmit, values, handleSubmit }) => (
  <div style={{ height: "100%", position: "relative" }}>
    <div style={{ position: "absolute", top: "10px", right: "10px", zIndex: "5" }}>

      <Field name="deviceId" bsSize="xsmall" items={devices} component={renderSelect} onSelect={handleDeviceIdChange} />
      <Field name="trackId" bsSize="xsmall" items={tracks} component={renderSelect} onSelect={handleTrackIdChange} />

    </div>
    <Field name="trackId" component={props => (
      <div style={{ height: "100%" }}>
        {props.input.value && <Track id={props.input.value} />}
      </div>
    )} />
  </div>
);

TrackPage.propTypes = {
  //handleDeviceIdChange:PropTypes.func.isRequired
  //handleTrackIdChange:
};

export default TrackPage;
