import { Component, OnInit } from '@angular/core';
import { BooleanResult } from '../infrastructure/booleanResult';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService, Privacy } from '../services/auth.service';
import { HttpClient } from '@angular/common/http';
import { GlobalStatusService } from '../shared/global-status.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  auth: BooleanResult<Privacy> = null;
  navOpen = false;
  authOpen = false;

  constructor(private service: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private httpClient: HttpClient,
    private globalSvc: GlobalStatusService) {
    this.globalSvc.clickRoot$.subscribe(x => {
      this.closeNav();
      this.closeAuthDropdown();
    })
  }

  ngOnInit() {
    if (typeof window !== 'undefined') {
      this.service.getPrivacy().subscribe(x => {
        this.auth = x;
      });
    }
  }

  isLinkActive(url: string) {
    const queryParamsIndex = this.router.url.indexOf('?');
    const baseUrl = queryParamsIndex === -1 ? this.router.url :
      this.router.url.slice(0, queryParamsIndex);
    return baseUrl === url;
  }

  signin() {
    let returnUrl = this.route.snapshot.queryParamMap.get('returnUrl');
    if (!!!returnUrl) {
      returnUrl = location.href;
    }
    location.href = 'https://account.cnblogs.com/signin?returnUrl=' + returnUrl;
  }

  signout() {
    this.httpClient.post("https://account.cnblogs.com/signout", null, { withCredentials: true })
      .subscribe(x => {
        location.reload(true);
      });
  }

  closeNav() {
    this.navOpen = false;
  }

  closeAuthDropdown() {
    this.authOpen = false;
    this.navOpen = false;
  }

  nobuble(event: Event) {
    event.stopPropagation();
  }
}

