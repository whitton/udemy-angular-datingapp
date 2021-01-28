import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  users: any;

  constructor(private http: HttpClient){

  }

  ngOnInit(): void {    
    this.getUsers()
  }

  getUsers() {
    // In order to access any properties of our class (users or title) you must use this
    this.http.get("http://localhost:5000/api/users").subscribe(
       response=> {this.users = response;},
       error=>{console.log(error);}
     )
  }
}
