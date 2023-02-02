import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PagedResult } from '../models/post/paged-result.model';
import { PostCreate } from '../models/post/post-create.model';
import { PostPaging } from '../models/post/post-paging.model';
import { Post } from '../models/post/post.model';

@Injectable({
  providedIn: 'root'
})
export class PostService {

  constructor(
    private http: HttpClient
  ) { }

  create(model: PostCreate) : Observable<Post> {
    return this.http.post<Post>(`${environment.webApi}/Post`, model);
  }

  getAll(postPaging: PostPaging) : Observable<PagedResult<Post>> {
    return this.http.get<PagedResult<Post>>(
      `${environment.webApi}/Post?Page=${postPaging.page}&PageSize=${postPaging.pageSize}`);
  }

  get(postId: number) : Observable<Post> {
    return this.http.get<Post>(`${environment.webApi}/Post/${postId}`);
  }

  getByApplicationUserId(applicationUserId: number) : Observable<Post[]> {
    return this.http.get<Post[]>(`${environment.webApi}/Post/user/${applicationUserId}`);
  }

  getMostFamous() : Observable<Post[]> {
    return this.http.get<Post[]>(`${environment.webApi}/Post/famous`);
  }

  delete(postId: number) : Observable<number> {
    return this.http.delete<number>(`${environment.webApi}/Post/${postId}`);
  }
}
