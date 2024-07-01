import React, { PropTypes } from 'react';

const Device = ({imei, title, description}) => (
  <div className="root">
    <section className="">
        <h2 className="title">Device </h2>
        <span>{imei}</span>
        <span>{title}</span>
        <span>{description}</span>
    </section>
  </div>
);

Device.propTypes = {
    //code: PropTypes.func.isRequired
};

export default Device;