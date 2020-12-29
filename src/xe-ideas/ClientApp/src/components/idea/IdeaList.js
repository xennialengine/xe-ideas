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
    this._subscription = authService.subscribe(() => this.populateState());
    this.populateState()
    .then(() => {
      this.fetchData();
    });
  }

  componentWillUnmount() {
      authService.unsubscribe(this._subscription);
  }

  componentDidUpdate = (prevProps) => {
    if (this.props.match.params.userName !== prevProps.match.params.userName) {
      this.fetchData();
    }
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

    let title = this.state.userName
      ? `${this.state.userName}'s Ideas`
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
    authService.getAccessToken()
      .then((token) => {
        const userName = this.props.match.params.userName ?? '';

        return fetch(`api/idea?userName=${userName}`, {
          headers: token ? { 'Authorization': `Bearer ${token}` } : {}
        });
      })
      .then((response) => {
        if (response.status >= 400) {
          return {};
        }

        return response.json();
      })
      .then((data) => {
        this.setState({ ideas: data, loading: false, userName: this.props.match.params.userName });
      });
  }
}
