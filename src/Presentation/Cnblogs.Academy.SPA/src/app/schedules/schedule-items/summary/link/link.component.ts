import { Component, OnInit, Input } from '@angular/core';
import { SummaryLink } from 'src/app/schedules/schedule';
import { SchedulesService } from 'src/app/schedules/schedules.service';

@Component({
  selector: 'app-link',
  templateUrl: './link.component.html',
  styleUrls: ['./link.component.scss']
})
export class LinkComponent implements OnInit {
  link_collapse = true;
  link_open = false;
  @Input() itemId: number;
  @Input() links: SummaryLink[];
  @Input() readonly = true;
  constructor(private svc: SchedulesService) { }

  ngOnInit() {
  }

  openBlogAdmin() {
    this.link_open = false;
    window.open('//i.cnblogs.com/EditPosts.aspx?opt=1')
  }

  removeLink(linkId: number) {
    this.svc.removeSummaryLink(this.itemId, linkId).subscribe(x => {
      const index = this.links.findIndex(_ => _.id === linkId);
      this.links.splice(index, 1);
    })
  }

  nopop(event: Event) {
    event.stopPropagation();
  }
}
