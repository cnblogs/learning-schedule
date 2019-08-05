import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs/operators';
import { getRouterParam, getRouterData } from '../infrastructure/paramFetcher';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {

  constructor(private authSvc: AuthService, private route: ActivatedRoute) { }

  get isOwner() {
    const alias = getRouterParam(this.route.snapshot.root, 'alias');
    if (!!!alias) return of(true);
    return this.isCurrentUser(alias);
  }

  isCurrentUser(alias: string) {
    return this.authSvc.getPrivacy().pipe(map(x => {
      if (x.success) {
        return x.value.alias === alias;
      }
      else {
        return false;
      }
    }));
  }

  get inTeach() {
    return !!getRouterData(this.route.snapshot.root, 'teach');
  }
}
