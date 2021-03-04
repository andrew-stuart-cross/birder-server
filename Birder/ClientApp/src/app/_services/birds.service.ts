import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BirdSummaryViewModel, BirdsDto } from '@app/_models/BirdSummaryViewModel';
import { HttpParams, HttpClient } from '@angular/common/http';
import { BirdDetailViewModel } from '@app/_models/BirdDetailViewModel';

@Injectable({
  providedIn: 'root'
})
export class BirdsService {

  constructor(private http: HttpClient) { }

  getBirds(pageIndex: number, pageSize: number, speciesFilter: string): Observable<BirdsDto> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString())
      .set('speciesFilter', speciesFilter.toString());

    return this.http.get<BirdsDto>('api/Birds', { params })
      .pipe();
  }

  getBirdsDdl(): Observable<BirdSummaryViewModel[]> {
    return this.http.get<BirdSummaryViewModel[]>('api/Birds/BirdsList')
      .pipe();
  }

  getBird(id: string): Observable<BirdDetailViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } : {};

    return this.http.get<BirdDetailViewModel>('api/Birds/GetBird', options)
      .pipe();
  }
}
