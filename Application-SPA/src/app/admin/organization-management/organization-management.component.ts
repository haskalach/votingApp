import { AlertifyService } from './../../_services/alertify.service';
import { VoterType } from './../../_models/VoterType';
import { Organization } from './../../_models/Organization';
import { Component, OnInit } from '@angular/core';
import { OrganizationService } from 'src/app/_services/organization/organization.service';

@Component({
  selector: 'app-organization-management',
  templateUrl: './organization-management.component.html',
  styleUrls: ['./organization-management.component.scss']
})
export class OrganizationManagementComponent implements OnInit {
  organizations: Organization[];
  organizationTypes: VoterType[];
  constructor(
    private organizationService: OrganizationService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.getOrganizationList();
    this.getOrganizationTypesList();
  }
  updateOrganizationType(org: Organization) {
    const assignObject = {
      id: org.id,
      VoterTypeId: org.voterTypeId
    };
    this.organizationService.updateOrganizationType(assignObject).subscribe(
      next => {
        this.alertifyService.success('updatedSuccesfully');
      },
      error => {
        this.alertifyService.error(error);
      }
    );
  }

  getOrganizationList() {
    this.organizationService.getOrganizations().subscribe(
      (organizations: Organization[]) => {
        this.organizations = organizations;
      },
      error => {
        console.log(error);
      }
    );
  }
  getOrganizationTypesList() {
    this.organizationService.getOrganizationTypes().subscribe(
      (types: VoterType[]) => {
        this.organizationTypes = types;
      },
      error => {
        console.log(error);
      }
    );
  }
}
