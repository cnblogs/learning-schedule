import { Directive, AfterViewChecked, ElementRef } from '@angular/core';

@Directive({
  selector: '[appExtenalLinks]'
})
export class ExtenalLinksDirective implements AfterViewChecked {
  host = 'academy.cnblogs.com';

  constructor(private eltRef: ElementRef) { }

  ngAfterViewChecked(): void {
    const arr = this.eltRef.nativeElement.getElementsByTagName('a') as HTMLLinkElement[];
    for (const link of arr) {
      if (link.href.indexOf(this.host) < 0) {
        link.target = '_blank';
      }
    }
  }
}
