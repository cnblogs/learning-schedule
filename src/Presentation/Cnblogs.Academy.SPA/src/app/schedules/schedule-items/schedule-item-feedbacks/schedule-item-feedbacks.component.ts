import { Component, OnInit, Input } from '@angular/core';
import { Feedback } from '../../schedule';
import { SchedulesService } from '../../schedules.service';
import { ModalService } from '../../modal/modal.service';

@Component({
  selector: 'app-schedule-item-feedbacks',
  templateUrl: './schedule-item-feedbacks.component.html',
  styleUrls: ['./schedule-item-feedbacks.component.css'],
  providers: [ModalService]
})
export class ScheduleItemFeedbacksComponent implements OnInit {
  @Input() feedbacks: Feedback[];
  @Input() owner: boolean;
  @Input() itemId: number;
  feedback: Feedback;
  constructor(private svc: SchedulesService, public modal: ModalService) { }

  ngOnInit() {
    if (this.feedbacks.length > 0) {
      this.feedback = this.feedbacks[0];
    } else {
      this.feedback = new Feedback(this.itemId);
    }
  }

  enter(e: KeyboardEvent) {
    if (e.code === 'Enter') {
      if (!e.shiftKey) {
        e.preventDefault();
        const textarea = e.target as HTMLTextAreaElement;
        textarea.blur();
      }
    }
  }

  async save() {
    const id = await this.svc.putFeedback(this.feedback).toPromise();
    if (id > 0) {
      this.feedback.id = id;
    }
  }
}
