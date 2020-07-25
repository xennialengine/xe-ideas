import React, { Component } from 'react';
import authService from '../api-authorization/AuthorizeService';

export class CommentForm extends Component {
  static displayName = CommentForm.name;

  constructor(props) {
    super(props);
    this.state = { comment: "" };
 
    this.addComment = this.addComment.bind(this);
  }

  render() {
    return (
      <form>
        <table className='table table-striped' aria-labelledby="tabelLabel">
          <tbody>
            <tr>
                <th>Comment</th>
                <td><textarea name="comment" value={this.state.comment} onChange={this.handleChange} /></td>
            </tr>
            <tr>
                <th></th>
                <td><input type="button" value="Save" onClick={this.addComment} /></td>
            </tr>
          </tbody>
        </table>
      </form>
    );
  }

  handleChange = (event) => {
    this.setState({comment: event.target.value, loading: false})
  };

  addComment = async (event) => {
    authService.getAccessToken()
      .then((token) => {
        return fetch(`api/idea/${this.props.ideaId}/comment`, {
          method: 'POST',
          headers: token 
            ? { 
                'Authorization': `Bearer ${token}`,
                'Accept': 'application/json',
                'Content-Type': 'application/json'
              }
            : {},
          body: JSON.stringify({
            "ideaId": this.props.ideaId, 
            "creatorId": this.props.creatorId, 
            "content": this.state.comment
          })
        });
      })
      .then((response) => response.json())
      .then((response) => {
        if (response.status >= 400) {
          alert("There was an error saving the data.");
          return;
        }

        this.props.onCommentAdded(response);
      });
  };
}
