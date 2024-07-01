import React, { PropTypes } from 'react';
import { Field } from 'redux-form';
import { labelInput, labelText } from '../../shared/inputs';

const EditDevice = ({handleSubmit, updateDevice}) => (
  <div className="root">
    <section className="">
      <form onSubmit={handleSubmit(updateDevice)}>
        <h2 className="title">Edit Device</h2>
        <Field name="imei" type="text" component={labelInput} label="Enter Tracker IMEI" />
        <Field name="title" type="text" component={labelInput} label="Enter Tracker Name" />
        <Field name="description" component={labelText} label="Enter Tracker Description" />
        <Field name="softwareVersion" component={labelText} label="Enter Tracker Software" />
        <Field name="general" type="hidden" component={labelInput} />
        <div className="controls">
          <button type="submit" >Update</button>
        </div>
      </form>
    </section>
  </div>
);

EditDevice.propTypes = {
    editDevice: PropTypes.func.isRequired,
    handleSubmit: PropTypes.func.isRequired,
};

export default EditDevice;