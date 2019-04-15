import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { UserProfileViewModel, NetworkUserViewModel } from '../../../_models/UserProfileViewModel';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { UserService } from '../../../app/user.service';

@Component({
  selector: 'app-info-network',
  templateUrl: './info-network.component.html',
  styleUrls: ['./info-network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfoNetworkComponent implements OnInit {
  user: UserProfileViewModel;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.getUser();
  }

  getUser(): void {
    this.userService.getUser('')
      .subscribe(
        (data: UserProfileViewModel) => {
          this.user = data;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
        });
  }

  followOrUnfollow(element, user: NetworkUserViewModel): void {
    const action = element.innerText;

    if (action === 'Follow') {
      this.userService.postFollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            this.getUser();
            // element.innerText = 'Unfollow';
          },
          (error: ErrorReportViewModel) => {
            // console.log(error);
          });
      return;
    } else {
      this.userService.postUnfollowUser(user)
        .subscribe(
          (data: NetworkUserViewModel) => {
            this.getUser();
            // element.innerText = 'Follow';
          },
          (error: ErrorReportViewModel) => {
            // console.log(error);
          });
      return;
    }
  }
}
