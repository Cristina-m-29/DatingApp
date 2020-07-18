import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';

  constructor(private http: HttpClient) { }

  login(model: any){
    return this.http.post(this.baseUrl + 'login', model)
      .pipe(
        map((response: any) => {
          const user = response;
          if (user){
            localStorage.setItem('token', user.token);
          }
        })
      );
  }

  loggedIn(){
    const token = localStorage.getItem('token');
    if (token === '' || token === null){
      return false;
    }
    else {
      return true;
    }
  }

  register(model: any){
    return this.http.post(this.baseUrl + 'register', model);
  }
}
