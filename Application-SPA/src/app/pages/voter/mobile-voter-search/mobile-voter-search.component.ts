import { Component, OnInit } from '@angular/core';
import { Voter } from 'src/app/_models/Voter';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { ActivatedRoute } from '@angular/router';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-mobile-voter-search',
  templateUrl: './mobile-voter-search.component.html',
  styleUrls: ['./mobile-voter-search.component.scss']
})
export class MobileVoterSearchComponent implements OnInit {
  voter: Voter;
  code = '';
  VotingYears: string[] = null;
  currentYear: number;
  VoterTypeEnum = VoterTypeEnum;
  constructor(
    private route: ActivatedRoute,
    private voterSrvice: VoterService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.currentYear = new Date().getFullYear();
  }

  getVoter() {
    if (this.code) {
      this.voterSrvice.getVoterByCode(this.code).subscribe(
        (next: Voter) => {
          // console.log(next);
          this.voter = next;
          if (next) {
            this.code = '';
            this.VotingYears = [];
            this.voter.votingYears.forEach(year => {
              this.VotingYears.push(year.year.toString());
            });
          }
        },
        error => {
          this.voter = null;
        }
      );
    } else {
      this.voter = null;
    }
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
    if (this.voter) {
      this.voterSrvice.vote(this.voter.id).subscribe(
        next => {
          this.VotingYears.push(this.currentYear.toString());
          // console.log(this.VotingYears);
        },
        error => {
          this.alertifyService.error('could not vote for this user');
        }
      );
    }
  }
  appendNumber(number) {
    this.code = this.code + number;
  }
  delete() {
    if (this.code != null && this.code.length > 0) {
      this.code = this.code.substring(0, this.code.length - 1);
    }
  }
  clearmethod(){
    this.code = '';
    this.voter = null;
  }
}
