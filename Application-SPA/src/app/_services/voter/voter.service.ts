import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { Engenere } from 'src/app/_models/Engenere';
import { HttpClient, HttpRequest, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from 'src/app/_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class VoterService {
  voterRepo = 'Voter';
  baseUrl = 'http://localhost:5000/api/';
  constructor(private http: HttpClient) {}
  addEngenere(voter: Engenere) {
    return this.http.post(this.baseUrl + this.voterRepo, voter);
  }
  getEngeneres(
    page?,
    itemsPerPage?,
    voterParams?
  ): Observable<PaginatedResult<Engenere[]>> {
    const paginatedResult: PaginatedResult<Engenere[]> = new PaginatedResult<
      Engenere[]
    >();
    let params = new HttpParams();
    if (voterParams) {
      params = params.append('voterTypeId', voterParams.voterTypeId);
      if (voterParams.religion) {
        params = params.append('religion', voterParams.religion);
      }
      if (voterParams.politic) {
        params = params.append('politic', voterParams.politic);
      }
    }

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http
      .get<Engenere[]>(this.baseUrl + this.voterRepo, {
        observe: 'response',
        params
      })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
            return paginatedResult;
          }
        })
      );
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
