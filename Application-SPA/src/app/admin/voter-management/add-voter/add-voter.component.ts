import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Voter } from 'src/app/_models/voter';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-voter',
  templateUrl: './add-voter.component.html',
  styleUrls: ['./add-voter.component.scss']
})
export class AddVoterComponent implements OnInit {
  voterForm: FormGroup;
  voter: Voter;
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
      firstName: ['', Validators.required],
      birthDate: ['', Validators.required],
      registration: ['', Validators.required],
      graduation: ['', Validators.required],
      vote: [null, Validators.required],
      attend: [null, Validators.required],
      transport: [null, Validators.required],
      voted: [null, Validators.required]
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
