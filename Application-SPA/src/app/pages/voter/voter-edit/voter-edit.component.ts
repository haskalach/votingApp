import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormGroup, FormControl } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Voter } from 'src/app/_models/Voter';
import { ActivatedRoute } from '@angular/router';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';

@Component({
  selector: 'app-voter-edit',
  templateUrl: './voter-edit.component.html',
  styleUrls: ['./voter-edit.component.scss']
})
export class VoterEditComponent implements OnInit {
  voter: Voter;
  id = 0;
  VotingYears: string[] = null;
  currentYear: number;
  VoterTypeEnum = VoterTypeEnum;
  VoterForm: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private voterSrvice: VoterService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.VoterForm = new FormGroup({
      lastCoveredYear: new FormControl(),
      addressWork: new FormControl(),
      mobileWork: new FormControl(),
      phoneWork: new FormControl(),
      addressHome: new FormControl(),
      mobileHome: new FormControl(),
      phoneHome: new FormControl(),
      religion: new FormControl(),
      politic: new FormControl(),
      referenceId: new FormControl(null)
    });
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.id = +params['id'];
        this.getVoter(this.id);
      }
    });
  }

  getVoter(id) {
    this.voterSrvice.getVoter(id).subscribe((next: Voter) => {
      console.log(next);
      this.voter = next;
      this.VoterForm.patchValue(this.voter);
      this.VotingYears = [];
      this.voter.votingYears.forEach(year => {
        this.VotingYears.push(year.year.toString());
      });
    });
  }
  updateVoter() {
    this.voterSrvice.updateVoter(this.id, this.VoterForm.value).subscribe(
      next => {
        this.alertifyService.success('updated succesfully');
      },
      error => {
        this.alertifyService.error(error);
      }
    );
  }
}
