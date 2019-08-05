import { Directive, HostListener, ElementRef, OnDestroy } from '@angular/core';

@Directive({
  selector: '[appCountdown]'
})
export class CountdownDirective implements OnDestroy {
  private interval: any;

  constructor(private el: ElementRef) {
  }

  @HostListener('click') onClick() {
    let count = 5;
    const button = this.el.nativeElement;
    button.disabled = true;
    const text = button.innerText;

    const inter = this.interval = setInterval(function x() {
      button.innerText = `${text} (${count})`;
      count--;
      if (count < 0) {
        clearInterval(inter);
        button.innerText = text;
        button.disabled = false;
      }
      return x;
    }(), 1000);
  }

  ngOnDestroy(): void {
    if (this.interval) {
      clearInterval(this.interval);
    }
  }
}
