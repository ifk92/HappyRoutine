import { PostService } from './../../../services/post.service';
import { Post } from './../../../models/post/post.model';
import { Component, OnInit } from '@angular/core';
import { PagedResult } from 'src/app/models/post/paged-result.model';
import { PostPaging } from 'src/app/models/post/post-paging.model';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-posts',
  templateUrl: './posts.component.html',
  styleUrls: ['./posts.component.css']
})
export class PostsComponent implements OnInit {

  pagedPostResult: PagedResult<Post>;
  post: Post;

  constructor(
    private postService: PostService

  ) { }

  ngOnInit(): void {
    this.loadPagedPostResult(1, 10);
  }

  pageChanged(event: PageChangedEvent) : void {
    this.loadPagedPostResult(event.page, event.itemsPerPage);
  }

  loadPagedPostResult(page, itemsPerPage) {
    let blogPaging = new PostPaging(page, itemsPerPage);
    this.postService.getAll(blogPaging).subscribe(pagedPosts => {
      this.pagedPostResult = pagedPosts;
      console.log('POSTS', this.pagedPostResult);

    });
  }

}
