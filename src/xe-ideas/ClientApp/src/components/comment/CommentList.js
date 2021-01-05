import React, { Component } from 'react';
import { Link } from 'react-router-dom';

export class CommentList extends Component {
  static displayName = CommentList.name;

  constructor(props) {
    super(props);
  }

  render() {
    return (
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
            {this.props.idea.comments.map(comment =>
              <tr key={comment.id}>
                <td><Link to={`/u/${comment.creator?.userName}/ideas`}>{comment.creator?.userName}</Link></td>
                <td>{comment.createdDate}</td>
                <td>{comment.content}</td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    );
  }
}
