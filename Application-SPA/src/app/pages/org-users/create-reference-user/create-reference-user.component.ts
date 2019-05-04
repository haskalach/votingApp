import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/_models/user';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Router } from '@angular/router';
import { UtilitiesService } from 'src/app/_services/utilities/utilities.service';

@Component({
  selector: 'app-create-reference-user',
  templateUrl: './create-reference-user.component.html',
  styleUrls: ['./create-reference-user.component.scss']
})
export class CreateReferenceUserComponent implements OnInit {
  regitrationForm: FormGroup;
  user: User;

  constructor(
    private auth: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private utilities: UtilitiesService
  ) {}

  ngOnInit() {
    this.createRegisterForm();
  }
  createRegisterForm() {
    this.regitrationForm = this.fb.group({
      userName: ['', Validators.required],
      email: ['', Validators.required]
    });
  }

  register() {
    if (this.regitrationForm.valid) {
      this.user = Object.assign({}, this.regitrationForm.value);
      this.auth.CreateReferenceUser(this.user).subscribe(
        next => {
          // console.log('resgitration success');
          this.router.navigate(['/users']);
        },
        error => {
          console.log(error);
          this.alertify.error(error);
        },
        () => {}
      );
    } else {
      this.utilities.validateAllFormFields(this.regitrationForm);
    }
  }
  cancel() {
    this.router.navigate(['/admin']);
  }
}
