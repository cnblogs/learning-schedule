import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SchedulesService } from 'src/app/schedules/schedules.service';
import { SummaryLink } from 'src/app/schedules/schedule';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-link-edit',
  templateUrl: './link-edit.component.html',
  styleUrls: ['./link-edit.component.scss']
})
export class LinkEditComponent implements OnInit {
  @Input() itemId: number;
  @Input() links: SummaryLink[];
  postLinks: SummaryLink[];
  page = 1;
  size = 5;
  hasPrev = false;
  hasNext = false;
  @Output() close = new EventEmitter();

  constructor(private svc: SchedulesService, private toastr: ToastrService) { }

  ngOnInit() {
    this.fetchPagedPostLinks(this.page, this.size);
  }

  fetchPagedPostLinks(page: number, size: number) {
    this.svc.getRecentPostLinks(page, size).subscribe(x => {
      this.postLinks = x;
      this.hasPrev = this.page > 1;
      this.hasNext = x.length >= this.size;
    })
  }

  prev() {
    if (this.hasPrev) {
      this.page--;
      this.fetchPagedPostLinks(this.page, this.size);
    }
  }

  next() {
    if (this.hasNext) {
      this.page++;
      this.fetchPagedPostLinks(this.page, this.size);
    }
  }

  onChange(e: Event, link: SummaryLink) {
    var checkbox = e.target as HTMLInputElement;
    if (checkbox.checked) {
      this.svc.addSummaryLink(this.itemId, link).subscribe(x => {
        link.id = x.body;
        this.links.push(link);
      }, error => {
        checkbox.checked = false;
      });
    } else {
      this.svc.removeSummaryLink(this.itemId, link.id).subscribe(x => {
        const index = this.links.findIndex(_ => _.id === link.id);
        this.links.splice(index, 1);
      }, error => {
        checkbox.checked = true;
      })
    }
  }

  bind(postLink: SummaryLink) {
    const link = this.links.find(x => x.postId === postLink.postId);
    if (link) {
      postLink.id = link.id;
      return true;
    } else {
      return false;
    }
  }

}
