import { OrganizationType } from './../../_models/OrganizationType';
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
  organizationTypes: OrganizationType[];
  constructor(private organizationService: OrganizationService) {}

  ngOnInit() {
    this.getOrganizationList();
    this.getOrganizationTypesList();
  }
  updateOrganizationType(org: Organization) {
    const assignObject = {
      rganizationId: org.id,
      organizationTypeId: org.organizationTypeId
    };
    console.log({ assignObject });
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
      (types: OrganizationType[]) => {
        this.organizationTypes = types;
      },
      error => {
        console.log(error);
      }
    );
  }
}
