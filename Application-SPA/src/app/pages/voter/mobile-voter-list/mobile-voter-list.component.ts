import { Component, OnInit } from '@angular/core';
import { Voter } from 'src/app/_models/Voter';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { VoterType } from 'src/app/_models/VoterType';
import { User } from 'src/app/_models/user';
import { ConfigList } from 'src/app/_models/configList';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { OrganizationService } from 'src/app/_services/organization/organization.service';
import { UserService } from 'src/app/_services/user/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-mobile-voter-list',
  templateUrl: './mobile-voter-list.component.html',
  styleUrls: ['./mobile-voter-list.component.scss']
})
export class MobileVoterListComponent implements OnInit {
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
    attend: null,
    abroad: null,
    contacted: null
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

  getConfiguration() {
    this.voterService.getConfig().subscribe(next => {
      this.ConfigList = next;
    });
  }
}
