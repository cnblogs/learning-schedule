import { Injectable, PLATFORM_ID, Inject } from '@angular/core';
import { TransferState, makeStateKey, StateKey } from '@angular/platform-browser';
import { isPlatformServer } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class TransferStateService {

  constructor(private state: TransferState,
    @Inject(PLATFORM_ID) private platformId) {

  }

  init(key: string) {
    return makeStateKey(key);
  }

  async getOrSetState<T>(key: StateKey<string>, callback: any) {
    let result: T;
    if (this.state.hasKey(key)) {
      result = this.state.get<T>(key, null);
      this.state.remove(key);
    } else {
      result = await callback();
      if (isPlatformServer(this.platformId)) {
        this.state.set(key, result);
      }
    }
    return result;
  }
}
