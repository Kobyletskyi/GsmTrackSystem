import React, { PropTypes } from 'react';
import { Field } from 'redux-form';
import { FormControl } from 'react-bootstrap';
import DropdownButton from 'react-bootstrap/lib/DropdownButton';
import MenuItem from 'react-bootstrap/lib/MenuItem';

module.exports = {
    labelInput({ input, label, type, meta: { touched, error, warning } }) {
        return (
            <div className={"form-group " + type + " " + (touched && error ? "has-error" : "") + " " + (touched && !error ? "has-success" : "")}>
                <FormControl {...input} placeholder={label} type={type} className="form-control" />
                {touched && ((error && <span>{error}</span>) || (warning && <span>{warning}</span>))}
                {/*<div class="form-group has-error">
                  <label class="control-label" for="inputError">Input error</label>
                  <input type="text" class="form-control" id="inputError">
                </div>*/}
            </div>
        )
    },
    labelText({ input, label, type, meta: { touched, error, warning } }) {
        return (
            <div className={"input-container " + type + " " + (touched && error ? "error" : "")}>
                <FormControl {...input} placeholder={label} type={type} componentClass="textarea" />
                {touched && ((error && <span>{error}</span>) || (warning && <span>{warning}</span>))}
            </div>
        )
    },
    renderCheckbox(props) {
        return (
            <span>
                <input {...props.input} id={props.id} type="checkbox" />
                <label htmlFor={props.name}>{props.children}</label>
            </span>
        )
    },
    renderSelect(props) {
        const items = props.items || [];
        const selectedItem = items.find(i => i.id === props.input.value);
        const selectedTitle = selectedItem ? selectedItem.title : null;
        return (
            <DropdownButton title={selectedTitle || 'select'} id={`dropdown-basic-${props.name}`} onSelect={props.onSelect}>
                {items.map((item, index) =>
                    (<MenuItem key={item.id} eventKey={item} active={props.input.value === item.id}>{item.title}</MenuItem>)
                )}
            </DropdownButton>
        )
    }
};