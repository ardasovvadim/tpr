import {Component, ComponentFactoryResolver, ComponentRef, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {CriterionDirective} from './criterion.directive';
import {CriterionComponent} from './criterion/criterion.component';
import {CriterionModel} from './criterion/criterionModel';
import {AlternativeModel, AlternativeStatus} from './alternative.model';
import {AlternativeService} from './alternative.service';
import {MatTable} from '@angular/material/table';
import {SelectionModel} from '@angular/cdk/collections';

@Component({
  selector: 'app-lab-i',
  templateUrl: './lab-i.component.html',
  styleUrls: ['./lab-i.component.sass']
})
export class LabIComponent implements OnInit {

  readonly status = AlternativeStatus;
  amountCriteria = 1;
  criteriaForm: FormGroup;
  @ViewChild(CriterionDirective, {static: true}) criterionContainer: CriterionDirective;
  criterionComponentRefs: ComponentRef<CriterionComponent>[] = [];
  criteria: CriterionModel[];
  alternatives: AlternativeModel[];
  @ViewChild(MatTable) table: MatTable<any>;
  displayedColumns: string[] = [];
  selectedAlternative = new SelectionModel<AlternativeModel>(false, []);

  constructor(private formBuilder: FormBuilder,
              private factory: ComponentFactoryResolver,
              private alternativeService: AlternativeService) {
    this.initCriteriaForm();
  }

  ngOnInit(): void {
    this.selectedAlternative.changed.subscribe(val => this.alternatives.forEach(alt => {
      switch (alt.status) {
        case AlternativeStatus.BETTER:
        case AlternativeStatus.WORSE:
        case AlternativeStatus.NOT_COMPARABLE:
          alt.status = AlternativeStatus.USUAL;
      }
    }));
  }

  generateCriteria() {
    this.criterionContainer.viewContainerRef.clear();
    this.criteria = [];
    const componentFactory = this.factory.resolveComponentFactory(CriterionComponent);
    for (let i = 0; i < this.amountCriteria; ++i) {
      const criterion = new CriterionModel();
      criterion.id = i;
      criterion.name = i.toString();
      const criterionComponent = this.criterionContainer.viewContainerRef.createComponent(componentFactory);
      criterionComponent.instance.criterion = criterion;
      criterionComponent.instance.nameCriterion = criterion.name;
      this.criterionComponentRefs.push(criterionComponent);
      this.criteria.push(criterion);
    }
  }

  private initCriteriaForm() {
    this.criteriaForm = this.formBuilder.group({
      amountCriteria: ['']
    });
  }

  getAll() {
    this.alternatives = [];
    this.displayedColumns = [];
    this.table.ngOnInit();
    this.alternativeService.getAll(this.criteriaToString()).subscribe(data => {
      this.alternatives = data;
      this.alternatives.forEach(alt => alt.status = AlternativeStatus.USUAL);
      this.generateTable();
      this.table.renderRows();
    });
  }

  criteriaToString(): string[][] {
    const criteria = [];
    this.criteria.forEach(crit => {
      const res = [];
      res.push(crit.name);
      crit.criterionValues.forEach(v => res.push(v.name));
      criteria.push(res);
    });
    return criteria;
  }

  private generateTable() {
    const alt = this.alternatives[0];
    alt.alternativeValues.forEach(val => {
      this.displayedColumns.push(val.key.name);
    });
  }

  getBestAndWorth() {
    this.alternativeService.getBestAndWorse(this.alternatives).subscribe(data => {
      this.alternatives.find(alt => alt.id === data[0].id).status = AlternativeStatus.THE_BEST;
      this.alternatives.find(alt => alt.id === data[1].id).status = AlternativeStatus.THE_WORST;
    });
  }

  getBetter() {
    this.alternativeService.getBetter(this.alternatives, this.selectedAlternative.selected[0]).subscribe(data => {
      if (data.length === 0) {
        return;
      }
      data.forEach(d => this.alternatives.find(alt => alt.id === d.id).status = AlternativeStatus.BETTER);
    });
  }

  getWorse() {
    this.alternativeService.getWorse(this.alternatives, this.selectedAlternative.selected[0]).subscribe(data => {
      if (data.length === 0) {
        return;
      }
      data.forEach(d => this.alternatives.find(alt => alt.id === d.id).status = AlternativeStatus.WORSE);
    });
  }

  getNotComparable() {
    this.alternativeService.getNotComparable(this.alternatives, this.selectedAlternative.selected[0]).subscribe(data => {
      if (data.length === 0) {
        return;
      }
      data.forEach(d => this.alternatives.find(alt => alt.id === d.id).status = AlternativeStatus.NOT_COMPARABLE);
    });
  }

  onChangeSelectedAlternative(row: AlternativeModel) {
    row.status = AlternativeStatus.SELECTED;
  }
}

