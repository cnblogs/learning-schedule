import { Injectable } from '@angular/core';
import { DataService } from '../infrastructure/data.service';
import { Feed, User } from './feed';
import { PagedResults } from './pagedResults';

@Injectable({
  providedIn: 'root'
})
export class FeedService {

  constructor(private svc: DataService) { }

  list(page = 1, size = 10) {
    return this.svc.get<Feed[]>(`api/feeds?page=${page}&size=${size}`);
  }

  getFeeds(alias: string, page = 1, size = 10, guest = false, myself = false) {
    return this.svc.getWithParams<PagedResults<Feed>>(`api/feeds/${alias}?page=${page}&size=${size}`,
      { guest: guest.toString(), myself: myself.toString() }).toPromise();
  }

  getUser(alias: string) {
    return this.svc.get<User>(`api/feeds/user/${alias}`).toPromise();
  }
}
