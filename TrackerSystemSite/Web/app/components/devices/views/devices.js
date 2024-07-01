import React, { PropTypes } from 'react';
import { Field } from 'redux-form';
import { Link } from 'react-router';
import { labelInput } from '../../shared/inputs';

const Devices = ({ devices }) => (
  <div className="root">
    <section className="">
        <h2 className="title">Your Devices</h2>
        {devices.map((device, index) =>
          <div key={index}>
            <Link to={`/trackers/${device.id}/code`}>{device.imei}</Link>            
          </div>
        )}
    </section>
  </div>
);

Devices.propTypes = {
};

export default Devices;