import { User } from 'src/app/_models/user';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  repo = 'auth/';
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/images/user.png');
  currentPhotoUrl = this.photoUrl.asObservable();
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  changeMemberPhoto(PhotoUrl: string) {
    this.photoUrl.next(PhotoUrl);
  }
  login(model: any) {
    return this.http.post(this.baseUrl + this.repo + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          localStorage.setItem('user', JSON.stringify(user.user));
          this.decodedToken = this.jwtHelper.decodeToken(user.token);
          this.currentUser = user.user;
          this.changeMemberPhoto(this.currentUser.photoUrl);
        }
      })
    );
  }
  changePassword(model: any) {
    return this.http.post(this.baseUrl + this.repo + 'changePassword', model);
  }

  resgiter(user: User) {
    return this.http.post(this.baseUrl + this.repo + 'register', user);
  }
  CreateReferenceUser(user: User) {
    return this.http.post(this.baseUrl + this.repo + 'createReference', user);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
  roleMatch(allowedRoles): boolean {
    let isMatch = false;
    if (!this.decodedToken) {
      return isMatch;
    } else {
      let userRoles = this.decodedToken.role as Array<string>;
      if (userRoles instanceof Array) {
      } else {
        userRoles = [userRoles];
      }
      allowedRoles.forEach(element => {
        if (userRoles.includes(element)) {
          isMatch = true;
          return;
        }
      });
      return isMatch;
    }
  }
  TypeMatch(allowedTypes): boolean {
    let isMatch = false;
    const userTypes = this.decodedToken.organizationType as string;
    if (userTypes === allowedTypes) {
      isMatch = true;
    } else {
      return;
    }
    return isMatch;
  }
}
