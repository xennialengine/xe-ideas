import React, { Component } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import { LoginMenu } from './api-authorization/LoginMenu';
import './NavMenu.css';
import authService from './api-authorization/AuthorizeService';

export class NavMenu extends Component {
  static displayName = NavMenu.name;

  constructor (props) {
    super(props);

    this.toggleNavbar = this.toggleNavbar.bind(this);
    this.state = {
      collapsed: true,
      isAuthenticated: false,
    };
  }

  componentDidMount() {
      this._subscription = authService.subscribe(() => this.populateState());
      this.populateState();
  }

  componentWillUnmount() {
      authService.unsubscribe(this._subscription);
  }

  async populateState() {
      const [isAuthenticated, user] = await Promise.all([authService.isAuthenticated(), authService.getUser()])
      this.setState({
          collapsed: true,
          isAuthenticated
      });
  }

  toggleNavbar () {
    this.setState({
      collapsed: !this.state.collapsed,
      isAuthenticated: this.state.isAuthenticated
    });
  }

  render () {
    return (
      <header>
        <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
          <Container>
            <NavbarBrand tag={Link} to="/">xe_ideas</NavbarBrand>
            <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
            <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
              <ul className="navbar-nav flex-grow">
                <NavItem>
                  <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                </NavItem>
                
                {this.state.isAuthenticated ? this.renderMyIdeas() : ""}
                {this.state.isAuthenticated ? this.renderBrowse() : ""}

                <LoginMenu>
                </LoginMenu>
              </ul>
            </Collapse>
          </Container>
        </Navbar>
      </header>
    );
  }

  renderMyIdeas() {
    return (
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/u/superkarn@gmail.com/ideas">My Ideas</NavLink>
      </NavItem>
    );
  }

  renderBrowse() {
    return (
      <NavItem>
        <NavLink tag={Link} className="text-dark" to="/ideas">Browse</NavLink>
      </NavItem>
    );
  }
}
