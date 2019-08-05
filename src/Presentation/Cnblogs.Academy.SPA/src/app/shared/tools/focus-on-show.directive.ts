import { Directive, ElementRef, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[appFocusOnShow]'
})
export class FocusOnShowDirective implements AfterViewInit {
  constructor(private el: ElementRef) { }

  ngAfterViewInit(): void {
    this.el.nativeElement.focus();
  }
}
