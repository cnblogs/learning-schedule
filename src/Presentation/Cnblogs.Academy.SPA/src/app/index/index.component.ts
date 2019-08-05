import { Component, OnInit } from '@angular/core';
import { FeedService } from '../services/feed.service';
import { ActivatedRoute } from '@angular/router';
import { User } from '../services/feed';
import { StateKey } from '@angular/platform-browser';
import { TransferStateService } from '../services/transferState.service';

const INDEX = 'index-home';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  alias: string;
  user: User;
  stateKey: StateKey<string>;


  constructor(private svc: FeedService,
    private route: ActivatedRoute,
    private stateSvc: TransferStateService) {
    this.stateKey = stateSvc.init(INDEX);
  }

  ngOnInit() {
    this.route.params.subscribe(async p => {
      this.alias = p['alias'];
      this.user = await this.stateSvc.getOrSetState<User>(this.stateKey, () => this.svc.getUser(this.alias));
    });
  }

}
