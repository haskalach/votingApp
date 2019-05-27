import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EngenereService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}
  // addPhotoForItem(fileToUpload: File) {
  //   const formData: FormData = new FormData();
  //   console.log({ fileToUpload });
  //   formData.append('File', fileToUpload, fileToUpload.name);
  //   return this.http.post(
  //     this.baseUrl + 'api/EngenereUpload',
  //     formData,
  //     {
  //       reportProgress: true
  //     }
  //   );
  // }
}
