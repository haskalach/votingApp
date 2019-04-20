import { UtilitiesService } from './../../../_services/utilities/utilities.service';
import { Organization } from 'src/app/_models/Organization';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Router } from '@angular/router';
import { OrganizationService } from 'src/app/_services/organization/organization.service';
import { OrganizationType } from 'src/app/_models/OrganizationType';

@Component({
  selector: 'app-creat-organization',
  templateUrl: './creat-organization.component.html',
  styleUrls: ['./creat-organization.component.scss']
})
export class CreatOrganizationComponent implements OnInit {
  regitrationForm: FormGroup;
  organization: Organization;
  organizationTypes: OrganizationType[];
  constructor(
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router,
    private organizationService: OrganizationService,
    private utilities: UtilitiesService
  ) {}
  ngOnInit() {
    this.createRegisterForm();
    this.getOrganizationTypesList();
  }
  createRegisterForm() {
    this.regitrationForm = this.fb.group({
      name: ['', Validators.required],
      organizationTypeId: [null, Validators.required]
    });
  }
  register() {
    if (this.regitrationForm.valid) {
      this.organization = Object.assign({}, this.regitrationForm.value);
      this.organizationService.createOrganization(this.organization).subscribe(
        next => {
          this.alertify.success('Organization Created Successfully ');
          this.regitrationForm.reset();
          // console.log('resgitration success');
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
    this.router.navigate(['/admin/organizations']);
  }
  getOrganizationTypesList() {
    this.organizationService.getOrganizationTypes().subscribe(
      (types: OrganizationType[]) => {
        this.organizationTypes = types;
      },
      error => {
        console.log(error);
      }
    );
  }
}
