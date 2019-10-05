import { Injectable } from '@angular/core';
import { BooleanResult } from '../infrastructure/booleanResult';
import { DataService } from '../infrastructure/data.service';
import { Observable, of } from 'rxjs';
import { tap, share, map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public authResult = new BooleanResult<Privacy>();
  private adminResult: Observable<boolean>;

  constructor(private service: DataService) { }

  getPrivacy(): Observable<BooleanResult<Privacy>> {
    if (this.authResult.success) {
      return of(this.authResult);
    } else {
      return this.service.get<BooleanResult<Privacy>>('api/auth/privacy')
        .pipe(
          tap(x => this.authResult = x),
          catchError((err, caught) => {
            var result = new BooleanResult<Privacy>();
            result.success = false;
            this.authResult = result;
            return of(result);
          }),
          share(),
        );
    }
  }

  validateAdministrator() {
    if (this.adminResult === undefined) {
      this.adminResult = this.service.getResponse('api/auth/admin')
        .pipe(
          share(),
          map(x => x.ok)
        );
    }
    return this.adminResult;
  }
}

export class Privacy {
  constructor(
    public name: string,
    public alias: string,
    public homeUrl: string,
    public icon: string,
    public unreadCount?: number,
  ) { }
}
