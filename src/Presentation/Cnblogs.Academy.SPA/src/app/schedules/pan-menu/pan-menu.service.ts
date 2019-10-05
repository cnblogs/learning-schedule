import { Injectable } from '@angular/core';
import { GlobalStatusService } from 'src/app/shared/global-status.service';

@Injectable({
  providedIn: 'root'
})
export class PanMenuService {
  slideThreshold = 100;
  startDirection = 2; // 2: left; 4 right;
  elements: HTMLElement[];
  constructor(private globalSvc: GlobalStatusService) {
    this.elements = [];
    this.globalSvc.clickRoot$.subscribe(x => {
      this.closeAll();
    });
  }

  panstart(ele: HTMLElement, e: any, ) {
    this.startDirection = e.direction;
    this.elements.push(ele);
    if (this.elements.indexOf(ele) < 0) {
    }
    this.elements.forEach(x => {
      if (x !== ele) {
        this.closeMenu(x);
      }
    });
  }

  panmove(e: any, ele: HTMLElement) {
    const left = this.getLeft(ele);
    if (this.startDirection === 2) {
      if (e.deltaX < 1 && left <= 0) {
        // 左滑
        if (Math.abs(left) >= this.slideThreshold) { return; } // 已经滑到左边了，就不要再滑了
        if (Math.abs(e.deltaX) < this.slideThreshold) {
          ele.style.left = e.deltaX + 'px';
        }
      } else {
        // 右滑 停下 左滑
        // let x = this.slideThreshold / 2 - -e.deltaX;
        // x = x < 0 ? 0 : x;
        // ele.style.left = x + 'px';
      }
    } else if (this.startDirection === 4) {
      if (e.deltaX > 0 && left >= 0) {
        // 右滑
        // if (Math.abs(left) >= this.slideThreshold / 2) { return; } // 已经滑倒最右边了，不要再滑了
        // if (Math.abs(e.deltaX) < this.slideThreshold) {
        //   ele.style.left = e.deltaX + 'px';
        // }
      } else {
        // 左滑 停下 右滑
        let x = e.deltaX - this.slideThreshold;
        if (left > x) { return; } // 左移再右移的轨迹中会出现 left > x 的情况，要禁止。
        x = x > 0 ? 0 : x;
        ele.style.left = x + 'px';
      }
    }

  }

  panend(ele: HTMLElement) {
    let left = this.getLeft(ele);

    if (this.startDirection === 2) {
      if (-left > this.slideThreshold / 2) {
        const id = setInterval(() => {
          if (Math.abs(left) >= this.slideThreshold) {
            clearInterval(id);
          } else {
            left--;
            ele.style.left = left + 'px';
          }
        }, 5);
      } else {
        this.closeMenu(ele);
      }
    } else if (this.startDirection === 4) {
      if (left > this.slideThreshold / 4) {
        const id = setInterval(() => {
          if (Math.abs(left) >= this.slideThreshold / 2) {
            clearInterval(id);
          } else {
            left++;
            ele.style.left = left + 'px';
          }
        });
      } else {
        this.closeMenu(ele);
      }
    }
  }

  closeMenu(ele: HTMLElement) {
    let left = this.getLeft(ele);
    const id = setInterval(() => {
      if (left == 0) {
        clearInterval(id);
      } else {
        left = left > 0 ? left - 1 : left + 1;
        ele.style.left = left + 'px';
      }
    });
  }

  getLeft(ele): number {
    return +ele.style.left.slice(0, -2);
  }

  closeAll() {
    this.elements.forEach(x => {
      this.closeMenu(x);
    })
  }
}
