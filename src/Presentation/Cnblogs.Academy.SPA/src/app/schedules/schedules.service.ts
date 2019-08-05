import { Injectable } from '@angular/core';
import { DataService } from '../infrastructure/data.service';
import { BooleanResult } from '../infrastructure/booleanResult';
import * as Enumerable from 'linq';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';
import { Feedback, Schedule, ScheduleDetail, ScheduleItem, ScheduleItemDetail } from './schedule';


@Injectable({
  providedIn: 'root'
})
export class SchedulesService {
  constructor(private svc: DataService) { }

  addSchedule(schedule: Schedule): Promise<BooleanResult> {
    return this.svc.post<BooleanResult>('api/schedule', schedule)
      .toPromise().then(x => x.body);
  }

  getRawSchedules(stage: string, page = 1, size = 10) {
    return this.svc.get<ScheduleDetail[]>(`api/schedules/?stage=${stage}&page=${page}&size=${size}`).toPromise();
  }

  list(completed = false, page = 1, size = 10) {
    return this.svc.getResponse(`api/schedule/mine?completed=${completed}&page=${page}&pageSize=${size}`)
      .toPromise()
      .then(x => {
        const count = x.headers.get('x-count');
        return { count: count, list: x.body as Schedule[] };
      });
  }

  addMarkdownItem(id: number, item: string) {
    return this.svc.post<number>(`api/schedule/${id}/items/[markdown]`, { title: item })
      .toPromise().then(x => x.body);
  }

  todoItem(scheduleId: number, item: ScheduleItem) {
    return this.svc.post<BooleanResult>(`api/schedule/${scheduleId}/items/${item.id}/todo`, item).toPromise().then(x => x.body);
  }

  delRecord(scheduleId: number, itemId: number) {
    return this.svc.delete(`api/schedule/${scheduleId}/items/${itemId}/record`)
      .toPromise().then(x => x.body as BooleanResult);
  }

  complete(scheduleId: number) {
    return this.svc.patch2<BooleanResult>(`api/schedule/${scheduleId}/dateEnd`)
      .toPromise();
  }

  cancelComplete(scheduleId: number) {
    return this.svc.delete(`api/schedule/${scheduleId}/dateEnd`)
      .toPromise()
      .then(x => x.ok);
  }

  updateSchedule(id: number, title: string, isPrivate: boolean) {
    return this.svc.patch(`api/schedule/${id}/`, { title: title, isPrivate: isPrivate })
      .toPromise()
      .then(rep => rep.ok);
  }

  deleteSchedule(id: number) {
    return this.svc.delete(`api/schedule/${id}`)
      .toPromise()
      .then(rep => rep.ok);
  }

  updateItem(id: number, title: string) {
    return this.svc.patch(`api/schedule/item/${id}/title`, { title: title })
      .toPromise()
      .then(x => x.ok);
  }

  updateRecord(itemId: number, content: string) {
    return this.svc.patch(`api/schedule/item/${itemId}/content`, { content: content })
      .toPromise()
      .then(x => x.ok);
  }

  delItem(itemId: number) {
    return this.svc.delete(`api/schedule/item/${itemId}`)
      .toPromise()
      .then(x => x.ok);
  }

  listWithItems(alias: string = "", completed = false, teachOnly = false, page: number = 1, size = 30) {
    return this.svc.get<{ totalCount: number, items: ScheduleDetail[] }>(
      `api/schedules/withItems/?completed=${completed}&page=${page}&size=${size}&alias=${alias}&teachOnly=${teachOnly}`)
      .toPromise();
  }

  groupByDate(schedules: Schedule[]) {
    const source = Enumerable.from(schedules);
    return source.groupBy(x => new Date(x.dateUpdated).toISOString()).toArray();
  }

  getSchedule(alias: string, scheduleId: number) {
    return this.svc.get<ScheduleDetail>(`api/schedule/${scheduleId}/detail?alias=${alias}`)
      .toPromise();
  }

  getLastUpdatePrivate(scheduleId: number) {
    return this.svc.get<Date>(`api/schedule/${scheduleId}/private/record`).toPromise();
  }

  updatePrivate(scheduleId: number, isPrivate: boolean) {
    return this.svc.put(`api/schedule/${scheduleId}/private?to=${isPrivate}`, null);
  }

  getScheduleItemDetail(itemId: number) {
    return this.svc.get<ScheduleItemDetail>(`api/schedule/item/${itemId}/detail`);
  }

  addSubtask(content: string, itemId: number) {
    return this.svc.post<number>(`api/schedule/item/${itemId}/subtasks`, { content: content }).pipe(
      map(x => x.body)
    );
  }

  addRef(url: string, itemId: number) {
    return this.svc.post<number>(`api/schedule/item/${itemId}/references`, { url: url }).pipe(
      map(x => x.body)
    );
  }

  addComment(content: string, itemId: number) {
    return this.svc.post<number>(`api/schedule/item/${itemId}/comments`, { content: content }).pipe(
      map(x => x.body)
    );
  }

  accomplishSubtask(itemId: number, taskId: number, completed = true) {
    return this.svc.put(`api/schedule/item/${itemId}/subtasks/${taskId}?completed=${completed}`, null).pipe(
      map(x => x.ok),
      catchError(() => of(false))
    );
  }

  updateSubtask(itemId: number, taskId: number, content: string) {
    return this.svc.put(`api/schedule/item/${itemId}/subtasks/${taskId}/content`, { content: content }).pipe(
      map(x => x.ok),
      catchError(() => of(false))
    );
  }

  deleteSubtask(subtaskId: number) {
    return this.svc.delete(`api/schedule/item/subtasks/${subtaskId}`).pipe(
      map(x => x.ok),
      catchError(() => of(false))
    );
  }

  deleteReference(refId: number) {
    return this.svc.delete(`api/schedule/item/references/${refId}`).pipe(
      map(x => x.ok),
      catchError(() => of(false))
    );
  }

  updateReference(itemId: number, refId: number, url: string) {
    return this.svc.put(`api/schedule/item/${itemId}/references/${refId}/url`, { url: url }).pipe(
      map(x => x.ok),
      catchError(() => of(false))
    )
  }

  putFeedback(feedback: Feedback) {
    return this.svc.put(`api/schedule/item/feedback/`, feedback).pipe(
      map(x => x.body as number),
      catchError(() => of(0))
    )
  }
}
