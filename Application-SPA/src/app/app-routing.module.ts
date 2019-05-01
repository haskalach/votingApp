import { OrganizationManagementComponent } from './admin/organization-management/organization-management.component';
import { AddVoterComponent } from './admin/voter-management/add-voter/add-voter.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberDetailedComponent } from './pages/member/member-detailed/member-detailed.component';
import { MainLayoutComponent } from './_layout/main-layout/main-layout.component';
import { AuthGuard } from './_guards/auth.guard';
import { HomeComponent } from './pages/home/home.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditComponent } from './pages/member/member-edit/member-edit.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { RegisterComponent } from './admin/user-management/register/register.component';
import { VoterManagementComponent } from './admin/voter-management/voter-management.component';
import { CreatOrganizationComponent } from './admin/organization-management/creat-organization/creat-organization.component';
import { OrganizationTypeManagementComponent } from './admin/organization-type-management/organization-type-management.component';
// tslint:disable-next-line: max-line-length
import { CreateOrganizationTypeComponent } from './admin/organization-type-management/create-organization-type/create-organization-type.component';
import { OrgUsersComponent } from './pages/org-users/org-users.component';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,

    children: [
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      // { path: 'home', component: HomeComponent },
      {
        path: 'home',
        component: HomeComponent
      },
      // user area
      {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
          {
            path: 'members/:id',
            component: MemberDetailedComponent,
            resolve: { user: MemberDetailResolver }
          },
          {
            path: 'member/edit',
            component: MemberEditComponent,
            resolve: { user: MemberEditResolver },
            canDeactivate: [PreventUnsavedChanges]
          },
          {
            path: 'users',
            data: { roles: ['OrganizationAdmin'] },
            component: OrgUsersComponent
          }
        ]
      },
      // admin area
      {
        path: 'admin',
        component: AdminPanelComponent,
        canActivate: [AuthGuard],
        runGuardsAndResolvers: 'always',
        children: [
          {
            path: '',
            data: { roles: ['Admin'] },
            component: UserManagementComponent
          },
          {
            path: 'users',
            data: { roles: ['Admin'] },
            component: UserManagementComponent
          },
          {
            path: 'register',
            data: { roles: ['Admin'] },
            component: RegisterComponent
          },
          {
            path: 'voters',
            data: { roles: ['Admin'] },
            component: VoterManagementComponent
          },
          {
            path: 'add-voter',
            data: { roles: ['Admin'] },
            component: AddVoterComponent
          },
          {
            path: 'organizations',
            data: { roles: ['Admin'] },
            component: OrganizationManagementComponent
          },
          {
            path: 'create-organizations',
            data: { roles: ['Admin'] },
            component: CreatOrganizationComponent
          },
          {
            path: 'organizations-types',
            data: { roles: ['Admin'] },
            component: OrganizationTypeManagementComponent
          },
          {
            path: 'create-organizations-types',
            data: { roles: ['Admin'] },
            component: CreateOrganizationTypeComponent
          }
        ]
      }
    ]
  },

  { path: '**', redirectTo: '', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
