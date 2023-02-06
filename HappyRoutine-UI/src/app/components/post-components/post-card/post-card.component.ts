import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Post } from 'src/app/models/post/post.model';
import { PostService } from 'src/app/services/post.service';

@Component({
  selector: 'app-post-card',
  templateUrl: './post-card.component.html',
  styleUrls: ['./post-card.component.css']
})
export class PostCardComponent implements OnInit {

  @Input() post: Post;
  @Input() index: number;

  constructor(
    private router: Router,
    private postService: PostService
  ) { }

  ngOnInit(): void {
  }

  readMore(postId: number) {
    this.router.navigate([`/post/${postId}`])
  }

}
