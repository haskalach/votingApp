import { UtilitiesService } from './../../../_services/utilities/utilities.service';
import { AlertifyService } from './../../../_services/alertify.service';
import { AuthService } from './../../../_services/auth.service';

import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { User } from 'src/app/_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  regitrationForm: FormGroup;
  user: User;
  @Output() cancelRegister = new EventEmitter();
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
    this.regitrationForm = this.fb.group(
      {
        gender: ['male'],
        userName: ['', Validators.required],
        name: [''],
        dateOfBirth: [null, Validators.required],
        city: [''],
        country: [''],
        password: [
          null,
          [
            Validators.required,
            Validators.minLength(4),
            Validators.maxLength(8)
          ]
        ],
        confirmPassword: [null, Validators.required]
      },
      { validator: this.passwordMatchValidators }
    );
  }

  passwordMatchValidators(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { missmatch: true };
  }

  register() {
    console.log(this.regitrationForm);
    if (this.regitrationForm.valid) {
      this.user = Object.assign({}, this.regitrationForm.value);
      this.auth.resgiter(this.user).subscribe(
        next => {
          // console.log('resgitration success');
          this.router.navigate(['/admin/users']);
        },
        error => {
          console.log(error);
          this.alertify.error(error);
        },
        () => {
          // this.auth.login(this.user).subscribe(() => {
          //   this.router.navigate(['/members']);
          // });
        }
      );
    } else {
      this.utilities.validateAllFormFields(this.regitrationForm);
    }
  }
  cancel() {
    this.router.navigate(['/admin']);
  }
}
