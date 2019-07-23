import { UserService } from 'src/app/_services/user/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Voter } from './../../../_models/Voter';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { FormGroup, FormControl } from '@angular/forms';
import { Location } from '@angular/common';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-voter-view',
  templateUrl: './voter-view.component.html',
  styleUrls: ['./voter-view.component.scss']
})
export class VoterViewComponent implements OnInit {
  voter: Voter;
  id = 0;
  VotingYears: string[] = null;
  currentYear: number;
  VoterTypeEnum = VoterTypeEnum;
  VoterForm: FormGroup;
  ReferenceUsers: User[];
  constructor(
    private route: ActivatedRoute,
    private voterSrvice: VoterService,
    private alertifyService: AlertifyService,
    private location: Location,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.getReferenceUsers();
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.id = +params['id'];
        this.getVoter(this.id);
      }
    });
    this.currentYear = new Date().getFullYear();
    this.VoterForm = new FormGroup({
      addressWork: new FormControl(),
      mobileWork: new FormControl(),
      phoneWork: new FormControl(),
      addressHome: new FormControl(),
      mobileHome: new FormControl(),
      phoneHome: new FormControl(),
      email: new FormControl(),
      religion: new FormControl(),
      politic: new FormControl(),
      referenceId: new FormControl(null)
    });
  }

  getVoter(id) {
    this.voterSrvice.getVoter(id).subscribe((next: Voter) => {
      // console.log(next);
      this.voter = next;
      this.VoterForm.patchValue(this.voter);
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
        // console.log(this.VotingYears);
      },
      error => {
        this.alertifyService.error('could not vote for this user');
      }
    );
  }
  contact() {
    this.voterSrvice.contact(this.id).subscribe(
      next => {
        this.alertifyService.success('contacted Succesfully');
        this.voter.contacted = true;
      },
      error => {
        this.alertifyService.error('could not contact this voter');
      }
    );
  }
  attend(status) {
    const attendObj = {
      id: this.id,
      status: status
    };
    this.voterSrvice.attend(attendObj).subscribe(
      next => {
        this.alertifyService.success('set as attend succesfully');
        this.voter.attend = attendObj.status;
      },
      error => {
        this.alertifyService.error('could not set as attend');
      }
    );
  }
  abroad(status) {
    const attendObj = {
      id: this.id,
      status: status
    };
    this.voterSrvice.abroad(attendObj).subscribe(
      next => {
        this.alertifyService.success('set as Abroad succesfully');
        this.voter.abroad = attendObj.status;
      },
      error => {
        this.alertifyService.error('could not set as Abroad');
      }
    );
  }
  updateVoter() {
    this.voterSrvice.updateVoter(this.id, this.VoterForm.value).subscribe(
      next => {
        this.alertifyService.success('updated succesfully');
        this.location.back();
      },
      error => {
        this.alertifyService.error(error);
      }
    );
  }
  getReferenceUsers() {
    this.userService.getOrgReference().subscribe((next: User[]) => {
      this.ReferenceUsers = next;
    });
  }
  // findIndexInData(data, property, value) {
  //   for (let i = 0, l = data.length; i < l; i++) {
  //     if (data[i][property] === value) {
  //       return i;
  //     }
  //   }
  //   return -1;
  // }
}
