import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService';

export class IdeaDetailsEdit extends Component {
  static displayName = IdeaDetailsEdit.name;

  constructor(props) {
    super(props);
    this.state = { idea: {}, loading: true };

    this.handleChange = this.handleChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  componentDidMount() {
    this.fetchData();
  }

  renderTable() {
    return (
      <form>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <tbody>
            <tr>
                <th>ID</th>
                <td>{this.state.idea.id}</td>
            </tr>
            <tr>
                <th>owner</th>
                <td>{this.state.idea.creatorId}</td>
            </tr>
            <tr>
                <th>Public</th>
                <td>
                  <select name="privacyId" value={this.state.idea.privacyId} onChange={this.handleChange}>
                    <option value="1">Private</option>
                    <option value="2">Public</option>
                  </select>
                </td>
            </tr>
            <tr>
                <th>Name</th>
                <td><input name="name" type="text" value={this.state.idea.name} onChange={this.handleChange} /></td>
            </tr>
            <tr>
                <th>Description</th>
                <td><textarea name="description" value={this.state.idea.description ?? ""} onChange={this.handleChange} /></td>
            </tr>
            <tr>
                <th>Created Date</th>
                <td>{this.state.idea.createdDate}</td>
            </tr>
            <tr>
                <th>Last Modified Date</th>
                <td>{this.state.idea.lastModifiedDate}</td>
            </tr>
            <tr>
                <th></th>
                <td><input type="button" value="Save" onClick={this.handleSubmit} /></td>
            </tr>
          </tbody>
        </table>
      </form>
    );
  }

  render() {
    let contents = "";
    let title = "";

    if (this.state.loading) {
      contents = <p><em>Loading...</em></p>;
    } else if (Object.keys(this.state.idea).length === 0) {
      contents = "";
      title = "Idea not found";
    } else {
      contents = this.renderTable(this.state.idea);
      title = this.state.idea.name;
    }

    return (
      <div>
        <h1 id="tabelLabel">{title}</h1>
        {contents}
      </div>
    );
  }

  async fetchData() {
    authService.getAccessToken()
      .then((token) => {
        return fetch(`api/idea/${this.props.match.params.ideaId}`, {
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
        this.setState({ idea: data, loading: false });
      });
  }

  handleChange = (event) => {
    let updatedIdea = {...this.state.idea};

    // Convert int properly in json
    if (["privacyId"].includes(event.target.name)) {
      updatedIdea[event.target.name] = parseInt(event.target.value);
    } else {
      updatedIdea[event.target.name] = event.target.value;
    }

    this.setState({idea: updatedIdea, loading: false})
  };

  handleSubmit = async (event) => {
    authService.getAccessToken()
      .then((token) => {
        return fetch(`api/idea/${this.props.match.params.ideaId}`, {
          method: 'PUT',
          headers: token 
            ? { 
                'Authorization': `Bearer ${token}`,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
              }
            : {},
          body: JSON.stringify(this.state.idea)
        });
      })
      .then((response) => {
        if (response.status >= 400) {
          alert("There was an error saving the data.");
        } else {
          alert("The idea has been saved.");
        }

        event.preventDefault();
      });
  };
}
