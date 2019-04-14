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
}
