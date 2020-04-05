export class CriterionModel {
  id: number;
  name: string;
  criterionValues: CriterionValueModel[];
  private indexer = 1;
  constructor() {
  }
  static Create(name: string, ...values: string[]): CriterionModel {
    const crit = new CriterionModel();
    crit.name = name;
    crit.criterionValues = [];
    values.forEach(val => {
      crit.criterionValues.push({name: val, index: crit.indexer});
      ++crit.indexer;
    });
    return crit;
  }
  addValue(n: string): void {
    this.criterionValues.push({name: n, index: this.indexer});
    ++this.indexer;
  }
  refreshIndexer(): void {
    this.indexer = 1;
  }
}

export class CriterionValueModel {
  name: string;
  index: number;
}
