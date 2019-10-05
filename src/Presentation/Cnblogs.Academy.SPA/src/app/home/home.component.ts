import { Component, OnInit } from '@angular/core';
import { PaginationInstance } from 'ngx-pagination';
import { ActivatedRoute } from '@angular/router';
import { makeStateKey, StateKey } from '@angular/platform-browser';
import { TransferStateService } from '../services/transferState.service';
import { Feed } from '../services/feed';
import { FeedService } from '../services/feed.service';
import { finalize } from 'rxjs/operators';

const FEEDS_HOME = makeStateKey('feeds-home');

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  feeds: Feed[];
  loading = false;
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
    this.loading = true;
    this.route.queryParams.subscribe(async x => {
      const p = +x['page'] || 1;
      this.config.currentPage = p;

      this.feeds = await this.stateSvc.getOrSetState<Feed[]>(this.stateKey,
        () => this.feedSvc.list(p, this.config.itemsPerPage).pipe(
          finalize(() => { this.loading = false; })
        ).toPromise());
      this.loading = false;
    })
  }
}
