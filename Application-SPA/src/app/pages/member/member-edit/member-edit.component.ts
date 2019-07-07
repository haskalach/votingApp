import { AuthService } from './../../../_services/auth.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, HostListener } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.scss']
})
export class MemberEditComponent implements OnInit {
  changepasswordForm: FormGroup;
  user: User;
  userForm: FormGroup;
  photoUrl: string;
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.userForm.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private userService: UserService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.changepasswordForm = new FormGroup({
      currentPassword: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
    this.authService.currentPhotoUrl.subscribe(p => {
      this.photoUrl = p;
    });
    this.userForm = new FormGroup({
      introduction: new FormControl(),
      lookingFor: new FormControl(),
      interests: new FormControl(),
      city: new FormControl(),
      country: new FormControl()
    });
    this.route.data.subscribe(data => {
      this.user = data['user'];
      // console.log(this.user);
    });
    this.userForm.patchValue(this.user);
  }
  updateUser() {
    // console.log(this.userForm);
    this.userService.updateUser(this.userForm.value).subscribe(
      success => {
        this.alertify.success('Updated Succesfully');
        this.userForm.markAsPristine();
        this.userForm.markAsUntouched();
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
  updateMainPhoto(event: string) {
    this.user.photoUrl = event;
  }
  onSubmit() {
    if (this.changepasswordForm.valid) {
      this.authService.changePassword(this.changepasswordForm.value).subscribe(
        next => {
          // console.log('logged in succesfully');
          this.alertify.success('password changed succesfully');
          this.changepasswordForm.reset();
        },
        error => {
          this.alertify.error(error);
        },
        () => {
          // this.router.navigate(['/members']);
        }
      );
    }
  }
}
