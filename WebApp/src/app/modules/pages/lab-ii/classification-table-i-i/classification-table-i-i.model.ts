import {AlternativeModel} from '../../lab-i/alternative.model';
import {ClassEnum} from './classification-table-i-i.component';

export class ClassificationTableIIModel {
  rows: RowModel[];
  iteration: number;
  centerClass1: number[];
  centerClass2: number[];
}

export class RowModel {
  alternative: AlternativeModel;
  g: number;
  d1: number;
  d2: number;
  p1: number;
  p2: number;
  g1: number;
  g2: number;
  f1: number;
  f2: number;
  f: number;
  status: ClassEnum = ClassEnum.DEFAULT_CLASS;
}
