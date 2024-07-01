import React, { PropTypes } from 'react';

const DeviceCode = ({device}) => (
  <div className="root">
    {device && <section className="">
        <h2 className="title">Device Auth Code</h2>
        <div>{device.imei}</div>
        <div>{device.title}</div>
        <div>{device.code}</div>
    </section>}
  </div>
);

DeviceCode.propTypes = {
    //code: PropTypes.func.isRequired
};

export default DeviceCode;