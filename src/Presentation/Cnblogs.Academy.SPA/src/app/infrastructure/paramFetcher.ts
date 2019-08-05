import { ActivatedRouteSnapshot } from "@angular/router";

export function getRouterParam(rootRoute: ActivatedRouteSnapshot, name = 'alias'): string {
  if (rootRoute) {
    var alias = rootRoute.params[name];
    if (!!!alias) {
      for (let i = 0; i < rootRoute.children.length; i++) {
        const child = rootRoute.children[i];
        if (child) {
          alias = getRouterParam(child, name);
        }
      }
    }
    return alias;
  } else {
    return '';
  }
}

export function getRouterData(rootRoute: ActivatedRouteSnapshot, name = 'teach'): string {
  if (rootRoute) {
    var data = rootRoute.data[name];
    if (!!!data) {
      for (let i = 0; i < rootRoute.children.length; i++) {
        const child = rootRoute.children[i];
        if (child) {
          data = getRouterData(child, name);
        }
      }
    }
    return data;
  } else {
    return '';
  }
}
