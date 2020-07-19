import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import authService from '../api-authorization/AuthorizeService';

export class IdeaDetails extends Component {
  static displayName = IdeaDetails.name;

  constructor(props) {
    super(props);
    this.state = { idea: [], loading: true };
  }

  componentDidMount() {
    this.fetchData();
  }

  static renderTable(idea) {
    return (
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <tbody>
            <tr>
                <th>ID</th>
                <td>{idea.id}</td>
            </tr>
            <tr>
                <th>owner</th>
                <td>{idea.creatorId}</td>
            </tr>
            <tr>
                <th>Privacy</th>
                <td>{idea.privacyId === 1 ? "Private" : "Public"}</td>
            </tr>
            <tr>
                <th>Name</th>
                <td>{idea.name}</td>
            </tr>
            <tr>
                <th>Description</th>
                <td>{idea.description}</td>
            </tr>
            <tr>
                <th>Created Date</th>
                <td>{idea.createdDate}</td>
            </tr>
            <tr>
                <th>Last Modified Date</th>
                <td>{idea.lastModifiedDate}</td>
            </tr>
          </tbody>
        </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : IdeaDetails.renderTable(this.state.idea);

    let title = this.state.idea
      ? `${this.state.idea.name}`
      : "Content not found.";

    return (
      <div>
        <h1 id="tabelLabel">{title}</h1>
        {contents}
      </div>
    );
  }

  async fetchData() {
    const token = await authService.getAccessToken();
    const response = await fetch(`api/idea/${this.props.match.params.ideaId}`, {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ idea: data, loading: false });
  }
}
