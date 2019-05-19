import { Engenere } from '../../_models/Engenere';
import { Component, OnInit } from '@angular/core';
import { VoterService } from 'src/app/_services/voter/voter.service';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-engenere-management',
  templateUrl: './engenere-management.component.html',
  styleUrls: ['./engenere-management.component.scss']
})
export class EngenereManagementComponent implements OnInit {
  voters: Engenere[];
  pageNumber = 1;
  pageSize = 10;
  pagination: Pagination;
  constructor(private voterService: VoterService) {}

  ngOnInit() {
    this.loadItems();
  }

  loadItems(pageNumber?, pageSize?) {
    this.voterService
      .getEngeneres(pageNumber, pageSize)
      .subscribe((res: PaginatedResult<Engenere[]>) => {
        this.voters = res.result;
        this.pagination = res.pagination;
      });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadItems(this.pagination.currentPage, this.pagination.itemsPerPage);
  }

  // exportVoters() {
  //   this.voterService.exportVoters().subscribe(next => {
  //     console.log(next);
  //     window.open(next.toString());
  //     // this.downloadFile(next.toString());
  //   });
  // }
}
