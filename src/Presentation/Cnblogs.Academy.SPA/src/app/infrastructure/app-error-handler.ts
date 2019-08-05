import { Injectable, ErrorHandler } from '@angular/core';

import { swalFailure } from './swalFailure';

@Injectable()
export class AppErrorHandler implements ErrorHandler {

  constructor() { }

  handleError(error: any): void {
    if (error.hasOwnProperty('rejection')) {
      const rejection = error.rejection;
      if (rejection === -1) {
        return;
      }
    }
    if (error === -1) {
      return;
    }
    console.error(error);
    swalFailure('请稍后重试', '客户端发生异常', error);
  }
}
