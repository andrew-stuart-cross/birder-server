import { Component, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { NetworkUserViewModel } from '@app/_models/UserProfileViewModel';
import { NetworkService } from '@app/_services/network.service';

@Component({
  selector: 'app-followers',
  templateUrl: './followers.component.html',
  styleUrls: ['./followers.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FollowersComponent {
  username: string;
  requesting: boolean;
  followers: NetworkUserViewModel[];

  constructor(private route: ActivatedRoute
    , private networkService: NetworkService) {
    route.params.subscribe(_ => {
      this.route.paramMap.subscribe(pmap => {
        this.username = pmap.get('username');
        this.getFollowers();
      })
    });
  }

  getFollowers(): void {
    this.requesting = true;
    this.networkService.getFollowers(this.username)
      .subscribe(
        (data: NetworkUserViewModel[]) => {
          this.followers = data;
        },
        (error: ErrorReportViewModel) => {
          console.log(error);
        },
        () => this.requesting = false
      );
  }
}