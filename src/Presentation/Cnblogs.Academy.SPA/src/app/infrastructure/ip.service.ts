import { Observable ,  Observer } from 'rxjs';

export function whereIp(): Observable<string> {
  return Observable.create((ob: Observer<string>) => {
    const http = new XMLHttpRequest();
    http.open('GET', '//ip-api.io/json/');
    http.onload = function () {
      if (http.status === 200) {
        const json = JSON.parse(http.responseText);
        const r = `${json.ip} ${json.country_name} ${json.region_name} ${json.city}`;
        ob.next(r);
      } else {
        ob.next('unknow');
      }
    };
    http.send();
  });
}
