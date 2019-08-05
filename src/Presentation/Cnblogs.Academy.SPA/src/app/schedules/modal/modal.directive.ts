import { Directive, Input, OnInit, OnDestroy, TemplateRef, ViewContainerRef } from '@angular/core';
import { ModalService } from './modal.service';

@Directive({
  selector: '[appModal]'
})
export class ModalDirective implements OnInit, OnDestroy {
  @Input('appModal') id: string;
  visible = false;
  constructor(
    private svc: ModalService,
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef) { }

  ngOnInit() {
    this.svc.register(this);
  }

  ngOnDestroy() {
    this.svc.unregister(this);
  }

  open() {
    this.viewContainer.createEmbeddedView(this.templateRef);
    this.visible = true;
  }

  close() {
    this.viewContainer.clear();
    this.visible = false;
  }

  toggle() {
    if (this.visible) {
      this.close();
    } else {
      this.open();
    }
  }
}
