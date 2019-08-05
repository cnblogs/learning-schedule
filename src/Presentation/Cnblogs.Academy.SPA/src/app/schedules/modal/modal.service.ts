import { Injectable } from '@angular/core';
import { ModalDirective } from './modal.directive';
import { BoardDirective } from './board.directive';

@Injectable()
export class ModalService {

  modals: ModalDirective[];
  boards: BoardDirective[];

  constructor() {
    this.modals = [];
    this.boards = [];
  }

  register(modal: ModalDirective) {
    this.modals.push(modal);
  }

  registerBoard(board: BoardDirective) {
    this.boards.push(board);
  }

  unregister(modal: ModalDirective) {
    this.modals = this.modals.filter(m => m.id !== modal.id);
  }

  unregisterBoard(id: string) {
    this.boards = this.boards.filter(x => x.id !== id);
  }

  open(id: string) {
    const b = this.boards.find(x => x.id === id);
    if (b) {
      b.close();
    }
    const m = this.modals.find(x => x.id === id);
    if (m) {
      if (!m.visible) {
        m.open();
      }
    }
  }

  close(id: string) {
    const m = this.modals.find(x => x.id === id);
    if (m) {
      m.close();
    }
    const b = this.boards.find(x => x.id === id);
    if (b) {
      if (!b.show) {
        b.open();
      }
    }
  }

  toggle(id: string) {
    const m = this.modals.find(x => x.id === id);
    if (m) {
      m.toggle();
    }
    const b = this.boards.find(x => x.id === id);
    if (b) {
      b.toggle();
    }
  }
}
