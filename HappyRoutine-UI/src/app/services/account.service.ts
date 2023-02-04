import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

import { ApplicationUserCreate } from '../models/account/application-user-create.model';
import { ApplicationUserLogin } from '../models/account/application-user-login.model';
import { ApplicationUser } from '../models/account/application-user.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private currentUserSubject$: BehaviorSubject<ApplicationUser|null>

  constructor(
    private http: HttpClient
  ) {
    const value = localStorage.getItem('happyRoutine-currentUser') || '';
    this.currentUserSubject$ =  new BehaviorSubject<ApplicationUser|null>(value !== '' ?JSON.parse(value):null);
  }

  login(model: ApplicationUserLogin) : Observable<ApplicationUser>  {
    return this.http.post<any>(`${environment.webApi}/Account/login`, model).pipe(
      map((user : ApplicationUser) => {

        if (user) {
          localStorage.setItem('happyRoutine-currentUser', JSON.stringify(user));
          this.setCurrentUser(user);
        }

        return user;
      })
    )
  }

  register(model: ApplicationUserCreate) : Observable<ApplicationUser> {
    return this.http.post<any>(`${environment.webApi}/Account/register`, model).pipe(
      map((user : ApplicationUser) => {

        if (user) {
          localStorage.setItem('happyRoutine-currentUser', JSON.stringify(user));
          this.setCurrentUser(user);
        }

        return user;
      })
    )
  }

  setCurrentUser(user: ApplicationUser) {
    this.currentUserSubject$.next(user);
  }

  public get currentUserValue(): ApplicationUser|null {
    return this.currentUserSubject$.value;
  }

  public givenUserIsLoggedIn(username: string) {
    return this.isLoggedIn() && this.currentUserValue?.username === username;
  }

  public isLoggedIn() {
    const currentUser = this.currentUserValue;
    const isLoggedIn = !!currentUser && !!currentUser.token;
    return isLoggedIn;
  }

  logout() {
    localStorage.removeItem('happyRoutine-currentUser');
    this.currentUserSubject$.next(null);
  }


}
