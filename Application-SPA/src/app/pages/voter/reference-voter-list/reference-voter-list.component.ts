import { Component, OnInit } from '@angular/core';
import { Voter } from 'src/app/_models/Voter';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { VoterType } from 'src/app/_models/VoterType';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { ConfigList } from 'src/app/_models/configList';

@Component({
  selector: 'app-reference-voter-list',
  templateUrl: './reference-voter-list.component.html',
  styleUrls: ['./reference-voter-list.component.scss']
})
export class ReferenceVoterListComponent implements OnInit {
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
    contacted: null,
    voted: null
  };
  ConfigList: ConfigList = {
    religion: [],
    politics: [],
    subChapter: [],
    civilIdMouhavaza: [],
    civilIdKadaa: [],
    civilIdRegion: [],
    referenceUsers: []
  };
  constructor(private voterService: VoterService) {}

  ngOnInit() {
    this.loadItems(null, null, this.voterParams);
  }

  loadItems(pageNumber?, pageSize?, voterParams?) {
    this.voterService
      .getReferenceVoters(pageNumber, pageSize, voterParams)
      .subscribe((res: PaginatedResult<Voter[]>) => {
        this.voters = res.result;
        this.pagination = res.pagination;
      });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadItems(this.pagination.currentPage, this.pagination.itemsPerPage,this.voterParams);
  }

  refreshData() {
    this.loadItems(
      this.pagination.currentPage,
      this.pagination.itemsPerPage,
      this.voterParams
    );
  }
  // getConfiguration() {
  //   this.voterService.getConfig().subscribe(next => {
  //     this.ConfigList = next;
  //   });
  // }
}
