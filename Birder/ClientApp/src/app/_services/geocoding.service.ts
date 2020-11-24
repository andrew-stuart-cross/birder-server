import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {
  private readonly apiUrl = 'https://maps.google.com/maps/api/geocode/json?';

  constructor(private http: HttpClient) { }

  geocode(searchTerm: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}address=${encodeURIComponent(searchTerm)}&key=${environment.mapKey}`)
      .pipe(
      )
  }

  reverseGeocode(latitude: number, longitude: number): Observable<any> {
    const latLng = latitude + ',' + longitude;
    return this.http.get<any>(`${this.apiUrl}latlng=${encodeURIComponent(latLng)}&key=${environment.mapKey}`)
      .pipe(
      )
  }
}

  // Alternative which returns a Promise
  // geocode(term: string): Promise<any> {
  //   return this.http.get<any>(`https://maps.google.com/maps/api/geocode/json?address=${this.formatSearchTerm(term)}&key=AIzaSyD4IghqI4x7Sld9KP3sP6FtbN7wCPGySmY`)
  //     .toPromise()
  //     .then((response) => Promise.resolve(response))
  //     .catch((error) => Promise.resolve(error.json()));
  // }

  // Called like this...
  // findLocation(): void {
  //   this.geo.getLocation(`Whiteknights campus, uk`)
  //   .then((response) => { console.log(response.results[0]) }) //{ this.result = response.results[0] })
  //   .catch((error) => console.error(error));




  // Old service implementation using @agm/core

//   import { Injectable } from '@angular/core';
// import { MapsAPILoader } from '@agm/core';
// import { Observable } from 'rxjs/Observable';
// import { of } from 'rxjs/observable/of';
// import { tap, map, switchMap } from 'rxjs/operators';
// import { fromPromise } from 'rxjs/observable/fromPromise';
// import { LocationViewModel } from '../_models/LocationViewModel';

// declare var google: any;

// @Injectable()
// export class GeocodeService {
//   private geocoder: any;

//   constructor(private mapLoader: MapsAPILoader) {}

//   private initGeocoder() {
//     console.log('Init geocoder!');
//     this.geocoder = new google.maps.Geocoder();
//   }

//   private waitForMapsToLoad(): Observable<boolean> {
//     if (!this.geocoder) {
//       return fromPromise(this.mapLoader.load())
//       .pipe(
//         tap(() => this.initGeocoder()),
//         map(() => true)
//       );
//     }
//     return of(true);
//   }
//   // reverseGeocode(model: LocationViewModel): Observable<LocationViewModel> {
//   reverseGeocode(latitude: number, longitude: number): Observable<LocationViewModel> {
//     const latlng = { lat: latitude, lng: longitude };

//     // console.log('Start geocoding!');
//     return this.waitForMapsToLoad().pipe(
//       switchMap(() => {
//         return new Observable<LocationViewModel>(observer => {
//           this.geocoder.geocode({ 'location': latlng }, function (results, status) {
//             if (status === google.maps.GeocoderStatus.OK) {
//               // console.log('Geocoding complete!');
//               // console.log(results[0]);
//               observer.next({
//                 latitude: results[0].geometry.location.lat(),
//                 longitude: results[0].geometry.location.lng(),
//                 formattedAddress: results[0].formatted_address
//               });
//             } else {
//               console.log('Error - ', results, ' & Status - ', status);
//               observer.next({ latitude: 0, longitude: 0, formattedAddress: '' });
//             }
//             observer.complete();
//           });
//         });
//       })
//     );
//   }

//   geocodeAddress(location: string): Observable<LocationViewModel> {
//     // console.log('Start geocoding!');
//     return this.waitForMapsToLoad().pipe(
//       switchMap(() => {
//         return new Observable<LocationViewModel>(observer => {
//           this.geocoder.geocode({'address': location}, (results, status) => {
//             if (status === google.maps.GeocoderStatus.OK) {
//               // console.log('Geocoding complete!');
//               observer.next({
//                 latitude: results[0].geometry.location.lat(),
//                 longitude: results[0].geometry.location.lng(),
//                 formattedAddress: results[0].formatted_address
//               });
//             } else {
//                 console.log('Error - ', results, ' & Status - ', status);
//                 observer.next({ latitude: 0, longitude: 0, formattedAddress: '' });
//             }
//             observer.complete();
//           });
//         });
//       })
//     );
//   }
// }