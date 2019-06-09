import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from './../../../_services/user/user.service';
import { User } from './../../../_models/user';
import { Component, OnInit } from '@angular/core';
import { Voter } from 'src/app/_models/Voter';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { VoterType } from 'src/app/_models/VoterType';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { OrganizationService } from 'src/app/_services/organization/organization.service';

@Component({
  selector: 'app-voter-list',
  templateUrl: './voter-list.component.html',
  styleUrls: ['./voter-list.component.scss']
})
export class VoterListComponent implements OnInit {
  voters: Voter[];
  pageNumber = 1;
  pageSize = 10;
  pagination: Pagination;
  VoterType = VoterTypeEnum;
  voterTypes: VoterType[];
  voterParams: any = {
    voterTypeId: VoterTypeEnum.all
  };
  ReferenceUsers: User[];
  constructor(
    private voterService: VoterService,
    private organizationService: OrganizationService,
    private userService: UserService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.loadItems(null, null, this.voterParams);
    this.getReferenceUsers();
  }

  loadItems(pageNumber?, pageSize?, voterParams?) {
    this.voterService
      .getVoters(pageNumber, pageSize, voterParams)
      .subscribe((res: PaginatedResult<Voter[]>) => {
        this.voters = res.result;
        this.pagination = res.pagination;
      });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadItems(this.pagination.currentPage, this.pagination.itemsPerPage);
  }

  getReferenceUsers() {
    this.userService.getOrgReference().subscribe((next: User[]) => {
      this.ReferenceUsers = next;
    });
  }
  setUserReference(voter: Voter) {
    const object = {
      id: voter.id,
      // referenceId: voter.referenceId ? voter.referenceId : 0
      referenceId: voter.referenceId
    };
    this.voterService.updateVoterReference(object).subscribe(
      next => {
        this.alertifyService.success('success');
      },
      error => {
        this.alertifyService.error(error);
      }
    );
  }

  refreshData() {
    this.loadItems(
      this.pagination.currentPage,
      this.pagination.itemsPerPage,
      this.voterParams
    );
  }
}
