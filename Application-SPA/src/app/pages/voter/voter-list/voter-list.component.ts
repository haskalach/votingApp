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
import { ConfigList } from 'src/app/_models/configList';

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
    code: null,
    firstNameArabic: null,
    fatherNameArabic: null,
    familyArabic: null,
    subChapter: null,
    civilIdMouhavaza: null,
    civilIdKadaa: null,
    civilIdRegion: null,
    civilIdPlace: null,
    school: null,
    religion: null,
    politic: null,
    attend: null,
    abroad: null,
    contacted: null,
    referenceId: null
  };
  ReferenceUsers: User[];
  ConfigList: ConfigList = {
    religion: [],
    politics: [],
    subChapter: [],
    civilIdMouhavaza: [],
    civilIdKadaa: [],
    civilIdRegion: [],
    referenceUsers: []
  };
  constructor(
    private voterService: VoterService,
    private organizationService: OrganizationService,
    private userService: UserService,
    private alertifyService: AlertifyService
  ) {}

  ngOnInit() {
    this.loadItems(null, null, this.voterParams);
    this.getReferenceUsers();
    this.getConfiguration();
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
    this.loadItems(
      this.pagination.currentPage,
      this.pagination.itemsPerPage,
      this.voterParams
    );
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

  getConfiguration() {
    this.voterService.getConfig().subscribe(next => {
      this.ConfigList = next;
    });
  }
  exportVotersFiltered() {
    this.voterService.exportVotersFiltered(this.voterParams).subscribe(
      next => {
        // console.log({ next });
        window.location.href = next['url'];
      },
      error => {
        this.alertifyService.error('could not fetch data');
        // console.log({ error });
      }
    );
  }
}
