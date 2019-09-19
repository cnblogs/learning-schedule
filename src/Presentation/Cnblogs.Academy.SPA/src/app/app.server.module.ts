import { NgModule, ApplicationRef, Optional, Inject } from '@angular/core';
import { ServerModule, ServerTransferStateModule } from '@angular/platform-server';
import { ModuleMapLoaderModule } from '@nguniversal/module-map-ngfactory-loader';

import { AppModule } from './app.module';
import { AppComponent } from './app.component';
import { REQUEST } from '@nguniversal/express-engine/tokens';
import { URL } from 'url';
import { ShortcutComponent } from './shortcut/shortcut.component';

@NgModule({
  imports: [
    AppModule,
    ServerModule,
    ModuleMapLoaderModule,
    ServerTransferStateModule
  ],
  providers: [
    // Add universal-only providers here
    { provide: 'BASE_URL', useValue: 'http://academy_api/' }
  ],
  // bootstrap: [AppComponent],
  entryComponents: [AppComponent, ShortcutComponent]
})
export class AppServerModule {
  constructor(@Optional() @Inject(REQUEST) private request: any) {
  }

  ngDoBootstrap(appRef: ApplicationRef) {
    if (this.request) {
      try {
        const url = new URL(this.request.url);
        if (url.pathname.startsWith('/shortcut')) {
          return appRef.bootstrap(ShortcutComponent, '#app');
        }
      } catch (error) {
        console.error(error);
        return appRef.bootstrap(AppComponent, '#app');
      }
    }
    return appRef.bootstrap(AppComponent, '#app');
  }
}
