import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsersWithRoles() {
    return this.http.get(this.baseUrl + 'admin/usersWithRoles');
  }

  updateUserRoles(user: User, roles: {}) {
    return this.http.post(
      this.baseUrl + 'admin/editRoles/' + user.userName,
      roles
    );
  }
  updateUserStatus(userId: number, status: boolean) {
    return this.http.post(this.baseUrl + 'admin/changeUserStatus', {
      userId: userId,
      disable: status
    });
  }
  updateAllUsersStatus(status: boolean) {
    return this.http.post(
      this.baseUrl + 'admin/changeAllUserStatus/' + status,
      {}
    );
  }
}
