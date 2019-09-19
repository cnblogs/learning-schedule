import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ModalService } from '../modal/modal.service';
import { SwitcherService } from '../switcher.service';
import { PermissionService } from '../permission.service';

@Component({
  selector: 'app-schedule-subnav',
  templateUrl: './schedule-subnav.component.html',
  styleUrls: ['./schedule-subnav.component.css'],
  providers: [SwitcherService, ModalService]
})
export class ScheduleSubnavComponent implements OnInit {
  open = false;
  tooltipOptions = {
    placement: 'bottom',
    'display-mobile': false
  };
  constructor(private route: ActivatedRoute,
    private router: Router,
    public modalSvc: ModalService,
    private switcherSvc: SwitcherService,
    public permissionSvc: PermissionService) {
  }

  ngOnInit() {
  }

  toggle() {
    const path = this.route.snapshot.children[0].routeConfig.path;
    if (path !== 'doing') {
      this.router.navigate(['doing'], { queryParams: { open: true }, relativeTo: this.route });
    } else if (+this.route.snapshot.queryParams['page'] > 1) {
      this.router.navigate(['doing'], { queryParams: { open: true }, relativeTo: this.route });
    } else {
      this.open = !this.open;
      this.switcherSvc.setNewOneModal(this.open);
    }
  }
}

