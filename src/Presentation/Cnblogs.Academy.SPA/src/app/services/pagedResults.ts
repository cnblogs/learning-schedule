export class PagedResults<T> {
  totalCount: number;
  items: T[];

  static Empty<T>() {
    const result = new PagedResults<T>();
    result.totalCount = 0;
    result.items = [];
    return result;
  }
}
