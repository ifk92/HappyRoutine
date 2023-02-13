import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommentViewModel } from 'src/app/models/comment/comment-view-model.model';
import { IComment } from 'src/app/models/comment/comment.model';
import { AccountService } from 'src/app/services/account.service';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  @Input() comments: CommentViewModel[];

  constructor(
    public accountService: AccountService,
    private toastr: ToastrService,
    private commentService: CommentService
  ) { }

  ngOnInit(): void {
  }

  editComment(comment: CommentViewModel) {
    comment.isEditable = true;
  }

  showDeleteConfirm(comment: CommentViewModel) {
    comment.deleteConfirm = true;
  }

  cancelDeleteConfirm(comment: CommentViewModel) {
    comment.deleteConfirm = false;
  }

  deleteConfirm(comment: CommentViewModel, comments: CommentViewModel[]) {
    this.commentService.delete(comment.commentId).subscribe(() => {

      let index = 0;

      for(let i=0; i<comments.length; i++) {
        if (comments[i].commentId === comment.commentId) {
          index = i;
        }
      }

      if (index > -1) {
        comments.splice(index, 1);
      }

      this.toastr.info(" Comment deleted.");
    });
  }

  replyComment(comment: CommentViewModel) {
    let replyComment: CommentViewModel = {
      parentCommentId: comment.commentId,
      content: '',
      postId: comment.postId,
      commentId: -1,
      username: this.accountService.currentUserValue.username,
      publishDate: new Date(),
      updateDate: new Date(),
      isEditable: false,
      deleteConfirm: false,
      isReplying: true,
      comments: []
    };

    comment.comments.push(replyComment);
  }

  onCommentSaved(blogComment: IComment, comment: CommentViewModel) {
    comment.commentId = blogComment.commentId;
    comment.parentCommentId = blogComment.parentCommentId;
    comment.postId = blogComment.postId;
    comment.content = blogComment.content;
    comment.publishDate = blogComment.publishDate;
    comment.updateDate = blogComment.updateDate;
    comment.username = blogComment.username;
    comment.isEditable = false;
    comment.isReplying = false;
  }


}
