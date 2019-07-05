import { UtilitiesService } from './../../../_services/utilities/utilities.service';
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
    private organizationService: OrganizationService,
    private utilities: UtilitiesService
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
      firstName: [''],
      fatherName: [''],
      family: [''],
      nationality: [''],
      speciality: [''],
      subChapter: [''],
      birthDate: ['', Validators.required],
      birthCountry: [''],
      birthPlace: [''],
      civilIdMouhavaza: [''],
      civilIdKadaa: [''],
      civilIdRegion: [''],
      RegisteryNumber: [''],
      civilIdPlace: [''],
      registration: ['', Validators.required],
      lastCoveredYear: [''],
      graduation: ['', Validators.required],
      school: [''],
      graduationCountry: [''],
      addressWork: [''],
      mobileWork: [''],
      phoneWork: [''],
      addressHome: [''],
      mobileHome: [''],
      phoneHome: [''],
      email: [''],
      religion: ['', Validators.required],
      politic: [''],
      VoterTypeId: [null, Validators.required]
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
          //console.log(error);
          this.alertify.error(error);
        },
        () => {}
      );
    } else {
      this.utilities.validateAllFormFields(this.voterForm);
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
