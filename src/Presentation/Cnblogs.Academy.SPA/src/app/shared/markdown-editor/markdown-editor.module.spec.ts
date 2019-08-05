import { MarkdownEditorModule } from './markdown-editor.module';

describe('MarkdownEditorModule', () => {
  let markdownEditorModule: MarkdownEditorModule;

  beforeEach(() => {
    markdownEditorModule = new MarkdownEditorModule();
  });

  it('should create an instance', () => {
    expect(markdownEditorModule).toBeTruthy();
  });
});
