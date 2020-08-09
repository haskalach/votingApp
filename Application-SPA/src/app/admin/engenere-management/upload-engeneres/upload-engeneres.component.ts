import { AlertifyService } from './../../../_services/alertify.service';
import { VoterService } from './../../../_services/voter/voter.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpEventType } from '@angular/common/http';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';
import { VoterType } from 'src/app/_models/VoterType';
import { OrganizationService } from 'src/app/_services/organization/organization.service';

@Component({
  selector: 'app-upload-engeneres',
  templateUrl: './upload-engeneres.component.html',
  styleUrls: ['./upload-engeneres.component.scss']
})
export class UploadEngeneresComponent implements OnInit {
  loading = false;
  public progress: number;
  public message: string;
  baseUrl = environment.apiUrl;
  fileToUpload: File = null;
  formData;
  VoterTypeId = VoterTypeEnum.engenere;
  voterTypes: VoterType[];
  @ViewChild('file', { static: true }) fileInput: ElementRef;
  constructor(
    private voterService: VoterService,
    private alertify: AlertifyService,
    private organizationService: OrganizationService
  ) {}

  ngOnInit() {
    this.getOrganizationTypesList();
  }
  upload(files) {
    if (files.length === 0) {
      return;
    }

    this.formData = new FormData();

    for (const file of files) {
      this.formData.append(file.name, file);
    }
  }

  uploadFiles() {
    if (this.formData) {
      this.loading = true;
      this.voterService
        .uploadData(this.formData, 'Voter/Upload/' + this.VoterTypeId)
        .subscribe(
          event => {
            if (event.type === HttpEventType.UploadProgress) {
              this.progress = Math.round((100 * event.loaded) / event.total);
            } else if (event.type === HttpEventType.Response) {
              // console.log({ event });
              this.alertify.success(' records added');
              this.fileInput.nativeElement.value = '';
              this.loading = false;
            }
          },
          error => {
            this.alertify.error('error uploading data' + error);
            this.loading = false;
          }
        );
    }
  }
  exportdata() {
    this.voterService.exportData(this.VoterTypeId).subscribe(
      next => {
        // console.log({ next });
        window.location.href = next['url'];
      },
      error => {
        // console.log({ error });
      }
    );
  }
  deleteData() {
    this.loading = true;
    this.voterService.deleteOldData(this.VoterTypeId).subscribe(
      next => {
        // console.log({ next });
        // window.location.href = next['url'];
        this.alertify.success('old data deleted Succesfully');
        this.loading = false;
      },
      error => {
        this.alertify.error('old faild to delete');
        this.loading = false;
        // console.log({ error });
      }
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
}
