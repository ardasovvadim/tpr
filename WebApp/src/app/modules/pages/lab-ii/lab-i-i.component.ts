import {Component, ComponentFactoryResolver, ComponentRef, OnInit, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {CriterionComponent} from '../lab-i/criterion/criterion.component';
import {CriterionModel} from '../lab-i/criterion/criterionModel';
import {CriterionDirective} from '../lab-i/criterion.directive';
import {AlternativeModel, AlternativeStatus} from '../lab-i/alternative.model';
import {MatTable} from '@angular/material/table';
import {SelectionModel} from '@angular/cdk/collections';
import {AlternativeService} from '../lab-i/alternative.service';
import {ClassificationTableIIModel} from './classification-table-i-i/classification-table-i-i.model';
import {ClassificationService} from './classification.service';

@Component({
  selector: 'app-lab-ii',
  templateUrl: './lab-i-i.component.html',
  styleUrls: ['./lab-i-i.component.sass']
})
export class LabIIComponent implements OnInit {

  readonly status = AlternativeStatus;
  amountCriteria = 1;
  criteriaForm: FormGroup;
  @ViewChild(CriterionDirective, {static: true}) criterionContainer: CriterionDirective;
  criterionComponentRefs: ComponentRef<CriterionComponent>[] = [];
  // TODO: remove initialize
  criteria: CriterionModel[] = [
    CriterionModel.Create('Ціна', 'Низька', 'Висока'),
    CriterionModel.Create('Якість виготовлення', 'Висока', 'Низька'),
    CriterionModel.Create('Час виготовлення', 'Швидко', 'Довго'),
    CriterionModel.Create('Складність', 'Не складно', 'Складно'),
    CriterionModel.Create('Зацікавленність', 'Музика', 'Наука', 'Загальне')
  ];
  alternatives: AlternativeModel[];
  @ViewChild(MatTable) table: MatTable<any>;
  displayedColumns: string[] = [];
  selectedAlternative = new SelectionModel<AlternativeModel>(false, []);
  // TODO: remove initialize
  answers: number[] = [1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2];
  classifyTables: ClassificationTableIIModel[] = null;

  constructor(private formBuilder: FormBuilder,
              private factory: ComponentFactoryResolver,
              private alternativeService: AlternativeService,
              private classService: ClassificationService) {
    this.initCriteriaForm();
  }

  ngOnInit(): void {
    this.selectedAlternative.changed.subscribe(val => this.alternatives.forEach(alt => {
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

  getAnswers(answers: number[]) {
    this.answers = answers;
  }

  classify() {
    const criteria = this.criteriaToString();
    const answers = this.answers;
    this.classService.classify(criteria, answers).subscribe(data => {
      this.classifyTables = data;
    });
  }
}
