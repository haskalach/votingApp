import { OrganizationService } from 'src/app/_services/organization/organization.service';
import { Voter } from './../../_models/Voter';
import { Engenere } from '../../_models/Engenere';
import { Component, OnInit } from '@angular/core';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { VoterType } from 'src/app/_models/VoterType';

@Component({
  selector: 'app-engenere-management',
  templateUrl: './engenere-management.component.html',
  styleUrls: ['./engenere-management.component.scss']
})
export class EngenereManagementComponent implements OnInit {
  voters: Voter[];
  pageNumber = 1;
  pageSize = 10;
  pagination: Pagination;
  VoterType = VoterTypeEnum;
  voterTypeId = VoterTypeEnum.all;
  voterTypes: VoterType[];
  constructor(
    private voterService: VoterService,
    private organizationService: OrganizationService
  ) {}

  ngOnInit() {
    this.getOrganizationTypesList();
    this.loadItems(this.voterTypeId);
  }

  loadItems(voterTypeId, pageNumber?, pageSize?) {
    this.voterService
      .getEngeneres(voterTypeId, pageNumber, pageSize)
      .subscribe((res: PaginatedResult<Voter[]>) => {
        this.voters = res.result;
        this.pagination = res.pagination;
      });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadItems(
      this.voterTypeId,
      this.pagination.currentPage,
      this.pagination.itemsPerPage
    );
  }

  getOrganizationTypesList() {
    this.organizationService.getOrganizationTypes().subscribe(
      (types: VoterType[]) => {
        this.voterTypes = types;
      },
      error => {
        console.log(error);
      }
    );
  }
  voterTypeChange() {
    this.loadItems(
      this.voterTypeId,
      this.pagination.currentPage,
      this.pagination.itemsPerPage
    );
  }
  // exportVoters() {
  //   this.voterService.exportVoters().subscribe(next => {
  //     console.log(next);
  //     window.open(next.toString());
  //     // this.downloadFile(next.toString());
  //   });
  // }
}
