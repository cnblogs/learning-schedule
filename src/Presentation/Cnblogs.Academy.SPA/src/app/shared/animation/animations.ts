import {
  animation, animate, style
} from '@angular/animations';

export const slideInAnimation = animation([
  style({
    transform: 'translateX(100%)',
    opacity: 1
  }),
  animate('100ms ease-in')
]);

export const slideOutAnimation = animation([
  animate(100, style({
    transform: 'translateX(100%)',
    opacity: 0
  }))
]);
