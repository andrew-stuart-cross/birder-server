import { Component, ViewEncapsulation } from '@angular/core';
import { tap, map, filter, debounceTime, distinct, flatMap, switchMap } from 'rxjs/operators';
import { BehaviorSubject, fromEvent, merge, Observable } from 'rxjs';
import * as _ from 'lodash';
import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';
import { ObservationsFeedService } from '@app/_services/observations-feed.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ObservationFeedFilter } from '@app/_models/FilterFeedOptions';

@Component({
  selector: 'app-infitite-scroll-test',
  templateUrl: './infitite-scroll-test.component.html',
  styleUrls: ['./infitite-scroll-test.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class InfititeScrollTestComponent {

  filterOptions = ObservationFeedFilter;
  private allLoaded = false;
  private cache = [];
  private pageByManual$ = new BehaviorSubject(1);

  private itemHeight = 145;
  private numberOfItems = 10;
  private pageByScroll$ = fromEvent(window, 'scroll')
    .pipe(
      map(() => window.scrollY),
      filter(current => current >= document.body.clientHeight - window.innerHeight),
      debounceTime(200),
      distinct(),
      map(y => Math.ceil((y + window.innerHeight) / (this.itemHeight * this.numberOfItems)))
    );

  private pageByResize$ = fromEvent(window, 'resize')
    .pipe(
      debounceTime(200),
      map(_ => Math.ceil(
        (window.innerHeight + document.body.scrollTop) / (this.itemHeight * this.numberOfItems)
      ))
    );

  private pageToLoad$ = merge(this.pageByManual$, this.pageByScroll$, this.pageByResize$)
    .pipe(
      distinct(),
      filter(page => this.cache[page - 1] === undefined)
    );

  loading = false;


  itemResults$: Observable<ObservationViewModel[]> = this.pageToLoad$
    .pipe(
      tap(_ => this.loading = true),

      flatMap((page: number) => {
        return this.observationsFeedService.getObservationsFeed1(page)
          .pipe(
            tap((resp: ObservationFeedDto) => {
              // this.n = resp.totalItems;

              if (page === Math.ceil(<number>resp.totalItems / <number>this.numberOfItems)) { this.allLoaded = true; }
            },
              (error: ErrorReportViewModel) => {
                // this.router.navigate(['/page-not-found']);
              }),
            map((resp: any) => resp.items), // resp.results),
            tap(resp => {
              this.cache[page - 1] = resp;
              if ((this.itemHeight * this.numberOfItems * page) < window.innerHeight) {
                this.pageByManual$.next(page + 1);
              }
            }),
          );
        // }
      }),
      map(() => _.flatMap(this.cache))
    );


  constructor(private observationsFeedService: ObservationsFeedService) { }

  onFilterFeed(value): void {
    const selectedFilter: ObservationFeedFilter = (<any>ObservationFeedFilter)[value];
    console.log(selectedFilter)
    this.cache = [];
    this.itemResults$ = this.pageToLoad$
      .pipe(
        tap(_ => this.loading = true),
        switchMap((page: number) => {
          return this.observationsFeedService.getObservationsFeed1(page)
            .pipe(
              tap((resp: ObservationFeedDto) => {
                if (page === Math.ceil(<number>resp.totalItems / <number>this.numberOfItems)) { this.allLoaded = true; }
              },
                (error: ErrorReportViewModel) => {
                  // this.router.navigate(['/page-not-found']);
                }),
              map((resp: any) => resp.items),
              tap(resp => {
                this.cache[page - 1] = resp;
                if ((this.itemHeight * this.numberOfItems * page) < window.innerHeight) {
                  this.pageByManual$.next(page + 1);
                }
              }),
            );
        }),
        map(() => _.flatMap(this.cache))
      );
  }
}