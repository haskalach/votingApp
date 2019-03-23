import { AuthService } from './../../../_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from './../../../_services/user/user.service';
import { Photo } from './../../../_models/photo';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.scss']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Output() updateMainPhotoUrl = new EventEmitter<string>();
  public uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.initializeUploader();
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 2 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = file => {
      file.withCredentials = false;
    };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo);
        if (photo.isMain) {
          this.updateMainPhoto(photo);
        }
      }
    };
  }
  setMainPhoto(photo: Photo) {
    this.userService.setMainPhoto(photo.id).subscribe(
      next => {
        this.alertify.success('sucesfully updated MainPhoto');
        this.updateMainPhoto(photo);
      },
      error => {
        this.alertify.error(error);
      }
    );
  }

  updateMainPhoto(photo: Photo) {
    this.photos.forEach((element: Photo) => {
      if (element.id === photo.id) {
        element.isMain = true;
      } else {
        element.isMain = false;
      }
    });
    this.updateMainPhotoUrl.emit(photo.url);
    this.authService.changeMemberPhoto(photo.url);
    this.authService.currentUser.photoUrl = photo.url;
    localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
  }
  deletePhoto(id: number) {
    this.alertify.confirm('Are you sure you want to delete this photo?', () => {
      this.userService.deletePhoto(id).subscribe(
        success => {
          this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
          this.alertify.success('Succesfully delete the image');
        },
        error => {
          this.alertify.error(error);
        }
      );
    });
  }

  fileSelected(event) {
    // console.log({ event });
  }
}
