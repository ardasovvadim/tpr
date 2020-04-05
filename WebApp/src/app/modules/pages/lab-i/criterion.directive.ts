import {Directive, ViewContainerRef} from '@angular/core';

@Directive({
  selector: '[appCriterion]'
})
export class CriterionDirective {
  constructor(public viewContainerRef: ViewContainerRef) { }
}
