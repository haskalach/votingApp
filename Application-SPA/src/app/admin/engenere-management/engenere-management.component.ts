import {  Engenere } from '../../_models/Engenere';
import { Component, OnInit } from '@angular/core';
import { VoterService } from 'src/app/_services/voter/voter.service';

@Component({
  selector: 'app-engenere-management',
  templateUrl: './engenere-management.component.html',
  styleUrls: ['./engenere-management.component.scss']
})
export class EngenereManagementComponent implements OnInit {
  voters: Engenere[];
  constructor(private voterService: VoterService) {}

  ngOnInit() {
    this.getVoters();
  }
  getVoters() {
    this.voterService.getEngeneres().subscribe(
      (voters: Engenere[]) => {
        this.voters = voters;
      },
      error => {
        console.log(error);
      }
    );
  }

  // exportVoters() {
  //   this.voterService.exportVoters().subscribe(next => {
  //     console.log(next);
  //     window.open(next.toString());
  //     // this.downloadFile(next.toString());
  //   });
  // }
}
