import { Injectable } from '@angular/core';
import { CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class OwneronlyService implements CanActivateChild {

  constructor(private svc: AuthService, private router: Router) { }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : boolean | Observable<boolean> | Promise<boolean> {
    const alias = state.root.firstChild.firstChild.params['alias'];
    const self = alias === this.svc.authResult.value.alias;
    if (self) {
      return true;
    } else {
      this.router.navigate(['u', alias, 'schedules', 'target', { target: true }]);
    }
  }
}
