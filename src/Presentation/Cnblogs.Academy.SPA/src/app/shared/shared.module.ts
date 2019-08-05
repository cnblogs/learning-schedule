import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MomentModule } from 'angular2-moment';
import { UnlessModule } from '@isoden/ngx-unless';
import { NgxSlimDropdownModule } from 'ngx-slim-dropdown';
import { PagingComponent } from './paging/paging.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { RouterModule } from '@angular/router';
import { NgxLoadingModule } from 'ngx-loading';
import { TooltipModule } from 'ng2-tooltip-directive';

@NgModule({
  imports: [
    CommonModule,
    MomentModule,
    RouterModule,
    UnlessModule,
    NgxSlimDropdownModule,
    NgxPaginationModule,
    NgxLoadingModule.forRoot({
      backdropBackgroundColour: 'transparent',
      primaryColour: '#85c485',
      secondaryColour: '#85c485',
      tertiaryColour: '#85c485'
    }),
    TooltipModule
  ],
  declarations: [
    PagingComponent,
  ],
  exports: [
    UnlessModule,
    MomentModule,
    NgxSlimDropdownModule,
    PagingComponent,
    NgxPaginationModule,
    NgxLoadingModule,
    TooltipModule
  ]
})
export class SharedModule { }
