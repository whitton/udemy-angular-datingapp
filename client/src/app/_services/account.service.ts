import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { logging } from 'protractor';
import { ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { RegisterUser } from '../Models/registeruser';
import { User } from '../Models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = "http://localhost:5000/api/";
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.baseUrl + 'account/login', model).pipe(
      map((response : User) => {
        const user = response;

        if (!!user){
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(newUser : RegisterUser){
    return this.http.post(this.baseUrl + 'account/register', newUser).pipe(
      map((user: User) => {
        if (user){
        localStorage.setItem('user', JSON.stringify(user));
        this.currentUserSource.next(user);
        }
      })
    );
  }

  setCurrentUser(user : User){
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
}
