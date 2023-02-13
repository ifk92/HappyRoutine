import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CommentCreate } from '../models/comment/comment-create.model';
import { IComment } from '../models/comment/comment.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(
    private http: HttpClient
  ) { }

  create(model: CommentCreate) : Observable<IComment>  {
    return this.http.post<IComment>(`${environment.webApi}/Comment`, model);
  }

  delete(commentId: number) : Observable<number>  {
    return this.http.delete<number>(`${environment.webApi}/Comment/${commentId}`);
  }

  getAll(postId: number) : Observable<IComment[]> {
    return this.http.get<IComment[]>(`${environment.webApi}/Comment/${postId}`);
  }
}
