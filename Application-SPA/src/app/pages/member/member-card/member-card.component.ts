import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user/user.service';
import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.scss']
})
export class MemberCardComponent implements OnInit {
  @Input() user: User;
  constructor(
    private userSrvice: UserService,
    private alertify: AlertifyService
  ) {}

  ngOnInit() {}
  sendLike(recipientId: number) {
    this.userSrvice.sendLike(recipientId).subscribe(
      data => {
        this.alertify.success('You have like ' + this.user.knowAs);
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
}
