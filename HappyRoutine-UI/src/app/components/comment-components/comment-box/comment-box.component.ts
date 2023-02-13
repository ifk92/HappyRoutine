import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CommentCreate } from 'src/app/models/comment/comment-create.model';
import { CommentViewModel } from 'src/app/models/comment/comment-view-model.model';
import { IComment } from 'src/app/models/comment/comment.model';
import { CommentService } from 'src/app/services/comment.service';

@Component({
  selector: 'app-comment-box',
  templateUrl: './comment-box.component.html',
  styleUrls: ['./comment-box.component.css']
})
export class CommentBoxComponent implements OnInit {

  @Input() comment: CommentViewModel;
  @Output() commentSaved = new EventEmitter<IComment>();
  @ViewChild('commentForm') commentForm: NgForm;

  constructor(
    private commentService: CommentService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
  }

  resetComment() {
    this.commentForm.reset();
  }

  onSubmit() {

    let commentCreate: CommentCreate = {
      commentId: this.comment.commentId,
      parentCommentId: this.comment.parentCommentId,
      postId: this.comment.postId,
      content: this.comment.content
    };

    this.commentService.create(commentCreate).subscribe(comment => {
      this.toastr.info("Comment saved.");
      this.resetComment();
      this.commentSaved.emit(comment);
    })
  }

}
