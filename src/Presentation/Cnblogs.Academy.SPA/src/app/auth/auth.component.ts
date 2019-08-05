import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {
  returnUrl: string;
  constructor(route: ActivatedRoute) {
    this.returnUrl = route.snapshot.queryParams['returnUrl'];
  }

  ngOnInit() {
  }

}
