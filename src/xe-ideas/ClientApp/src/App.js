import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { FetchData } from './components/FetchData';
import { Counter } from './components/Counter';
import { IdeaList } from './components/idea/IdeaList';
import { IdeaDetails } from './components/idea/IdeaDetails';
import { IdeaDetailsEdit } from './components/idea/IdeaDetailsEdit';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <AuthorizeRoute path='/fetch-data' component={FetchData} />
        <AuthorizeRoute path='/ideas' exact component={IdeaList}  />
        <AuthorizeRoute path='/u/:userName/ideas' exact component={IdeaList}  />
        <AuthorizeRoute path='/i/:ideaId' exact component={IdeaDetails}  />
        <AuthorizeRoute path='/i/:ideaId/edit' exact component={IdeaDetailsEdit}  />
        <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
      </Layout>
    );
  }
}
