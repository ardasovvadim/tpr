import { Injectable } from '@angular/core';
import {ApiService} from '../../core/services/api.service';
import {Observable, of} from 'rxjs';
import {ClassificationTableIIModel} from './classification-table-i-i/classification-table-i-i.model';
import {catchError} from 'rxjs/operators';

@Injectable()
export class ClassificationService {

  constructor(private api: ApiService) { }

  classify(criteriaParam: string[][], answersParam: number[]): Observable<ClassificationTableIIModel[]> {
    return this.api.post('classify/GetTables', {criteria: criteriaParam, answers: answersParam}).pipe(
      catchError(err => of([]))
    );
  }
}
