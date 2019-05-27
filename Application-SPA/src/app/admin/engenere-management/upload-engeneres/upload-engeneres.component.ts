import { AlertifyService } from './../../../_services/alertify.service';
import { VoterService } from './../../../_services/voter/voter.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpEventType } from '@angular/common/http';
import { VoterTypeEnum } from 'src/app/_enum/VoterType.enum';

@Component({
  selector: 'app-upload-engeneres',
  templateUrl: './upload-engeneres.component.html',
  styleUrls: ['./upload-engeneres.component.scss']
})
export class UploadEngeneresComponent implements OnInit {
  public progress: number;
  public message: string;
  baseUrl = environment.apiUrl;
  fileToUpload: File = null;
  formData;
  @ViewChild('file') fileInput: ElementRef;
  constructor(
    private voterService: VoterService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}
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
    this.voterService
      .uploadData(this.formData, 'Voter/Upload/' + VoterTypeEnum.engenere)
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progress = Math.round((100 * event.loaded) / event.total);
        } else if (event.type === HttpEventType.Response) {
          console.log({ event });
          this.alertify.success(event.body['length'] + ' added');
          this.fileInput.nativeElement.value = '';
        }
      });
  }
}
