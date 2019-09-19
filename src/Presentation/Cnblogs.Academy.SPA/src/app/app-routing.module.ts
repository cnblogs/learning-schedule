import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AboutComponent } from './about/about.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { AuthComponent } from './auth/auth.component';
import { HomeComponent } from './home/home.component';
import { ReleaseComponent } from './about/release/release.component';
import { ShortcutComponent } from './shortcut/shortcut.component';

const routes: Routes = [
  { path: 'shortcut', component: ShortcutComponent, loadChildren: () => import('./shortcut/shortcut/shortcut.module').then(x => x.ShortcutModule) },
  { path: 'about', component: AboutComponent },
  { path: 'release', component: ReleaseComponent },
  { path: 'auth', component: AuthComponent },
  { path: 'u/:alias', loadChildren: () => import('./index/index.module').then(x => x.IndexModule) },
  { path: 'schedules', loadChildren: () => import('./schedules/schedules.module').then(x => x.SchedulesModule) },
  { path: 'course-mall', loadChildren: () => import('./course-mall/course-mall.module').then(x => x.CourseMallModule) },
  { path: 'books', loadChildren: () => import('./books/books.module').then(x => x.BooksModule) },
  { path: 'site-admin/categories', loadChildren: () => import('./categories/categories.module').then(x => x.CategoriesModule) },
  { path: '', pathMatch: 'full', component: HomeComponent },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      scrollPositionRestoration: 'enabled',
      anchorScrolling: 'enabled'
    })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
