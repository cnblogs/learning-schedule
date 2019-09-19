import { Directive, HostListener } from '@angular/core';
import { Location } from '@angular/common';

@Directive({
  selector: '[appGoback]'
})
export class GobackDirective {

  constructor(private location: Location) { }

  @HostListener('click') onclick() {
    this.location.back();
  }
}
