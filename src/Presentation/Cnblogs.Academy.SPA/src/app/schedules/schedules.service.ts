import { Injectable } from '@angular/core';
import { DataService } from '../infrastructure/data.service';
import { BooleanResult } from '../infrastructure/booleanResult';
import * as Enumerable from 'linq';
import { Schedule, ScheduleDetail, ScheduleItem, ScheduleItemDetail, Summary, SummaryNote, SummaryLink, Following } from './schedule';
import { KeyValue } from '@angular/common';


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

  listWithItems(alias: string = "", completed = false, page: number = 1, size = 30) {
    return this.svc.get<{ totalCount: number, items: ScheduleDetail[] }>(
      `api/schedules/withItems/?completed=${completed}&page=${page}&size=${size}&alias=${alias}`)
      .toPromise();
  }

  groupByDate(schedules: Schedule[]) {
    const source = Enumerable.from(schedules);
    return source.groupBy(x => new Date(x.dateUpdated).toISOString()).toArray();
  }

  getSchedule(alias: string, scheduleId: number) {
    return this.svc.get<ScheduleDetail>(`api/schedule/${scheduleId}/detail?alias=${alias}`);
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

  addSummaryNote(itemId: number, note: SummaryNote) {
    return this.svc.post<number>(`api/schedule/item/${itemId}/summary/note`, note);
  }

  getSummary(itemId: number) {
    return this.svc.get<Summary>(`api/schedule/item/${itemId}/summary`);
  }

  updateSummaryNote(itemId: number, note: SummaryNote) {
    return this.svc.put(`api/schedule/item/${itemId}/summary/note`, note);
  }

  deleteSummaryNote(itemId: number, noteId: number) {
    return this.svc.delete(`api/schedule/item/${itemId}/summary/note/${noteId}`);
  }

  getRecentPostLinks(page: number, size: number) {
    return this.svc.get<SummaryLink[]>(`api/schedule/summary/post/links/recent/?page=${page}&size=${size}`);
  }

  addSummaryLink(itemId: number, post: SummaryLink) {
    return this.svc.post<number>(`api/schedule/item/${itemId}/summary/links`, post);
  }

  removeSummaryLink(itemId: number, linkId: number) {
    return this.svc.delete(`api/schedule/item/${itemId}/summary/links/${linkId}`);
  }

  subscribe(id: number) {
    return this.svc.post<number>(`api/schedule/${id}/subscription`, null);
  }

  getFollowings(id: number) {
    return this.svc.get<Following[]>(`api/schedule/${id}/following`);
  }

  fetchScheduleOptions(page: number, size: number) {
    return this.svc.get<KeyValue<number, string>[]>(`api/schedule/options?page=${page}&size=${size}`);
  }
}
