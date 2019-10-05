import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';

@Component({
  selector: 'app-pan-menu',
  templateUrl: './pan-menu.component.html',
  styleUrls: ['./pan-menu.component.css']
})
export class PanMenuComponent implements OnInit {
  @Output() edit = new EventEmitter();
  @Output() delete = new EventEmitter();
  @Output() checked = new EventEmitter();

  constructor() {
  }

  ngOnInit() {
  }

  onEdit() {
    this.edit.emit();
  }

  onDelete() {
    this.delete.emit();
  }

  onChecked() {
    this.checked.emit();
  }
}
