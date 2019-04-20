import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { OrganizationService } from 'src/app/_services/organization/organization.service';
import { UtilitiesService } from 'src/app/_services/utilities/utilities.service';
import { OrganizationType } from 'src/app/_models/OrganizationType';

@Component({
  selector: 'app-create-organization-type',
  templateUrl: './create-organization-type.component.html',
  styleUrls: ['./create-organization-type.component.scss']
})
export class CreateOrganizationTypeComponent implements OnInit {
  regitrationForm: FormGroup;
  organizationType: OrganizationType;
  constructor(
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private organizationService: OrganizationService,
    private utilities: UtilitiesService
  ) {}
  ngOnInit() {
    this.createRegisterForm();
  }
  createRegisterForm() {
    this.regitrationForm = this.fb.group({
      name: ['', Validators.required]
    });
  }
  register() {
    if (this.regitrationForm.valid) {
      this.organizationType = Object.assign({}, this.regitrationForm.value);
      this.organizationService
        .createOrganizationType(this.organizationType)
        .subscribe(
          next => {
            this.alertify.success('organization Type Created Succesfully');
            this.regitrationForm.reset();
            // this.router.navigate(['/admin/users']);
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
    this.router.navigate(['/admin/organizations-types']);
  }
}
