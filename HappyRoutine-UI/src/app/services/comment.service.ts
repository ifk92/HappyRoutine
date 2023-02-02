import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CommentCreate } from '../models/comment/comment-create.model';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(
    private http: HttpClient
  ) { }

  create(model: CommentCreate) : Observable<Comment>  {
    return this.http.post<Comment>(`${environment.webApi}/Comment`, model);
  }

  delete(commentId: number) : Observable<number>  {
    return this.http.delete<number>(`${environment.webApi}/Comment/${commentId}`);
  }

  getAll(postId: number) : Observable<Comment[]> {
    return this.http.get<Comment[]>(`${environment.webApi}/Comment/${postId}`);
  }
}
