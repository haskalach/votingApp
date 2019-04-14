import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { User } from 'src/app/_models/user';
import { PaginatedResult } from 'src/app/_models/pagination';
import { HttpParams, HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  repo = 'users';
  photoRepo = 'photos';
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getUsers(
    page?,
    itemsPerPage?,
    userParams?,
    likesParams?
  ): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<
      User[]
    >();
    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    if (userParams != null) {
      // console.log({ userParams });
      params = params.append('minAge', userParams.minAge);
      params = params.append('maxAge', userParams.maxAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }
    if (likesParams === 'Likers') {
      params = params.append('likers', 'true');
    }
    if (likesParams === 'Likees') {
      params = params.append('likees', 'true');
    }
    return this.http
      .get<User[]>(this.baseUrl + this.repo, { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }
  getUser(id: number): Observable<User> {
    return this.http.get<User>(this.baseUrl + this.repo + '/' + id);
  }
  updateUser(user: User): Observable<User> {
    return this.http.put<User>(this.baseUrl + this.repo, user);
  }
  setMainPhoto(id: number) {
    return this.http.post(
      this.baseUrl + this.repo + '/' + this.photoRepo + '/' + id + '/setMain',
      null
    );
  }
  deletePhoto(id: number) {
    return this.http.delete(
      this.baseUrl + this.repo + '/' + this.photoRepo + '/' + id
    );
  }
  sendLike(recipientId: number) {
    return this.http.post(this.baseUrl + 'users/like/' + recipientId, {});
  }
  assignOrganization(obj) {
    return this.http.post(
      this.baseUrl + this.repo + '/assignOrganization',
      obj
    );
  }
}
