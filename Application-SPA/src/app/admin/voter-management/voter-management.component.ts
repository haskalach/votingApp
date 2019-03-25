import { VoterService } from './../../_services/voter/voter.service';
import { Component, OnInit } from '@angular/core';
import { Voter } from 'src/app/_models/voter';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-voter-management',
  templateUrl: './voter-management.component.html',
  styleUrls: ['./voter-management.component.scss']
})
export class VoterManagementComponent implements OnInit {
  voters: Voter[];
  constructor(private voterService: VoterService) {}

  ngOnInit() {
    this.getVoters();
  }
  getVoters() {
    this.voterService.getVoters().subscribe(
      (voters: Voter[]) => {
        this.voters = voters;
      },
      error => {
        console.log(error);
      }
    );
  }
}
