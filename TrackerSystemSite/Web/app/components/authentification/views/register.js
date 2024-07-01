import React, { PropTypes } from 'react';
import { Field } from 'redux-form';
import { Link } from 'react-router';
import { labelInput, renderCheckbox } from '../../shared/inputs';
import Button from 'react-bootstrap/lib/Button';

const Register = ({handleSubmit, registerUser}) => (
  <div className="root">
    <section className="popup-form register">
      <form onSubmit={handleSubmit(registerUser)}>
        <h2 className="title">Sign Up</h2>
        <Field name="userName" type="text" component={labelInput} label="Enter Your Email" />
        <Field name="password" type="password" component={labelInput} label="Set Password" />
        <Field name="confirm" type="password" component={labelInput} label="Confirm Password" />
        <div>
          <Field name="agreed" id="agreed" component={renderCheckbox}>
            I have read and agree to the <a>Terms & Conditions</a> for website use
          </Field>
        </div>
        <Field name="general" type="hidden" component={labelInput} />
        <div className="controls">
          <Button type="submit" >Sign Up</Button>
        </div>
      </form>
    </section>
  </div>
);

Register.propTypes = {
    registerUser: PropTypes.func.isRequired,
    handleSubmit: PropTypes.func.isRequired,
};

export default Register;