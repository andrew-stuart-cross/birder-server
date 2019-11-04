import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { HttpEventType } from '@angular/common/http';
import { PhotosService } from '@app/_services/photos.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { Router, ActivatedRoute } from '@angular/router';
import { ObservationService } from '@app/_services/observation.service';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ToastrService } from 'ngx-toastr';
import { Lightbox } from 'ngx-lightbox';
import { PhotographAlbum } from '@app/_models/PhotographAlbum';


@Component({
  selector: 'app-photos-test',
  templateUrl: './photos-test.component.html',
  styleUrls: ['./photos-test.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PhotosTestComponent implements OnInit {
  files: File[] = [];
  fileUploadProgress: string = null;
  observation: ObservationViewModel;
  errorReport: ErrorReportViewModel;

  private _album: Array<PhotographAlbum> = [];
  // images = [1, 2, 3, 4, 5, 6, 7].map(() => `https://picsum.photos/900/500?random&t=${Math.random()}`);

  constructor(private router: Router
    , private route: ActivatedRoute
    , private _lightbox: Lightbox
    , private observationService: ObservationService
    , private photosService: PhotosService
    , private toast: ToastrService) {
  }

  open(index: number): void {
    // open lightbox
    this._lightbox.open(this._album, index);
  }

  close(): void {
    // close lightbox programmatically
    this._lightbox.close();
  }

  ngOnInit() {
    this.getObservation();
  }

  onSelect(event): void {
    console.log(event);
    this.files.push(...event.addedFiles);
  }

  onRemove(event): void {
    console.log(event);
    this.files.splice(this.files.indexOf(event), 1);
  }

  onSavePhotos(): void {
    const formData = new FormData();
    this.files.forEach((file) => { formData.append('files', file); });
    formData.append('observationId', this.observation.observationId.toString());

    this.photosService.postPhotos(formData)
      .subscribe(events => {
        if (events.type === HttpEventType.UploadProgress) {
          this.fileUploadProgress = Math.round(events.loaded / events.total * 100) + '%';
        } else if (events.type === HttpEventType.Response) {
          this.fileUploadProgress = '';
          this.toast.success('Please login again', 'Avatar successfully changed');

          this.files = [];
          // this.router.navigate(['/login'], { queryParams: { returnUrl: '/account-manager-avatar' } });
        }
      },
        (error: ErrorReportViewModel) => {
          this.toast.error(error.friendlyMessage, 'An error occurred');
        }
      );
  }

  getObservation(): void {
    // const id = +this.route.snapshot.paramMap.get('id');
    const id = 72;

    this.observationService.getObservation(id)
      .subscribe(
        (observation: ObservationViewModel) => {
          this.observation = observation;
          this.getPhotos(observation.observationId);

        },
        (error: ErrorReportViewModel) => {
          this.errorReport = error;
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

  onDeletePhoto(filename: string): void {
    console.log(filename);
  }

  getPhotos(id: number): void {
    this.photosService.getPhotos(id)
      .subscribe(
        (result: any) => {
          this._album = result.map((photo): PhotographAlbum => ({ // IProduct specified here ensures we get excess property checks
            src: photo.address,
            caption: '',
            thumb: photo.address,
            filename: photo.filename
          }));
        },
        (error: ErrorReportViewModel) => {
          this.errorReport = error;
          // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
  }

}
