import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import authService from '../api-authorization/AuthorizeService';

export class IdeaList extends Component {
  static displayName = IdeaList.name;

  constructor(props) {
    super(props);
    this.state = { ideas: [], loading: true, username: this.props.match.params.username };
  }

  componentDidMount() {
    this.fetchData();
  }

  static renderTable(ideas) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Public</th>
            <th>User</th>
            <th>Idea</th>
            <th>Description</th>
            <th>LastModified</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {ideas.map(idea =>
            <tr key={idea.id}>
              <td>{idea.privacyId === 2 ? "\u2713" : ""}</td>
              <td>{idea.creatorId}</td>
              <td>{idea.name}</td>
              <td>{idea.description}</td>
              <td>{idea.lastModifiedDate}</td>
              <td>
                  <Link to={`/i/${idea.id}/`}>View</Link>
                  |
                  <Link to={`/i/${idea.id}/edit`}>Edit</Link>
              </td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : IdeaList.renderTable(this.state.ideas);

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
