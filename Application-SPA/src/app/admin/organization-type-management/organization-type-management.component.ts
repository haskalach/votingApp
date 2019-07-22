import { AlertifyService } from 'src/app/_services/alertify.service';
import { Component, OnInit } from '@angular/core';
import { OrganizationService } from 'src/app/_services/organization/organization.service';
import { VoterType } from 'src/app/_models/VoterType';

@Component({
  selector: 'app-organization-type-management',
  templateUrl: './organization-type-management.component.html',
  styleUrls: ['./organization-type-management.component.scss']
})
export class OrganizationTypeManagementComponent implements OnInit {
  organizationTypes: VoterType[];
  constructor(
    private organizationService: OrganizationService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.getOrganizationTypesList();
  }
  getOrganizationTypesList() {
    this.organizationService.getOrganizationTypes().subscribe(
      (types: VoterType[]) => {
        this.organizationTypes = types;
      },
      error => {
        // console.log(error);
      }
    );
  }
  deleteOrganizationType(id) {
    this.organizationService.deleteOrganizationType(id).subscribe(
      next => {
        this.alertifyService.success('organization type Deleted Succesfully');
        this.getOrganizationTypesList();
      },
      error => {
        this.alertifyService.error('organizationcant type cant be deleted');
      }
    );
  }
}
