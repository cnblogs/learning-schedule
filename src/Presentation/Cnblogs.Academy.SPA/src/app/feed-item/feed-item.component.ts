import { Component, OnInit, Input } from '@angular/core';
import { Feed, subPath, FeedType } from '../services/feed';

@Component({
  selector: 'app-feed-item',
  templateUrl: './feed-item.component.html',
  styleUrls: ['./feed-item.component.css']
})
export class FeedItemComponent implements OnInit {
  @Input() feed: Feed;
  constructor() { }

  ngOnInit() {
  }

  subPath(link: string) {
    return subPath(link);
  }

  isScheduleType(feedType: FeedType) {
    return feedType === FeedType.ScheduleNew || feedType === FeedType.ScheduleCompleted;
  }

  isCompletedType(feedType: FeedType) {
    return feedType === FeedType.ScheduleCompleted || feedType === FeedType.ScheduleItemDone;
  }

}
