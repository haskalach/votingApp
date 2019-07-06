import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';

import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { library } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';
library.add(fas, far);
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TimeAgoPipe } from 'time-ago-pipe';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { ProgressbarModule } from 'ngx-bootstrap/progressbar';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './pages/home/home.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { AuthGuard } from './_guards/auth.guard';
import { MainLayoutComponent } from './_layout/main-layout/main-layout.component';
import { UserService } from './_services/user/user.service';
import { MemberCardComponent } from './pages/member/member-card/member-card.component';
import { MemberDetailedComponent } from './pages/member/member-detailed/member-detailed.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-lis.resolver';
import { MemberEditComponent } from './pages/member/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './pages/member/photo-editor/photo-editor.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/hasRole.directive';
import { PhotoManagementComponent } from './admin/photo-management/photo-management.component';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { AdminService } from './_services/admin.service';
import { RolesModalComponent } from './admin/roles-modal/roles-modal.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RegisterComponent } from './admin/user-management/register/register.component';
import { VoterService } from './_services/voter/voter.service';
import { OrganizationManagementComponent } from './admin/organization-management/organization-management.component';
import { OrganizationService } from './_services/organization/organization.service';
import { CreatOrganizationComponent } from './admin/organization-management/creat-organization/creat-organization.component';
import { UtilitiesService } from './_services/utilities/utilities.service';
import { OrganizationTypeManagementComponent } from './admin/organization-type-management/organization-type-management.component';
import { CreateOrganizationTypeComponent } from './admin/organization-type-management/create-organization-type/create-organization-type.component';
import { HasOrgTypeDirective } from './_directives/hasOrgType.directive';
import { OrgUsersComponent } from './pages/org-users/org-users.component';
import { CreateReferenceUserComponent } from './pages/org-users/create-reference-user/create-reference-user.component';
import { EngenereService } from './_services/engenere/engenere.service';
import { AddEngenereComponent } from './admin/engenere-management/add-engenere/add-engenere.component';
import { EngenereManagementComponent } from './admin/engenere-management/engenere-management.component';
import { UploadEngeneresComponent } from './admin/engenere-management/upload-engeneres/upload-engeneres.component';
import { VoterViewComponent } from './pages/voter/voter-view/voter-view.component';
import { VoterListComponent } from './pages/voter/voter-list/voter-list.component';
import { VoterEditComponent } from './pages/voter/voter-edit/voter-edit.component';
import { ReferenceVoterListComponent } from './pages/voter/reference-voter-list/reference-voter-list.component';
import { MobileVoterListComponent } from './pages/voter/mobile-voter-list/mobile-voter-list.component';
import { MobileVoterViewComponent } from './pages/voter/mobile-voter-view/mobile-voter-view.component';
import { MobileVoterSearchComponent } from './pages/voter/mobile-voter-search/mobile-voter-search.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}
@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MainLayoutComponent,
    MemberCardComponent,
    MemberDetailedComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TimeAgoPipe,
    AdminPanelComponent,
    HasRoleDirective,
    HasOrgTypeDirective,
    PhotoManagementComponent,
    UserManagementComponent,
    RolesModalComponent,
    OrganizationManagementComponent,
    CreatOrganizationComponent,
    OrganizationTypeManagementComponent,
    CreateOrganizationTypeComponent,
    OrgUsersComponent,
    CreateReferenceUserComponent,
    AddEngenereComponent,
    EngenereManagementComponent,
    UploadEngeneresComponent,
    VoterViewComponent,
    VoterListComponent,
    VoterEditComponent,
    ReferenceVoterListComponent,
    MobileVoterListComponent,
    MobileVoterViewComponent,
    MobileVoterSearchComponent
  ],
  imports: [
    ButtonsModule.forRoot(),
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ProgressbarModule.forRoot(),
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FontAwesomeModule,
    FormsModule,
    ReactiveFormsModule,
    NgxGalleryModule,
    FileUploadModule,
    TabsModule.forRoot(),
    BsDropdownModule.forRoot(),
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth/']
      }
    }),
    ModalModule.forRoot(),
    BrowserAnimationsModule
  ],
  providers: [
    AuthService,
    ErrorInterceptorProvider,
    AlertifyService,
    AuthGuard,
    UserService,
    MemberDetailResolver,
    MemberListResolver,
    MemberEditResolver,
    PreventUnsavedChanges,
    AdminService,
    VoterService,
    OrganizationService,
    UtilitiesService,
    EngenereService
  ],
  entryComponents: [RolesModalComponent],
  bootstrap: [AppComponent]
})
export class AppModule {}
