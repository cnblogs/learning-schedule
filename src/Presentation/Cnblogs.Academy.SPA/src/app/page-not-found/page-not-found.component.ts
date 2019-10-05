import { Component, OnInit } from '@angular/core';
import { ServerResponseService } from '../services/server-response';

@Component({
  selector: 'app-page-not-found',
  templateUrl: './page-not-found.component.html',
  styleUrls: ['./page-not-found.component.css'],
})
export class PageNotFoundComponent implements OnInit {

  constructor(private svc: ServerResponseService) {

  }

  ngOnInit() {
    this.svc.setNotFound();
  }

}
