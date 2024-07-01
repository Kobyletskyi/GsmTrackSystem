import React, { PropTypes } from 'react';
import { Field } from 'redux-form';
import { Link } from 'react-router';
import { labelInput, renderCheckbox } from '../../shared/inputs';
import Button from 'react-bootstrap/lib/Button';

const Login = ({ handleSubmit, loginUser}) => (
  <div className="root">
    <section className="popup-form login">
      <form onSubmit={handleSubmit(loginUser)}>
        <h2 className="title">Sign In</h2>
        <Field name="userName" type="text" component={labelInput} label="Email"/>
        <Field name="password" type="password" component={labelInput} label="Password"/>
        <div>
          <Field name="remember" id="remember" component={renderCheckbox}>Remember me on this computer</Field>
        </div>
        <Field name="general" type="hidden" component={labelInput} />
        <div className="controls">
          <Button type="submit">Sign In</Button>
        </div>
        </form>
    </section>
  </div>
);

Login.propTypes = {
    loginUser: PropTypes.func.isRequired,
    handleSubmit: PropTypes.func.isRequired
};

export default Login;