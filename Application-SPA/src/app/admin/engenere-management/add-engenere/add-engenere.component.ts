import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Engenere } from 'src/app/_models/Engenere';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-engenere',
  templateUrl: './add-engenere.component.html',
  styleUrls: ['./add-engenere.component.scss']
})
export class AddEngenereComponent implements OnInit {
  voterForm: FormGroup;
  voter: Engenere;
  constructor(
    private fb: FormBuilder,
    private alertify: AlertifyService,
    private voterService: VoterService,
    private router: Router
  ) {}

  ngOnInit() {
    this.createVoterForm();
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
      politic: ['', Validators.required]
    });
  }
  addVoter() {
    if (this.voterForm.valid) {
      this.voter = Object.assign({}, this.voterForm.value);
      this.voterService.addEngenere(this.voter).subscribe(
        next => {
          this.alertify.success('Voter Added Succesfully');
          this.voterForm.reset();
        },
        error => {
          console.log(error);
          this.alertify.error(error);
        },
        () => {}
      );
    }
  }
  cancel() {
    this.router.navigate(['/admin']);
  }
}
