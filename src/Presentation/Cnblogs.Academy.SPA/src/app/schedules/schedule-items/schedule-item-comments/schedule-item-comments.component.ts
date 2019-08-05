import { Component, OnInit, Input } from '@angular/core';
import { SchedulesService } from '../../schedules.service';
import { CommentItem } from '../../schedule';

@Component({
  selector: 'app-schedule-item-comments',
  templateUrl: './schedule-item-comments.component.html',
  styleUrls: ['./schedule-item-comments.component.css']
})
export class ScheduleItemCommentsComponent implements OnInit {
  @Input() itemId: number;

  constructor(private svc: SchedulesService) { }

  ngOnInit() {
  }

  async comment(textarea: HTMLTextAreaElement) {
    const id = await this.svc.addComment(textarea.value, this.itemId).toPromise();
    var newComment = new CommentItem();
    newComment.id = id;
    newComment.content = textarea.value;
    newComment.dateAdded = new Date();
    textarea.value = '';
  }
}
