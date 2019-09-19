import { BrowserModule, BrowserTransferStateModule, Title, HAMMER_GESTURE_CONFIG, HammerGestureConfig } from '@angular/platform-browser';
import { NgModule, ErrorHandler, ApplicationRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { DataService } from './infrastructure/data.service';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { AuthGuardService } from './auth-guard.service';
import { AppErrorHandler } from './infrastructure/app-error-handler';
import { AppHttpInterceptor } from './infrastructure/app-http-interceptor';
import { AboutComponent } from './about/about.component';
import { ToolsModule } from './shared/tools/tools.module';
import { LoadingBarHttpClientModule } from '@ngx-loading-bar/http-client';
import { SharedModule } from './shared/shared.module';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AuthComponent } from './auth/auth.component';
import { HomeComponent } from './home/home.component';
import { FeedItemModule } from './feed-item/feed-item.module';
import { ReleaseComponent } from './about/release/release.component';
import { ServerResponseService } from './services/server-response';
import { AdminGuardService } from './admin-guard.service';
import { ShortcutComponent } from './shortcut/shortcut.component';

export class MyHammerConfig extends HammerGestureConfig {
  overrides = <any>{
    'pinch': { enable: false },
    'rotate': { enable: false }
  };
  options = {
    cssProps: {
      userSelect: 'auto'
    }
  };
}

@NgModule({
  declarations: [
    AppComponent, ShortcutComponent,
    NavMenuComponent,
    AboutComponent,
    PageNotFoundComponent,
    AuthComponent,
    HomeComponent,
    ReleaseComponent,
    ShortcutComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserTransferStateModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(),
    ToolsModule,
    LoadingBarHttpClientModule,
    SharedModule,
    FeedItemModule
  ],
  providers: [
    { provide: ErrorHandler, useClass: AppErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: AppHttpInterceptor, multi: true },
    DataService,
    AuthGuardService,
    AdminGuardService,
    Title,
    {
      provide: HAMMER_GESTURE_CONFIG,
      useClass: MyHammerConfig
    },
    ServerResponseService
  ],
  // bootstrap: [AppComponent],
  entryComponents: [AppComponent, ShortcutComponent]
})
export class AppModule {
  ngDoBootstrap(appRef: ApplicationRef) {
    try {
      if (location) {
        if (location.pathname.startsWith('/shortcut')) {
          return appRef.bootstrap(ShortcutComponent, "#app");
        }
      }
    } catch (error) {
      console.error(error);
      return appRef.bootstrap(AppComponent, '#app');
    }
    return appRef.bootstrap(AppComponent, '#app');
  }
}

