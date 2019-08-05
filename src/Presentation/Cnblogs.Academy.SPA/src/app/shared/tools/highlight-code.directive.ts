import { Directive, ElementRef, AfterViewChecked, OnInit } from '@angular/core';
import * as hljs from 'highlight.js/lib/index.js';

@Directive({
  selector: '[appHighlightCode]'
})
export class HighlightCodeDirective implements OnInit, AfterViewChecked {

  constructor(private eltRef: ElementRef) { }

  ngOnInit(): void {
    hljs.registerLanguage('markup', sp => {
      return sp.getLanguage('html');
    });
  }

  ngAfterViewChecked(): void {
    const arr = this.eltRef.nativeElement.getElementsByTagName('pre');
    for (let index = 0; index < arr.length; index++) {
      const element = arr[index];
      hljs.highlightBlock(element);
    }

    const toc = this.eltRef.nativeElement.getElementsByClassName('mce-toc');
    if (toc[0]) {
      const achors = toc[0].getElementsByTagName('a');
      for (let index = 0; index < achors.length; index++) {
        const element = achors[index];
        element.href = location.origin + location.pathname + element.hash;
      }
    }
  }

}
