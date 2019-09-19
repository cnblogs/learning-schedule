import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ScheduleItem } from '../schedule';
import * as Enumerable from 'linq';
import { ModalService } from '../modal/modal.service';
import { PanMenuService } from '../pan-menu/pan-menu.service';
import { ActivatedRoute, Router } from '@angular/router';
import { PermissionService } from '../permission.service';
import { SwitcherService } from '../switcher.service';

@Component({
  selector: 'app-schedule-items',
  templateUrl: './schedule-items.component.html',
  styleUrls: ['./schedule-items.component.css'],
  providers: [
    ModalService
  ]
})
export class ScheduleItemsComponent implements OnInit {
  @Input() scheduleId: number;
  @Input() completed = false;
  @Input() owner = false;
  @Output() pageChanged = new EventEmitter();
  items: ScheduleItem[];
  pageIndex = 0;
  pageSize = 10;
  totalPage = 1;
  pageArray: number[];
  reanderDetailPannel = false;

  private _source: ScheduleItem[];
  @Input()
  set source(value: ScheduleItem[]) {
    this._source = value;
    this.goToPage(0);
  }
  get source() {
    return this._source;
  }

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
    if (!this.completed) {
      this.goToPage(-1);
    }
  }

  get hasPrev(): boolean {
    return this.pageIndex > 0;
  }

  get hasNext(): boolean {
    return this.pageIndex < this.totalPage - 1;
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
      });
    }
    if (index > -1) {
      this.items = source.orderBy(x => x.id).skip(this.pageIndex * this.pageSize).take(this.pageSize).toArray();
    } else {
      this.items = source.orderByDescending(x => x.id).take(this.pageSize - 1).orderBy(x => x.id).toArray();
    }
    this.pageArray = Array(this.totalPage).fill(null).map((x, i) => i);
  }

  next() {
    if (this.pageIndex < this.totalPage - 1) {
      this.pageIndex++;
      this.goto(this.pageIndex);
      this.goto(this.pageIndex);
    }
  }

  prev() {
    if (this.pageIndex > 0) {
      this.pageIndex--;
      this.goto(this.pageIndex);
    }
  }

  goto(page: number) {
    this.goToPage(page);
    this.pageChanged.emit();
  }

  async delItem(itemId: number) {
    const i = this.source.findIndex(x => x.id === itemId);
    this.source.splice(i, 1);
    if (this.items.length <= 1) {
      this.goto(--this.pageIndex);
    } else {
      this.goto(this.pageIndex);
    }
  }

  async addItem(item: ScheduleItem) {
    this.source.push(item);
    if (this.items.length >= this.pageSize) {
      this.goto(++this.pageIndex);
    } else {
      this.goto(this.pageIndex);
    }
  }
}
