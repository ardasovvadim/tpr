import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {AlternativeModel} from './alternative.model';
import {ApiService} from '../../core/services/api.service';
import {catchError} from 'rxjs/operators';

@Injectable()
export class AlternativeService {
  constructor(private api: ApiService) {
  }

  getAll(criteria: string[][]): Observable<AlternativeModel[]> {
    return this.api.post('alternative/', criteria).pipe(catchError(err => []));
  }

  getBestAndWorse(alternatives: AlternativeModel[]): Observable<AlternativeModel[]> {
    return this.api.post('alternative/GetBestAndWorst', alternatives).pipe(
      catchError(err => [])
    );
  }

  getBetter(alternativesParam: AlternativeModel[], findAlternativeParam: AlternativeModel): Observable<AlternativeModel[]> {
    return this.api.post('alternative/GetBetter', {alternatives: alternativesParam, findAlternative: findAlternativeParam}).pipe(
      catchError(err => [])
    );
  }

  getWorse(alternativesParam: AlternativeModel[], findAlternativeParam: AlternativeModel): Observable<AlternativeModel[]> {
    return this.api.post('alternative/GetWorse', {alternatives: alternativesParam, findAlternative: findAlternativeParam}).pipe(
      catchError(err => [])
    );
  }

  getNotComparable(alternativesParam: AlternativeModel[], findAlternativeParam: AlternativeModel): Observable<AlternativeModel[]> {
    return this.api.post('alternative/NotComparable', {alternatives: alternativesParam, findAlternative: findAlternativeParam}).pipe(
      catchError(err => [])
    );
  }
}
