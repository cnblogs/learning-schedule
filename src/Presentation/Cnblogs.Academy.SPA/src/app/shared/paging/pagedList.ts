export class PagedList<T> {
  constructor(public totalCount: number, public items: Array<T> = []) {
  }
}
