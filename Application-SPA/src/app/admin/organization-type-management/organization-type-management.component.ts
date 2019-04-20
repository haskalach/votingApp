import { Component, OnInit } from '@angular/core';
import { OrganizationType } from 'src/app/_models/OrganizationType';
import { OrganizationService } from 'src/app/_services/organization/organization.service';

@Component({
  selector: 'app-organization-type-management',
  templateUrl: './organization-type-management.component.html',
  styleUrls: ['./organization-type-management.component.scss']
})
export class OrganizationTypeManagementComponent implements OnInit {
  organizationTypes: OrganizationType[];
  constructor(private organizationService: OrganizationService) {}

  ngOnInit() {
    this.getOrganizationTypesList();
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
