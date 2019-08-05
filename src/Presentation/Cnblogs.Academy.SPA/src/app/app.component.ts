import { Component } from '@angular/core';
import { GlobalStatusService } from './shared/global-status.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(private globalSvc: GlobalStatusService) {
  }

  onTap(event: Event) {
    this.globalSvc.globalClick(event);
  }
}
