import { OrganizationService } from './../../../_services/organization/organization.service';

import { VoterType } from './../../../_models/VoterType';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

import { AlertifyService } from 'src/app/_services/alertify.service';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { Router } from '@angular/router';
import { Voter } from 'src/app/_models/Voter';

@Component({
  selector: 'app-add-engenere',
  templateUrl: './add-engenere.component.html',
  styleUrls: ['./add-engenere.component.scss']
})
export class AddEngenereComponent implements OnInit {
  voterForm: FormGroup;
  voter: Voter;
  voterTypes: VoterType[];
  constructor(
    private fb: FormBuilder,
    private alertify: AlertifyService,
    private voterService: VoterService,
    private router: Router,
    private organizationService: OrganizationService
  ) {}

  ngOnInit() {
    this.createVoterForm();
    this.getOrganizationTypesList();
  }
  createVoterForm() {
    this.voterForm = this.fb.group({
      code: ['', Validators.required],
      firstNameArabic: ['', Validators.required],
      fatherNameArabic: ['', Validators.required],
      familyArabic: ['', Validators.required],
      firstName: ['', Validators.required],
      fatherName: ['', Validators.required],
      family: ['', Validators.required],
      nationality: ['', Validators.required],
      speciality: ['', Validators.required],
      subChapter: ['', Validators.required],
      birthDate: ['', Validators.required],
      birthCountry: ['', Validators.required],
      birthPlace: ['', Validators.required],
      civilIdMouhafava: ['', Validators.required],
      civilIdKadaa: ['', Validators.required],
      civilIdRegion: ['', Validators.required],
      RegisteryNumber: ['', Validators.required],
      civilIdPlace: ['', Validators.required],
      registration: ['', Validators.required],
      lastCoveredYear: ['', Validators.required],
      graduation: ['', Validators.required],
      school: ['', Validators.required],
      graduationCountry: ['', Validators.required],
      addressWork: ['', Validators.required],
      mobileWork: ['', Validators.required],
      phoneWork: ['', Validators.required],
      addressHome: ['', Validators.required],
      mobileHome: ['', Validators.required],
      phoneHome: ['', Validators.required],
      email: ['', Validators.required],
      religion: ['', Validators.required],
      politic: ['', Validators.required],
      VoterTypeId: [1, Validators.required]
    });
  }
  addVoter() {
    if (this.voterForm.valid) {
      this.voter = Object.assign({}, this.voterForm.value);
      this.voterService.addVoter(this.voter).subscribe(
        next => {
          this.alertify.success('Voter Added Succesfully');
          this.voterForm.reset();
        },
        error => {
          // console.log(error);
          this.alertify.error(error);
        },
        () => {}
      );
    }
  }
  getOrganizationTypesList() {
    this.organizationService.getOrganizationTypes().subscribe(
      (types: VoterType[]) => {
        this.voterTypes = types;
      },
      error => {
        // console.log(error);
      }
    );
  }
  cancel() {
    this.router.navigate(['/admin']);
  }
}
