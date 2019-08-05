import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ConstWords } from '../infrastructure/constWords';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {

  constructor(titleService: Title) {
    titleService.setTitle('关于' + ConstWords.titleSuffix);
  }

  ngOnInit() {
  }

}
