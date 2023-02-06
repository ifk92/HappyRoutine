import { PostCreate } from './../../../models/post/post-create.model';
import { Post } from './../../../models/post/post.model';
import { PostService } from './../../../services/post.service';
import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-post-edit',
  templateUrl: './post-edit.component.html',
  styleUrls: ['./post-edit.component.css']
})
export class PostEditComponent implements OnInit {

  postForm: UntypedFormGroup;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: UntypedFormBuilder,
    private postService: PostService,
    private toastr: ToastrService,
  ) { }

  ngOnInit(): void {
    const postId = parseInt(this.route.snapshot.paramMap.get('id'));

    this.postForm = this.formBuilder.group({
      postId: [postId],
      content : ['', [
        Validators.required,
        Validators.minLength(10),
        Validators.maxLength(240),
      ]]
    });

    if (!!postId && postId !== -1) {
      this.postService.get(postId).subscribe(post => {
        this.updateForm(post);
      });
    }
  }

  isTouched(field: string) {
    return this.postForm.get(field).touched;
  }

  hasErrors(field: string) {
    return this.postForm.get(field).errors;
  }

  hasError(field: string, error: string) {
    return !!this.postForm.get(field).hasError(error);
  }

  isNew() {
    return parseInt(this.postForm.get('postId').value) === -1;
  }

  updateForm(post: Post) {

    this.postForm.patchValue({
      postId: post.postId,
      content: post.content,

    });
  }

  onSubmit() {

    let postCreate: PostCreate = new PostCreate(
      !!this.postForm.get("postId").value? this.postForm.get("postId").value : -1,
      this.postForm.get("content").value
    );

    this.postService.create(postCreate).subscribe(createdPost => {
      this.updateForm(createdPost);
      this.toastr.info("Post saved.");
    })
  }

}
