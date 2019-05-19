import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Engenere } from 'src/app/_models/Engenere';
import { HttpClient, HttpRequest } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class VoterService {
  engrepo = 'Engenere';
  baseUrl = 'http://localhost:5000/api/';
  constructor(private http: HttpClient) {}
  addEngenere(voter: Engenere) {
    return this.http.post(this.baseUrl + this.engrepo, voter);
  }
  getEngeneres() {
    return this.http.get(this.baseUrl + this.engrepo);
  }
  uploadData(formData, uploadUrl) {
    const uploadReq = new HttpRequest(
      'POST',
      this.baseUrl + uploadUrl,
      formData,
      {
        reportProgress: true
      }
    );
    return this.http.request(uploadReq);
  }
}
