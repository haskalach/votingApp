import { VoterType } from '../../_models/VoterType';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Organization } from 'src/app/_models/Organization';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrganizationService {
  repo = 'organization';
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}
  getOrganizations(): Observable<Organization[]> {
    return this.http.get<Organization[]>(this.baseUrl + this.repo);
  }
  getOrganizationTypes(): Observable<VoterType[]> {
    return this.http.get<VoterType[]>(
      this.baseUrl + this.repo + '/type'
    );
  }
  createOrganization(model) {
    return this.http.post(this.baseUrl + this.repo, model);
  }
  createOrganizationType(model) {
    return this.http.post(this.baseUrl + this.repo + '/type', model);
  }
  updateOrganizationType(obj) {}
}
