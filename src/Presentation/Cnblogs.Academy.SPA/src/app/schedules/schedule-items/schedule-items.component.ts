import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ScheduleItem } from '../schedule';
import * as Enumerable from 'linq';
import { ModalService } from '../modal/modal.service';
import { trigger, transition, query, stagger, animate, style } from '@angular/animations';
import { PanMenuService } from '../pan-menu/pan-menu.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionService } from '../permission.service';
import { SwitcherService } from '../switcher.service';

@Component({
  selector: 'app-schedule-items',
  templateUrl: './schedule-items.component.html',
  styleUrls: ['./schedule-items.component.css'],
  animations: [
    trigger('listAnimation', [
      transition('* => *', [
        query(':leave', [
          stagger(100, [
            animate('0s', style({ opacity: '0' })),
            animate('0.2s', style({ height: '0px', opacity: '0', display: 'none' }))
          ])
        ], { optional: true })
      ])
    ])
  ],
  providers: [
    ModalService
  ]
})
export class ScheduleItemsComponent implements OnInit {
  @Input() scheduleId: number;
  @Input() source: ScheduleItem[];
  @Input() completed = false;
  @Input() owner = false;
  @Output() pageChanged = new EventEmitter();
  items: ScheduleItem[];
  pageIndex = 0;
  pageSize = 100;
  totalPage = 1;
  pageArray: number[];
  reanderDetailPannel = false;

  constructor(
    public modalSvc: ModalService,
    public panSvc: PanMenuService,
    public permissionsvc: PermissionService,
    private route: ActivatedRoute,
    private router: Router,
    private switcherSvc: SwitcherService) {
    this.switcherSvc.detailPannel$.subscribe(x => {
      this.reanderDetailPannel = x;
    });
    this.switcherSvc.itemTitleChanged$.subscribe(x => {
      for (let i = 0; i < this.items.length; i++) {
        const _ = this.items[i];
        if (_.id === x.id) {
          _.title = x.title;
          _.html = x.html;
          _.completed = x.completed;
          _.dateEnd = x.dateEnd;
          return;
        }
      }
    });
    this.switcherSvc.itemDeletedSource$.subscribe(x => {
      this.delItem(x);
    })
  }

  ngOnInit() {
    this.source = this.source || [];
    this.goToPage(this.completed ? 0 : -1);
  }

  get hasPrev(): boolean {
    return this.pageIndex > 0;
  }

  get hasNext(): boolean {
    return this.pageIndex < this.totalPage - 1;
  }

  private scrollIntoView() {
    this.pageChanged.emit();
  }

  goToPage(index = 0) {
    this.totalPage = Math.ceil(this.source.length / this.pageSize);
    if (index > -1) {
      this.pageIndex = index;
    } else {
      this.pageIndex = this.totalPage - 1;
    }

    let source = Enumerable.from(this.source);
    if (!this.completed) {
      source = source.select(x => {
        x.dateAdded = new Date(x.dateAdded);
        return x;
      }).orderBy(x => x.dateAdded);
    }
    this.items = source.skip(this.pageIndex * this.pageSize).take(this.pageSize).toArray();
    this.pageArray = Array(this.totalPage).fill(null).map((x, i) => i);
  }

  next() {
    if (this.pageIndex < this.totalPage - 1) {
      this.pageIndex++;
      this.goToPage(this.pageIndex);
      this.scrollIntoView();
    }
  }

  prev() {
    if (this.pageIndex > 0) {
      this.pageIndex--;
      this.goToPage(this.pageIndex);
      this.scrollIntoView();
    }
  }

  async delItem(itemId: number) {
    this.source = this.source.filter(x => x.id !== itemId);
    if (this.items.length <= 1) {
      this.goToPage(--this.pageIndex);
    } else {
      this.goToPage(this.pageIndex);
    }
  }

  async addItem(item: ScheduleItem) {
    this.source.push(item);
    if (this.items.length >= this.pageSize) {
      this.goToPage(++this.pageIndex);
    } else {
      this.goToPage(this.pageIndex);
    }
  }
}
