import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { AuthService } from './services/auth.service';
import { isPlatformBrowser } from '@angular/common';

@Injectable()
export class AuthGuardService implements CanActivateChild {

  constructor(private svc: AuthService, private router: Router, @Inject(PLATFORM_ID) private platformId) { }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : boolean | Observable<boolean> | Promise<boolean> {
    if (this.svc.authResult.success) { return true; }
    return this.svc.getPrivacy().pipe(
      tap(x => {
        if (!x.success) {
          if (isPlatformBrowser(this.platformId)) {
            this.router.navigate(['auth'], { queryParams: { returnUrl: `${window.location.origin}${state.url}` } });
          }
        }
      }),
      map(x => x.success)
    );
  }
}

