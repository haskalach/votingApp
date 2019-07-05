import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BsModalRef } from '../../../../node_modules/ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.scss']
})
export class RolesModalComponent implements OnInit {
  @Output() updateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[] = [];

  constructor(public bsModalRef: BsModalRef) {}
  ngOnInit() {
    // console.log(this.user);
    // console.log(this.roles);
  }
  updateRoles() {
    this.updateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }
  changeRoles(role) {
    this.roles.forEach(elem => {
      if (elem.value !== role.value) {
        elem.checked = false;
      } else {
        elem.checked = true;
      }
    });
  }
}
