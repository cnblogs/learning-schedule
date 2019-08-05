import { Component, OnInit } from '@angular/core';
import { PaginationInstance } from 'ngx-pagination';
import { ActivatedRoute } from '@angular/router';
import { makeStateKey, StateKey } from '@angular/platform-browser';
import { TransferStateService } from '../services/transferState.service';
import { Feed } from '../services/feed';
import { FeedService } from '../services/feed.service';

const FEEDS_HOME = makeStateKey('feeds-home');

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  feeds: Feed[];
  config: PaginationInstance = {
    id: 'home-page',
    totalItems: 300,
    currentPage: 1,
    itemsPerPage: 15,
  };
  stateKey: StateKey<string>;

  constructor(private feedSvc: FeedService,
    private route: ActivatedRoute,
    private stateSvc: TransferStateService) {
    this.stateKey = stateSvc.init(FEEDS_HOME);
  }

  async ngOnInit() {
    this.route.queryParams.subscribe(async x => {
      const p = +x['page'] || 1;
      this.feeds = await this.stateSvc.getOrSetState<Feed[]>(this.stateKey, () => this.feedSvc.list(p, this.config.itemsPerPage));
      this.config.currentPage = p;
    });
  }

}
