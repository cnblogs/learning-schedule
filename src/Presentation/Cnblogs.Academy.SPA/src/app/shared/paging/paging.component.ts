import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { PaginationInstance } from 'ngx-pagination';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-paging',
  templateUrl: './paging.component.html',
  styleUrls: ['./paging.component.css']
})
export class PagingComponent implements OnInit {
  @Input() config: PaginationInstance;
  @Output() change = new EventEmitter();
  fragment: string;
  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
    this.route.fragment.subscribe(x => {
      this.fragment = x;
    });
  }

  onChange() {
    this.change.emit();
  }

  noBub(e: Event) {
    e.stopPropagation();
  }
}
