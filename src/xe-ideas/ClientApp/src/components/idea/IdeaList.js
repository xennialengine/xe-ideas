import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import authService from '../api-authorization/AuthorizeService';

export class IdeaList extends Component {
  static displayName = IdeaList.name;

  constructor(props) {
    super(props);
    this.state = { 
      ideas: [], 
      loading: true,
      userName: null
    };
  }

  componentDidMount() {
    this.fetchData();
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
          isAuthenticated,
          userId: user && user.userId,
          userName: user && user.userName
      });
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : this.renderTable(this.state.ideas);

    let title = this.state.username
      ? `${this.state.username}'s Ideas`
      : "All Public Ideas";

    return (
      <div>
        <h1 id="tabelLabel">{title}</h1>
        {contents}
      </div>
    );
  }

  renderTable(ideas) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Public</th>
            <th>User</th>
            <th>Idea</th>
            <th>Description</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {ideas.map(idea =>
            <tr key={idea.id}>
              <td>{idea.privacyId === 2 ? "\u2713" : ""}</td>
              <td><Link to={`/u/${idea.creator?.userName}/ideas`}>{idea.creator?.userName}</Link></td>
              <td>{idea.name}</td>
              <td>{idea.description}</td>
              <td>
                  <Link to={`/i/${idea.id}/`}>View</Link>
                  |
                  {idea.creatorId === this.state.userId 
                    ? <Link to={`/i/${idea.id}/edit`}>Edit</Link>
                    : "Edit"
                  }
                  
              </td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  async fetchData() {
    let userParam = this.state.username
      ? `username=${this.state.username}`
      : '';

    const token = await authService.getAccessToken();
    const response = await fetch(`api/idea?${userParam}`, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ ideas: data, loading: false, username: this.props.match.params.username });
  }
}
