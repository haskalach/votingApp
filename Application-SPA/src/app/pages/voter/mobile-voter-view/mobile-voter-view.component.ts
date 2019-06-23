import { Component, OnInit } from '@angular/core';
import { Voter } from 'src/app/_models/Voter';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { ActivatedRoute } from '@angular/router';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-mobile-voter-view',
  templateUrl: './mobile-voter-view.component.html',
  styleUrls: ['./mobile-voter-view.component.scss']
})
export class MobileVoterViewComponent implements OnInit {
  voter: Voter;
  id = 0;
  VotingYears: string[] = null;
  currentYear: number;
  VoterTypeEnum = VoterTypeEnum;
  constructor(
    private route: ActivatedRoute,
    private voterSrvice: VoterService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.id = +params['id'];
        this.getVoter(this.id);
      }
    });
    this.currentYear = new Date().getFullYear();
  }

  getVoter(id) {
    this.voterSrvice.getVoter(id).subscribe((next: Voter) => {
      console.log(next);
      this.voter = next;
      this.VotingYears = [];
      this.voter.votingYears.forEach(year => {
        this.VotingYears.push(year.year.toString());
      });
    });
  }
  Voted() {
    const index = this.VotingYears.indexOf(this.currentYear.toString());
    if (index > -1) {
      return true;
    } else {
      return false;
    }
  }
  vote() {
    this.voterSrvice.vote(this.id).subscribe(
      next => {
        this.VotingYears.push(this.currentYear.toString());
        console.log(this.VotingYears);
      },
      error => {
        this.alertifyService.error('could not vote for this user');
      }
    );
  }
}
