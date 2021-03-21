import { UserViewModel } from './UserViewModel';
import { BirdSummaryViewModel } from './BirdSummaryViewModel';
import { ObservationPosition } from './ObservationPosition';
import { ObservationNote } from './ObservationNote';

export interface ObservationViewModel {
   [x: string]: any;
    observationId: number;
    quantity: number;
    observationDateTime: Date | string;
    creationDate: Date | string;
    lastUpdateDate: Date | string;
    birdId: number;
    bird: BirdSummaryViewModel;
    user: UserViewModel;
    position: ObservationPosition;
    notes: ObservationNote[];
}
