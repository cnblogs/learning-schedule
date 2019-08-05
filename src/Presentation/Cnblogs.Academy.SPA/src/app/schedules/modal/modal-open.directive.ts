import { Directive, Input, HostListener } from '@angular/core';
import { ModalService } from './modal.service';

@Directive({
  // tslint:disable-next-line:directive-selector
  selector: '[modalOpen]'
})
export class ModalOpenDirective {
  @Input('modalOpen') id: string;
  constructor(private svc: ModalService) { }

  @HostListener('click')
  open() {
    this.svc.open(this.id);
  }

}
