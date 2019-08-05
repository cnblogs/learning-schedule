import { Injectable } from '@angular/core';
import { BooleanResult } from '../infrastructure/booleanResult';
import { DataService } from '../infrastructure/data.service';
import { Observable, of } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public authResult = new BooleanResult<Privacy>();
  constructor(private service: DataService) { }

  getPrivacy(): Observable<BooleanResult<Privacy>> {
    if (this.authResult.success) {
      return of(this.authResult);
    } else {
      return this.service.get<BooleanResult<Privacy>>('api/auth/privacy')
        .pipe(
          tap(x => this.authResult = x)
        );
    }
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
