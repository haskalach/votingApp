import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ApiCallService {
  httpHeader;
  httpHeaderFile;
  baseUrl = 'http://localhost:5000/api/';
  constructor(public http: HttpClient) {
    this.init();
  }

  init() {
    this.httpHeader = new HttpHeaders({
      'Content-Type': 'application/json'
      // Authorization: 'Bearer ' + this.auth.getToken()
    });
    this.httpHeaderFile = new HttpHeaders({
      // Authorization: 'Bearer ' + this.auth.getToken()
    });
    // console.log('ini', this.auth.getToken())
  }

  Get<T>(url: string) {
    return this.http.get<T>(this.baseUrl + url, {
      headers: this.httpHeader
    });
  }

  Post<T>(url: string, object?: T) {
    return this.http.post<T>(this.baseUrl + url, object, {
      headers: this.httpHeader
    });
  }

  Put<T>(url: string, object: T) {
    return this.http.put<T>(this.baseUrl + url, object, {
      headers: this.httpHeader
    });
  }

  Delete<T>(url: string, id?: number) {
    if (id) {
      return this.http.delete<T>(this.baseUrl + url + id, {
        headers: this.httpHeader
      });
    } else {
      return this.http.delete<T>(this.baseUrl + url, {
        headers: this.httpHeader
      });
    }
  }

  // when posting file
  PostFile<T>(url: string, object: T) {
    return this.http.post<T>(this.baseUrl + url, object, {
      headers: this.httpHeaderFile
    });
  }

  PutFile<T>(url: string, object: T) {
    return this.http.put<T>(this.baseUrl + url, object, {
      headers: this.httpHeaderFile
    });
  }
}
