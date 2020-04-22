import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {ClassificationTableIIModel} from './classification-table-i-i.model';
import {MatTable} from '@angular/material/table';

export enum ClassEnum {
  DEFAULT_CLASS,
  FIRST_CLASS,
  SECOND_CLASS
}

@Component({
  selector: 'app-classification-table-i-i',
  templateUrl: './classification-table-i-i.component.html',
  styleUrls: ['./classification-table-i-i.component.sass']
})
export class ClassificationTableIIComponent implements OnInit {

  @Input() classificationTable: ClassificationTableIIModel = null;
  @ViewChild(MatTable) table: MatTable<any>;
  displayedColumns: string[] = [];
  columns: string[] = [];
  status = ClassEnum;

  constructor() {

  }

  generate() {
    this.classificationTable.rows.forEach(r => {
      r.d1 = +r.d1.toFixed(2);
      r.d2 = +r.d2.toFixed(2);
      r.p1 = +r.p1.toFixed(2);
      r.p2 = +r.p2.toFixed(2);
      r.g1 = +r.g1.toFixed(2);
      r.g2 = +r.g2.toFixed(2);
      r.f1 = +r.f1.toFixed(2);
      r.f2 = +r.f2.toFixed(2);
      r.f = +r.f.toFixed(2);
      switch (r.g) {
        case 0:
          r.status = this.status.DEFAULT_CLASS;
          break;
        case 1:
          r.status = this.status.FIRST_CLASS;
          break;
        case 2:
          r.status = this.status.SECOND_CLASS;
          break;
      }
    });
    this.classificationTable.rows[0].alternative.alternativeValues.forEach(val => {
      this.displayedColumns.push(val.key.name);
    });
    this.columns.push(...this.displayedColumns);
    this.columns.push('g', 'd1', 'd2', 'p1', 'p2', 'g1', 'g2', 'f1', 'f2', 'f');
  }

  ngOnInit(): void {
    this.generate();
  }

}
