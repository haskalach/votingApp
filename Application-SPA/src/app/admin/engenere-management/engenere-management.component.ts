import { UserService } from './../../_services/user/user.service';
import { OrganizationService } from 'src/app/_services/organization/organization.service';
import { Voter } from './../../_models/Voter';
import { Component, OnInit } from '@angular/core';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { VoterType } from 'src/app/_models/VoterType';
import { User } from 'src/app/_models/user';
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
  voterTypes: VoterType[];
  ReferenceUsers: User[];
  voterParams: any = {
    voterTypeId: VoterTypeEnum.all,
    religion: '',
    politic: '',
    code: 0,
    firstNameArabic: '',
    familyArabic: '',
    fatherNameArabic: '',
    subChapter: '',
    contacted: null,
    attend: null,
    abroad: null,
    reiligion: '',
    school: '',
    civilIdMouhavaza: '',
    civilIdKadaa: '',
    civilIdRegion: '',
    civilIdPlace: ''
  };
  loading = false;
  constructor(
    private voterService: VoterService,
    private organizationService: OrganizationService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.getOrganizationTypesList();
    this.loadItems(null, null, this.voterParams);
  }

  loadItems(pageNumber?, pageSize?, voterParams?) {
    this.loading = true;
    this.voterService.getVoters(pageNumber, pageSize, voterParams).subscribe(
      (res: PaginatedResult<Voter[]>) => {
        this.voters = res.result;
        this.pagination = res.pagination;
        this.loading = false;
      },
      error => {
        this.loading = false;
      }
    );
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadItems(
      this.pagination.currentPage,
      this.pagination.itemsPerPage,
      this.voterParams
    );
  }

  getOrganizationTypesList() {
    this.organizationService.getOrganizationTypes().subscribe(
      (types: VoterType[]) => {
        this.voterTypes = types;
      },
      error => {
        // console.log(error);
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
  exportData() {
    // window.location.href = 'http://localhost:5000/demo.xlsx';
    //   this.voterService.exportData().subscribe(
    //     next => {
    //       console.log({ next });
    //       window.location.href = next['url'];
    //     },
    //     error => {
    //       console.log({ error });
    //     }
    //   );
  }
}
