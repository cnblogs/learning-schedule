import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FeedItemComponent } from './feed-item.component';
import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    FeedItemComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    RouterModule
  ],
  exports: [
    FeedItemComponent
  ]
})
export class FeedItemModule { }
