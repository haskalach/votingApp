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
  constructor() {}

  ngOnInit() {}
}
