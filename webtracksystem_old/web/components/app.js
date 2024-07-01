import React from 'react';
import './shared/bootstrap.css';
import Grid  from 'react-bootstrap/lib/Grid';
import Nav from 'react-bootstrap/lib/Nav';
import Navbar from 'react-bootstrap/lib/Navbar';
import NavItem  from 'react-bootstrap/lib/NavItem';
import { Link } from 'react-router';
import LinkContainer from 'react-router-bootstrap/lib/LinkContainer';

const App = ({ children }) => (
    <div className="root" style={{height: "100%"}}>
      <Navbar>
        <Navbar.Header>
          <Navbar.Brand>
            <Link to='/'>Hello World</Link>
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav navbar>
            <LinkContainer to='/trackers'>
              <NavItem>trackers</NavItem>
            </LinkContainer>
            <LinkContainer to='/track'>
              <NavItem>track</NavItem>
            </LinkContainer>              
          </Nav>
          <Nav navbar className={"navbar-right"}>
            <LinkContainer to='/logout'>
              <NavItem>signout</NavItem>
            </LinkContainer>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
      <Grid style={{height: 'calc(100% - 72px)'}}>
        {children}
      </Grid>
    </div>
  );

export default App;