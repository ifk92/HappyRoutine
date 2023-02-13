import { CommentService } from './../../../services/comment.service';
import { Component, Input, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';
import { CommentViewModel } from 'src/app/models/comment/comment-view-model.model';
import { IComment } from 'src/app/models/comment/comment.model';
@Component({
  selector: 'app-comment-system',
  templateUrl: './comment-system.component.html',
  styleUrls: ['./comment-system.component.css']
})
export class CommentSystemComponent implements OnInit {

  @Input() postId: number;

  standAloneComment: CommentViewModel;
  comments: IComment[];
  commentViewModels: CommentViewModel[];

  constructor(
    public commentService: CommentService,
    public accountService: AccountService

  ) { }

  ngOnInit(): void {
    this.commentService.getAll(this.postId).subscribe(comments => {

      if (this.accountService.isLoggedIn()) {
        this.initComment(this.accountService.currentUserValue.username);
      }

      this.comments = comments;
      this.commentViewModels = [];

      for (let i=0; i<this.commentViewModels.length; i++) {
        if (!this.commentViewModels[i].parentCommentId) {
          this.findCommentReplies(this.commentViewModels, i);
        }
      }

    });
  }

  findCommentReplies(blogCommentViewModels: CommentViewModel[], index: number) {

    let firstElement = this.comments[index];
    let newComments: CommentViewModel[] = [];

    let commentViewModel: CommentViewModel = {
      parentCommentId: firstElement.parentCommentId || null,
      content: firstElement.content,
      postId: firstElement.postId,
      commentId: firstElement.commentId,
      username: firstElement.username,
      publishDate: firstElement.publishDate,
      updateDate: firstElement.updateDate,
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      comments: newComments
    }

    blogCommentViewModels.push(commentViewModel);

    for (let i=0; i<this.comments.length; i++) {
      if (this.comments[i].parentCommentId === firstElement.commentId) {
        this.findCommentReplies(newComments, i);
      }
    }
  }

// a function with a parameter of type event


  initComment(username: string) {
    this.standAloneComment = {
      parentCommentId: null,
      content: '',
      postId: this.postId,
      commentId: -1,
      username: username,
      publishDate: null,
      updateDate: null,
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      comments: []
    };
  }

  onCommentSaved(comment: IComment) {
    let commentViewModel: CommentViewModel = {
      parentCommentId: comment.parentCommentId ,
      content: comment.content,
      postId: comment.postId,
      commentId: comment.commentId,
      username: comment.username,
      publishDate: comment.publishDate,
      updateDate: comment.updateDate,
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      comments: []
    }

    this.commentViewModels.unshift(commentViewModel);
  }

}
