import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { ScheduleItem } from './schedule';

@Injectable()
export class SwitcherService {
  private newoneModal = new Subject<boolean>();
  private detailPannel = new Subject<boolean>();
  private itemTitleChangedSource = new Subject<ScheduleItem>();
  private itemDeltedSource = new Subject<number>();

  newoneModal$ = this.newoneModal.asObservable();
  detailPannel$ = this.detailPannel.asObservable();
  itemTitleChanged$ = this.itemTitleChangedSource.asObservable();
  itemDeletedSource$ = this.itemDeltedSource.asObservable();

  setNewOneModal(toggle: boolean) {
    this.newoneModal.next(toggle);
  }

  setDetailPannel(toggle: boolean) {
    this.detailPannel.next(toggle);
  }

  changeItemTitleSource(item: ScheduleItem) {
    this.itemTitleChangedSource.next(item);
  }

  changeItemDeletionSource(itemId: number) {
    this.itemDeltedSource.next(itemId);
  }
}
