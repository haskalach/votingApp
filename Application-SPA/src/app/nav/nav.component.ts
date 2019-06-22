import { AlertifyService } from './../_services/alertify.service';
import { AuthService } from './../_services/auth.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  loginForm: FormGroup;
  photoUrl: string;
  cartCount: number;
  constructor(
    public authService: AuthService,
    private alertify: AlertifyService,
    private router: Router
  ) {}

  ngOnInit() {
    // this.cartCount = this.cartService.cartItemCount.getValue();
    this.authService.photoUrl.subscribe(p => {
      this.photoUrl = p;
    });
    this.loginForm = new FormGroup({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required)
    });
  }
  onSubmit() {
    this.authService.login(this.loginForm.value).subscribe(
      next => {
        // console.log('logged in succesfully');
        this.alertify.success('logged in succesfully');
      },
      error => {
        this.alertify.success(error);
      },
      () => {
        // this.router.navigate(['/members']);
      }
    );
  }
  loggedIn() {
    return this.authService.loggedIn();
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alertify.message('logged out');
    this.loginForm.reset();
    this.router.navigate(['/home']);
  }
}
