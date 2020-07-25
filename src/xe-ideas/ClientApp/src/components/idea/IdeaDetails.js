import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService';
import { CommentForm } from '../comment/CommentForm';

export class IdeaDetails extends Component {
  static displayName = IdeaDetails.name;

  constructor(props) {
    super(props);
    this.state = { idea: {}, loading: true };
    
    this.handleCommentAdded = this.handleCommentAdded.bind(this);
  }

  componentDidMount() {
    this.fetchData();
  }

  renderTable(idea) {
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
                  <td>{comment.creator.userName}</td>
                  <td>{comment.createdDate}</td>
                  <td>{comment.content}</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
                
        <CommentForm 
          ideaId={idea.id} 
          creatorId={"d9ce1329-f6f9-4c4c-a566-a9b51e24d3f1"} // TODO replace this
          onCommentAdded={this.handleCommentAdded}></CommentForm>
      </div>
    );
  }

  render() {
    let contents = "";
    let title = "";

    if (this.state.loading) {
      contents = <p><em>Loading...</em></p>;
    } else if (Object.keys(this.state.idea).length === 0) {
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

  handleCommentAdded = async (comment) => {
    var updated = this.state.idea;
    updated.comments.push(comment);

    this.setState({ idea: updated, loading: false });
  };
}
