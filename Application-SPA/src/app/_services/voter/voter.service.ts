import { map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpRequest, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from 'src/app/_models/pagination';
import { Voter } from 'src/app/_models/Voter';
import { environment } from 'src/environments/environment';
import { ConfigList } from 'src/app/_models/configList';

@Injectable({
  providedIn: 'root'
})
export class VoterService {
  voterRepo = 'Voter';
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}
  addVoter(voter: Voter) {
    return this.http.post(this.baseUrl + this.voterRepo, voter);
  }
  getVoters(
    page?,
    itemsPerPage?,
    voterParams?
  ): Observable<PaginatedResult<Voter[]>> {
    const paginatedResult: PaginatedResult<Voter[]> = new PaginatedResult<
      Voter[]
    >();
    let params = new HttpParams();
    if (voterParams) {
      if (voterParams.voterTypeId) {
        params = params.append('voterTypeId', voterParams.voterTypeId);
      }
      if (voterParams.religion) {
        params = params.append('religion', voterParams.religion);
      }
      if (voterParams.politic) {
        params = params.append('politic', voterParams.politic);
      }
      if (voterParams.code) {
        params = params.append('code', voterParams.code);
      }
      if (voterParams.firstNameArabic) {
        params = params.append('firstNameArabic', voterParams.firstNameArabic);
      }
      if (voterParams.familyArabic) {
        params = params.append('familyArabic', voterParams.familyArabic);
      }
      if (voterParams.fatherNameArabic) {
        params = params.append(
          'fatherNameArabic',
          voterParams.fatherNameArabic
        );
      }
      if (voterParams.subChapter) {
        params = params.append('subChapter', voterParams.subChapter);
      }
      if (voterParams.contacted !== null) {
        params = params.append('contacted', voterParams.contacted);
      }
      if (voterParams.attend !== null) {
        params = params.append('attend', voterParams.attend);
      }
      if (voterParams.abroad !== null) {
        params = params.append('abroad', voterParams.abroad);
      }
      if (voterParams.reiligion) {
        params = params.append('reiligion', voterParams.reiligion);
      }
      if (voterParams.school) {
        params = params.append('school', voterParams.school);
      }
      if (voterParams.civilIdMouhavaza) {
        params = params.append(
          'civilIdMouhavaza',
          voterParams.civilIdMouhavaza
        );
      }
      if (voterParams.civilIdKadaa) {
        params = params.append('civilIdKadaa', voterParams.civilIdKadaa);
      }
      if (voterParams.civilIdRegion) {
        params = params.append('civilIdRegion', voterParams.civilIdRegion);
      }
      if (voterParams.civilIdPlace) {
        params = params.append('civilIdPlace', voterParams.civilIdPlace);
      }
    }

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http
      .get<Voter[]>(this.baseUrl + this.voterRepo, {
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
  deleteOldData(id: number) {
    return this.http.delete(
      this.baseUrl + this.voterRepo + '/oldData' + '/' + id
    );
  }
  getVoter(id: number): Observable<Voter> {
    return this.http.get<Voter>(this.baseUrl + this.voterRepo + '/' + id);
  }
  getVoterByCode(code: string): Observable<Voter> {
    return this.http.get<Voter>(
      this.baseUrl + this.voterRepo + '/code/' + code
    );
  }
  updateVoter(id: number, voterupdate) {
    return this.http.put<Voter>(
      this.baseUrl + this.voterRepo + '/' + id,
      voterupdate
    );
  }
  updateVoterReference(voterReference) {
    return this.http.put<Voter>(
      this.baseUrl + this.voterRepo + '/updateReference',
      voterReference
    );
  }
  vote(id): Observable<Voter> {
    return this.http.post<Voter>(this.baseUrl + this.voterRepo + '/vote', {
      voterId: id
    });
  }
  contact(id): Observable<Voter> {
    return this.http.get<Voter>(
      this.baseUrl + this.voterRepo + '/contact/' + id
    );
  }
  attend(id): Observable<Voter> {
    return this.http.get<Voter>(
      this.baseUrl + this.voterRepo + '/attend/' + id
    );
  }
  getReferenceVoters(
    page?,
    itemsPerPage?,
    voterParams?
  ): Observable<PaginatedResult<Voter[]>> {
    const paginatedResult: PaginatedResult<Voter[]> = new PaginatedResult<
      Voter[]
    >();
    let params = new HttpParams();
    if (voterParams) {
      if (voterParams.voterTypeId) {
        params = params.append('voterTypeId', voterParams.voterTypeId);
      }
      if (voterParams.code) {
        params = params.append('code', voterParams.code);
      }
      if (voterParams.firstNameArabic) {
        params = params.append(
          'cofirstNameArabicde',
          voterParams.firstNameArabic
        );
      }
      if (voterParams.familyArabic) {
        params = params.append('familyArabic', voterParams.familyArabic);
      }
      if (voterParams.fatherNameArabic) {
        params = params.append(
          'fatherNameArabic',
          voterParams.fatherNameArabic
        );
      }
      if (voterParams.voted !== null) {
        params = params.append('voted', voterParams.voted);
      }
    }

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }
    return this.http
      .get<Voter[]>(this.baseUrl + this.voterRepo + '/ReferenceVoters', {
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
  exportData(id) {
    return this.http.get<string>(
      this.baseUrl + this.voterRepo + '/export/' + id
    );
  }
  getConfig() {
    return this.http.get<ConfigList>(
      this.baseUrl + this.voterRepo + '/configuration'
    );
  }
}
