import { Directive, Input, OnDestroy, TemplateRef, ViewContainerRef, OnInit } from '@angular/core';
import { ModalService } from './modal.service';

@Directive({
  selector: '[appBoard]'
})
export class BoardDirective implements OnInit, OnDestroy {
  @Input('appBoard') id: string;
  show: boolean;

  constructor(
    private svc: ModalService,
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef) { }

  ngOnInit() {
    this.svc.registerBoard(this);
    this.open();
  }

  ngOnDestroy() {
    this.svc.unregisterBoard(this.id);
  }

  close() {
    this.viewContainer.clear();
    this.show = false;
  }

  open() {
    this.viewContainer.createEmbeddedView(this.templateRef);
    this.show = true;
  }

  toggle() {
    if (this.show) {
      this.close();
    } else {
      this.open();
    }
  }

}
