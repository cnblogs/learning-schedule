import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GlobalStatusService {
  private clickRootSource = new Subject<Event>();
  clickRoot$ = this.clickRootSource.asObservable();
  constructor() { }

  globalClick(event: Event) {
    this.clickRootSource.next(event);
  }
}
