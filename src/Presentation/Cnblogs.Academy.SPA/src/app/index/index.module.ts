import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { IndexComponent } from './index.component';
import { FeedsComponent } from './feeds/feeds.component';
import { FeedItemModule } from '../feed-item/feed-item.module';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    IndexComponent,
    FeedsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', pathMatch: 'full', redirectTo: 'feeds' },
      {
        path: '', component: IndexComponent, children: [
          { path: 'feeds', component: FeedsComponent },
          { path: 'schedules', loadChildren: () => import('../schedules/schedules.module').then(x => x.SchedulesModule) }
        ]
      },
    ]),
    FeedItemModule,
    SharedModule
  ]
})
export class IndexModule { }
