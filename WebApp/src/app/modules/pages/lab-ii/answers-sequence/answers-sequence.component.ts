import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {FormArray, FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-answers-sequence',
  templateUrl: './answers-sequence.component.html',
  styleUrls: ['./answers-sequence.component.sass']
})
export class AnswersSequenceComponent implements OnInit {

  isGenerated = false;
  form: FormGroup;
  answers: number[] = [];
  @Output() answersEvent = new EventEmitter();

  constructor(private builder: FormBuilder) {
    this.initForm();
  }

  get controls() {
    return this.form.controls;
  }

  get answersControl() {
    return this.form.get('answers') as FormArray;
  }

  ngOnInit(): void {
  }

  private initForm() {
    this.form = this.builder.group({
      amount: ['0'],
      answers: this.builder.array([this.builder.control((''))])
    });
  }

  generateAnswers() {
    this.isGenerated = true;
    this.answersControl.controls = [];
    const amount = this.controls.amount.value;
    if (amount <= 0) { return; }
    for (let i = 0; i < amount; ++i) {
      this.answersControl.push(this.builder.control('', [Validators.required]));
    }
  }

  onChange() {
    if (this.form.invalid) { return; }
    this.answers = [];
    this.answersControl.controls.forEach(control => this.answers.push(control.value));
    this.answersEvent.emit(this.answers);
  }
}
