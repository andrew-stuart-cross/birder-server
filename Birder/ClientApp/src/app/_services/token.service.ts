import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserViewModel } from '../_models/UserViewModel';
import { JwtHelperService } from '@auth0/angular-jwt';
import { AuthenticationService } from './authentication.service';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  constructor(private jwtHelper: JwtHelperService
    , private authenticationService: AuthenticationService) { }

  getFlikrKey(): string {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const tokenDecoded = this.jwtHelper.decodeToken(token);
      return tokenDecoded.FlickrKey;
    } else {
      return null;
    }
  }

  checkIsRecordOwner(username: string): boolean {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      const tokenDecoded = this.jwtHelper.decodeToken(token);
      if (tokenDecoded.unique_name === username) {
        return true;
      } else {
        return false;
      }

      // this.isAuthenticated.next(true);
      // return true;
    } else {
      // TODO: Remove token (could be expired)
      // this.isAuthenticated.next(false);
      return false;
    }
  }

  getAuthenticatedUserDetails(): Observable<UserViewModel> {
    return new Observable<UserViewModel>(user => {
      const token = localStorage.getItem('jwt');
      if (token && !this.jwtHelper.isTokenExpired(token)) {
        const tokenDecoded = this.jwtHelper.decodeToken(token);
        user.next({
          userName: tokenDecoded.unique_name,
          avatar: tokenDecoded.ImageUrl,
          defaultLocationLatitude: parseFloat(tokenDecoded.DefaultLatitude),
          defaultLocationLongitude: parseFloat(tokenDecoded.DefaultLongitude)
        });
      } else {
        this.authenticationService.logout();
      }
      user.complete();
    });
  }
}
