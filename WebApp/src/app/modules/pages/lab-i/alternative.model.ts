import {CriterionModel, CriterionValueModel} from './criterion/criterionModel';

export class Pair<T, T1> {
  key: T;
  value: T1;
}

export class AlternativeModel {
  id: number;
  alternativeValues: Pair<CriterionModel, CriterionValueModel>[];
  status: AlternativeStatus = AlternativeStatus.USUAL;
}

export enum AlternativeStatus {
  USUAL,
  SELECTED,
  THE_BEST,
  THE_WORST,
  BETTER,
  WORSE,
  NOT_COMPARABLE
}
