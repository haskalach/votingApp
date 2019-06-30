import { UserService } from './../../_services/user/user.service';
import { AlertifyService } from './../../_services/alertify.service';
import { OrganizationService } from './../../_services/organization/organization.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import {
  BsModalService,
  BsModalRef
} from '../../../../node_modules/ngx-bootstrap/modal';
import { RolesModalComponent } from '../roles-modal/roles-modal.component';
import { Organization } from 'src/app/_models/Organization';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {
  users: User[];
  bsModalRef: BsModalRef;
  organizations: Organization[];
  constructor(
    private adminService: AdminService,
    private modalService: BsModalService,
    private organizationService: OrganizationService,
    private alertityService: AlertifyService,
    private userService: UserService
  ) {}

  ngOnInit() {
    this.getUsersWithRoles();
    this.getOrganizationList();
  }

  getUsersWithRoles() {
    this.adminService.getUsersWithRoles().subscribe(
      (users: User[]) => {
        this.users = users;
      },
      error => {
        console.log(error);
      }
    );
  }
  getOrganizationList() {
    this.organizationService.getOrganizations().subscribe(
      (organizations: Organization[]) => {
        this.organizations = organizations;
      },
      error => {
        console.log(error);
      }
    );
  }
  editRolesModal(user: User) {
    const initialState = {
      user,
      roles: this.getRolesArray(user)
    };
    this.bsModalRef = this.modalService.show(RolesModalComponent, {
      initialState
    });
    this.bsModalRef.content.updateSelectedRoles.subscribe(values => {
      const rolesToUpdate = {
        roleNames: [
          ...values.filter(el => el.checked === true).map(el => el.name)
        ]
      };
      console.log({ rolesToUpdate });
      if (rolesToUpdate) {
        this.adminService.updateUserRoles(user, rolesToUpdate).subscribe(
          () => {
            user.roles = [...rolesToUpdate.roleNames];
          },
          error => {
            console.log(error);
          }
        );
      }
    });
  }
  private getRolesArray(user) {
    const roles = [];
    const userRoles = user.roles;
    const availableRoles: any[] = [
      { name: 'Admin', value: 'Admin' },
      { name: 'OrganizationAdmin', value: 'OrganizationAdmin' },
      { name: 'LaptopUser', value: 'LaptopUser' },
      { name: 'MobileUser', value: 'MobileUser' },
      { name: 'Reference', value: 'Reference' }
    ];
    for (let i = 0; i < availableRoles.length; i++) {
      let isMatch = false;
      for (let j = 0; j < userRoles.length; j++) {
        if (availableRoles[i].name === userRoles[j]) {
          isMatch = true;
          availableRoles[i].checked = true;
          roles.push(availableRoles[i]);
          break;
        }
      }
      if (!isMatch) {
        availableRoles[i].checked = false;
        roles.push(availableRoles[i]);
      }
    }
    return roles;
  }
  updateUserOrganization(user: User) {
    const assignObject = {
      userName: user.userName,
      organizationId: user.organizationId == null ? 0 : user.organizationId
    };
    console.log({ assignObject });
    this.userService.assignOrganization(assignObject).subscribe(
      next => {
        this.alertityService.success('User Organization Update Succesfully');
      },
      error => {
        this.alertityService.error('User Organization Failed to Update');
      }
    );
  }
  updateUserStatus(userId: number, status: boolean) {
    this.adminService.updateUserStatus(userId, status).subscribe(
      next => {
        this.getUsersWithRoles();
      },
      error => {
        this.alertityService.error(error);
      }
    );
  }
  updateAllUserStatus(status: boolean) {
    this.adminService.updateAllUsersStatus(status).subscribe(
      next => {
        this.getUsersWithRoles();
      },
      error => {
        this.alertityService.error(error);
      }
    );
  }

  delteUser(userId: number) {
    this.adminService.deleteUser(userId).subscribe(
      next => {
        this.alertityService.success('User Deleted Succesfully');
        this.getUsersWithRoles();
      },
      error => {
        this.alertityService.error('User Failed To Delete');
      }
    );
  }
}
