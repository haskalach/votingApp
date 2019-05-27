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
  constructor(private organizationService: OrganizationService) {}

  ngOnInit() {
    this.getOrganizationList();
    this.getOrganizationTypesList();
  }
  updateOrganizationType(org: Organization) {
    const assignObject = {
      rganizationId: org.id,
      organizationTypeId: org.voterTypeId
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
      (types: VoterType[]) => {
        this.organizationTypes = types;
      },
      error => {
        console.log(error);
      }
    );
  }
}
