import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user/user.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-org-users',
  templateUrl: './org-users.component.html',
  styleUrls: ['./org-users.component.scss']
})
export class OrgUsersComponent implements OnInit {
  users: User[];
  constructor(private userService: UserService) {}

  ngOnInit() {
    this.getOrgUSers();
  }
  getOrgUSers() {
    this.userService.getOrgUsers().subscribe(next => {
      this.users = next;
      console.log({next});
    });
  }
}
