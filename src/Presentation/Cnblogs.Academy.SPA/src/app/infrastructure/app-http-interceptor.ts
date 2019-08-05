import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';

import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class AppHttpInterceptor implements HttpInterceptor {

  private config = { disableTimeOut: true, closeButton: true };

  constructor(private toastr: ToastrService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(catchError((rep) => {
        if (rep instanceof HttpErrorResponse) {
          switch (rep.status) {
            case 400:
              const problem = rep.error;
              if (problem.detail) {
                this.toastr.error(problem.detail);
              }
              const errors = problem.errors;
              let noDetail = false;
              for (const prop in errors) {
                if (errors.hasOwnProperty(prop)) {
                  const element = errors[prop];
                  if (element instanceof Array) {
                    const arr = element as Array<string>;
                    for (const item of arr) {
                      this.toastr.error(item);
                    }
                  } else if (typeof element === 'string') {
                    this.toastr.error(element);
                  } else {
                    noDetail = true;
                  }
                }
              }
              if (noDetail) {
                this.toastr.error('输入有误');
                noDetail = false;
              }
              break;
            case 403:
              if (typeof rep.error === 'string') {
                this.toastr.error(rep.error, null, this.config);
                break;
              } else {
                return throwError(rep);
              }
            case 401:
              const to = `<a href="//account.cnblogs.com/signin?returnUrl=${location.href}">身份验证失败，点我登录</a>`;
              this.toastr.error(to, null, { ...this.config, enableHtml: true });
              break;
            default:
              return throwError(rep);
          }
          return throwError(-1);
        }
        return throwError(rep);
      }));
  }
}
