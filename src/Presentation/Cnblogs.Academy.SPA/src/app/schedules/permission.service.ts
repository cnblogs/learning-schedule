import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs/operators';
import { getRouterParam } from '../infrastructure/paramFetcher';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PermissionService {
  public Owner = false;
  constructor(private authSvc: AuthService, private route: ActivatedRoute) {
    const alias = getRouterParam(this.route.snapshot.root, 'alias');
    if (!!!alias) this.Owner = true;
    else {
      this.isCurrentUser(alias).subscribe(x => {
        this.Owner = x;
      })
    }
  }

  /** @deprecated Use Owner instead */
  get isOwner() {
    const alias = getRouterParam(this.route.snapshot.root, 'alias');
    if (!!!alias) return of(true);
    return this.isCurrentUser(alias);
  }

  isCurrentUser(alias: string) {
    return this.authSvc.getPrivacy().pipe(
      map(x => {
        if (x.success) {
          return x.value.alias === alias;
        }
        else {
          return false;
        }
      })
    );
  }
}
