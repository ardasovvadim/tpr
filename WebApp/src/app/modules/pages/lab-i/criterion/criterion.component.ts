import {Component, Input, OnInit} from '@angular/core';
import {FormArray, FormBuilder, FormGroup} from '@angular/forms';
import {CriterionModel} from './criterionModel';

@Component({
  selector: 'app-criterion',
  templateUrl: './criterion.component.html',
  styleUrls: ['./criterion.component.sass']
})
export class CriterionComponent implements OnInit {

  @Input() nameCriterion: string;
  amountValue = 0;
  form: FormGroup;
  @Input() criterion: CriterionModel;
  values: FormGroup;
  isGenerated = false;

  constructor(private builder: FormBuilder) {
    this.initForm();
  }

  get valuesControl() {
    return this.form.get('values') as FormArray;
  }

  ngOnInit(): void {
  }

  private initForm() {
    this.form = this.builder.group({
      name: [''],
      amount: [''],
      values: this.builder.array([this.builder.control((''))])
    });
  }

  generateCriterion() {
    this.isGenerated = this.amountValue > 0;
    this.valuesControl.controls = [];
    for (let i = 0; i < this.amountValue; ++i) {
      this.valuesControl.push(this.builder.control(''));
    }
  }

  onChange() {
    if (this.form.invalid) { return; }
    this.criterion.name = this.form.get('name').value;
    this.criterion.criterionValues = [];
    this.criterion.refreshIndexer();
    this.valuesControl.controls.forEach(control => this.criterion.addValue(control.value));
  }
}

