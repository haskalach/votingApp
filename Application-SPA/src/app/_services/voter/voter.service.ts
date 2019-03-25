import { Injectable } from '@angular/core';
import { Voter } from 'src/app/_models/voter';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class VoterService {
  repo = 'voter';
  baseUrl = 'http://localhost:5000/api/';
  constructor(private http: HttpClient) {}
  addVoter(voter: Voter) {
    return this.http.post(this.baseUrl + this.repo, voter);
  }
}
