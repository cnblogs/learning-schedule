import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FeedService } from '../../services/feed.service';
import { Feed } from '../../services/feed';
import { PaginationInstance } from 'ngx-pagination';
import { StateKey } from '@angular/platform-browser';
import { PagedResults } from '../../services/pagedResults';
import { TransferStateService } from '../../services/transferState.service';
import { AuthService } from '../../services/auth.service';
import { combineLatest } from 'rxjs';

const INDEX_FEEDS = 'index-feeds';

@Component({
  selector: 'app-feeds',
  templateUrl: './feeds.component.html',
  styleUrls: ['./feeds.component.css']
})
export class FeedsComponent implements OnInit {
  alias: string;
  feeds: Feed[];
  stateKey: StateKey<string>;
  config: PaginationInstance = {
    id: 'index-feeds-page',
    totalItems: 15,
    currentPage: 1,
    itemsPerPage: 15,
  };
  page: number;
  self: boolean;
  myself = false;
  loading = false;

  constructor(private route: ActivatedRoute,
    private svc: FeedService,
    private transferStateSvc: TransferStateService,
    private authSvc: AuthService) {
    this.stateKey = transferStateSvc.init(INDEX_FEEDS);
  }

  ngOnInit() {
    this.loading = true;
    combineLatest(this.route.parent.params, this.route.queryParams, this.authSvc.getPrivacy(), this.route.fragment).subscribe(async x => {
      this.alias = x[0]['alias'];
      this.page = +x[1]['page'] || 1;
      if (x[2].success) {
        this.self = this.alias === x[2].value.alias;
      }
      this.myself = x[3] === 'myself';

      await this.getFeeds(this.myself);
      this.loading = false;
    });
  }

  async getFeeds(myself = false) {
    let result: PagedResults<Feed>;
    if (!!!this.self) {
      result = await this.transferStateSvc.getOrSetState<PagedResults<Feed>>(this.stateKey,
        () => this.svc.getFeeds(this.alias, this.page, this.config.itemsPerPage, true).finally(() => this.loading = false));
    } else {
      result = await this.svc.getFeeds(this.alias, this.page, this.config.itemsPerPage, false, myself).finally(() => this.loading = false);
    }
    this.feeds = result.items;
    this.config.totalItems = result.totalCount;
    this.config.currentPage = this.page;
  }
}
