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
      <div>
        <p>By {idea.creatorId}</p>
        <p>{idea.description}</p>
        <div>
          Comments:
          <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th>User</th>
                <th>Date</th>
                <th>Comment</th>
              </tr>
            </thead>
            <tbody>
              {idea.comments.map(comment =>
                <tr key={comment.id}>
                  <td>{comment.creatorId}</td>
                  <td>{comment.createdDate}</td>
                  <td>{comment.content}</td>
                </tr>
              )}
            </tbody>
          </table>
          
        </div>
      </div>
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
