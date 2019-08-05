import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpResponse, HttpParams } from '@angular/common/http';

const response = 'response';

@Injectable()
export class DataService {

  constructor(
    public http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) { }

  head(url: string) {
    return this.http.head(this.baseUrl + url, { observe: response });
  }

  getText(url: string): Promise<string> {
    return this.http.get(this.baseUrl + url, { responseType: 'text' }).toPromise();
  }

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(this.baseUrl + url);
  }

  getWithParams<T>(url: string, params: HttpParams | {
    [param: string]: string | string[];
  }): Observable<T> {
    return this.http.get<T>(this.baseUrl + url, { params: params })
  }

  getResponse(url: string) {
    return this.http.get(url, { observe: response });
  }

  post<T>(url: string, data: any): Observable<HttpResponse<T>> {
    return this.http.post<T>(this.baseUrl + url, data, { observe: response });
  }

  delete(url: string): Observable<HttpResponse<Object>> {
    return this.http.delete(this.baseUrl + url, { observe: response });
  }

  put(url: string, data: any): Observable<HttpResponse<Object>> {
    return this.http.put(this.baseUrl + url, data, { observe: response });
  }

  patch(url: string, data?: any): Observable<HttpResponse<Object>> {
    return this.http.patch(this.baseUrl + url, data, { observe: response });
  }

  patch2<T>(url: string, data?: any): Observable<T> {
    return this.http.patch<T>(this.baseUrl + url, data);
  }
}
