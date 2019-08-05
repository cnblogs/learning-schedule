import { TestBed, inject } from '@angular/core/testing';

import { MarkdownEditorService } from './markdown-editor.service';

describe('MarkdownEditorService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [MarkdownEditorService]
    });
  });

  it('should be created', inject([MarkdownEditorService], (service: MarkdownEditorService) => {
    expect(service).toBeTruthy();
  }));
});
