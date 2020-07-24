import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { AppComponent } from './app.component';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { TimeAgoPipe } from 'time-ago-pipe';
import { NgModule, Pipe, PipeTransform  } from '@angular/core';

import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { AuthService } from './_services/auth.service';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { MemberListComponent } from './members/member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail-resolver';
import { MemberListResolver } from './_resolvers/member-list-resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { MemberEditResolver } from './_resolvers/member-edit-resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';


export function tokenGet() {
   return localStorage.getItem('token');
}

// tslint:disable-next-line: use-pipe-transform-interface
@Pipe({
   name: 'timeAgo',
   pure: false
})
export class TimeAgoExtendsPipe extends TimeAgoPipe {}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      TimeAgoExtendsPipe,
      PhotoEditorComponent
   ],
   imports: [
      BrowserModule,
      BrowserAnimationsModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot(),
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      NgxGalleryModule,
      FileUploadModule,
      JwtModule.forRoot({
         config: {
            tokenGetter: tokenGet,
            allowedDomains: ['localhost:5000'],
            disallowedRoutes: ['localhost:5000/api/auth']
         }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      MemberDetailResolver,
      MemberListResolver,
      MemberEditResolver,
      PreventUnsavedChanges
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
