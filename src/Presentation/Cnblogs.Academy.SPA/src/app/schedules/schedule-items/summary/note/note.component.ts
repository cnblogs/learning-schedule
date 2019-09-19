import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { SummaryNote } from 'src/app/schedules/schedule';
import { SchedulesService } from 'src/app/schedules/schedules.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.scss']
})
export class NoteComponent implements OnInit {
  @Input() itemId: number;
  @Input() note: SummaryNote;
  @Input() readonly = true;
  note_edit = false;

  constructor(private svc: SchedulesService) { }

  ngOnInit() {
  }

  save(note: SummaryNote) {
    this.note = note;
    this.note_edit = false;
  }

  async delete() {
    var will = await Swal.fire({
      title: `确定要删除这篇心得体会吗？`, text: '删除后不可恢复',
      cancelButtonText: '取消',
      confirmButtonText: '删除',
      type: 'warning',
      showCancelButton: true,
      focusCancel: true
    });
    if (!will.value) return;

    if (this.note && this.note.id > 0) {
      this.svc.deleteSummaryNote(this.itemId, this.note.id).subscribe(x => {
        this.note = null;
      });
    }
  }

  close() {
    this.note_edit = false;
  }

  noPop(e: Event) {
    e.stopPropagation();
  }

}
