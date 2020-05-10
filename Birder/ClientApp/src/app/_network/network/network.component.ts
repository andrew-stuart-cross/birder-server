import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { NetworkService } from '@app/_services/network.service';
import { UserNetworkDto } from '@app/_models/UserNetworkDto';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { Subscription } from 'rxjs';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';

@Component({
  selector: 'app-network',
  templateUrl: './network.component.html',
  styleUrls: ['./network.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class NetworkComponent implements OnInit {
  // *********************
    // => disable network info???
  // *********************
  network: UserNetworkDto;
  subscription: Subscription;
  tabstatus = {};
  active;
  
  constructor(private networkService: NetworkService
    , private toast: ToastrService) { }


    ngOnInit() {
      this.active = 1;
      this.tabstatus = {};
      
      this.getUserNetwork();
      this.subscription = this.networkService.networkChanged$
      .subscribe(_ => {
        this.onNetworkChanged();
      });
    }
  
    onNetworkChanged() {
      this.getUserNetwork();
    }
  
    getUserNetwork(): void {
      this.networkService.getUserNetwork()
        .subscribe(
          (data: UserNetworkDto) => {
            this.network = data;
          },
          (error: ErrorReportViewModel) => {
            console.log(error);
          });
    }

    followOrUnfollow(element, user: NetworkUserViewModel): void {
      const action = element.innerText;
  
      if (action === 'Follow') {
        this.networkService.postFollowUser(user)
          .subscribe(
            (data: NetworkUserViewModel) => { // _______________________
              // this.getUser(); // obsolete due to event subsciption
              // element.innerText = 'Unfollow';
            },
            (error: ErrorReportViewModel) => {
              console.log(error);
            });
        return;
      } else {
        this.networkService.postUnfollowUser(user)
          .subscribe(
            (data: NetworkUserViewModel) => { // _______________________
              // this.getUser(); // obsolete due to event subsciption
              // element.innerText = 'Follow';
            },
            (error: ErrorReportViewModel) => {
              console.log(error);
            });
        return;
      }
    }

}
