import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from './services/auth.service';

@Injectable()
export class AdminGuardService implements CanActivateChild {

  constructor(private svc: AuthService, private router: Router, @Inject(PLATFORM_ID) private platformId) { }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : boolean | Observable<boolean> | Promise<boolean> {
    if (this.svc.authResult.success) { return true; }
    return this.svc.validateAdministrator();
  }
}

