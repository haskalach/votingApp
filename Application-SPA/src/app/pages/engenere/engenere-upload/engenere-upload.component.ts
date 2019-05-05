import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { EngenereService } from 'src/app/_services/engenere/engenere.service';

@Component({
  selector: 'app-engenere-upload',
  templateUrl: './engenere-upload.component.html',
  styleUrls: ['./engenere-upload.component.scss']
})
export class EngenereUploadComponent implements OnInit {
  public progress: number;
  public message: string;
  baseUrl = environment.apiUrl;
  fileToUpload: File = null;
  constructor(
    private http: HttpClient,
    private engenereService: EngenereService
  ) {}

  ngOnInit() {}
  upload(files) {
    if (files.length === 0) {
      return;
    }

    const formData = new FormData();

    for (let file of files) {
      formData.append(file.name, file);
    }

    const uploadReq = new HttpRequest(
      'POST',
      this.baseUrl + 'EngenereUpload',
      formData,
      {
        reportProgress: true
      }
    );

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round((100 * event.loaded) / event.total);
      } else if (event.type === HttpEventType.Response) {
        this.message = event.body.toString();
      }
    });
  }
}
