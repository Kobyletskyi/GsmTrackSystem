import React, { PropTypes } from 'react';
import { Field } from 'redux-form';
import { Link } from 'react-router';
import { labelInput } from '../../shared/inputs';
import urls from '../../../lib/urls';
import format from 'string-format';

const Devices = ({ devices }) => (
  <div className="root">
    <section className="">
        <h2 className="title">Your Devices</h2>
        {devices.map((device, index) =>
          <div key={index}>
            <Link to={format(urls.DEVICE_CODE_LINK, device.id)}>{device.title} {device.imei}</Link>            
          </div>
        )}
    </section>
  </div>
);

Devices.propTypes = {
};

export default Devices;