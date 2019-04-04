export interface LifeListViewModel {
    userName: string;
    lifeList: SpeciesSummaryViewModel[];
    // observationsAnalysis: ObservationAnalysisViewModel;
}

export interface SpeciesSummaryViewModel {
    birdId: number;
    englishName: string;
    species: string;
    populationSize: string;
    btoStatusInBritain: string;
    conservationStatus: string;
    count: number;
}
