import React, { PropTypes } from 'react';
import { Field } from 'redux-form';
import { labelInput, labelText } from '../../shared/inputs';

const NewDevice = ({handleSubmit, addDevice}) => (
  <div className="root">
    <section className="">
      <form onSubmit={handleSubmit(addDevice)}>
        <h2 className="title">Add Device</h2>
        <Field name="imei" type="text" component={labelInput} label="Enter Tracker IMEI" />
        <Field name="title" type="text" component={labelInput} label="Enter Tracker Name" />
        <Field name="description" component={labelText} label="Enter Tracker Description" />
        <Field name="general" type="hidden" component={labelInput} />
        <div className="controls">
          <button type="submit" >Add</button>
        </div>
      </form>
    </section>
  </div>
);

NewDevice.propTypes = {
    addDevice: PropTypes.func.isRequired,
    handleSubmit: PropTypes.func.isRequired,
};

export default NewDevice;