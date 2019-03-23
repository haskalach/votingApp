import { MemberEditComponent } from './../pages/member/member-edit/member-edit.component';
import { Injectable, Component } from '@angular/core';
import { CanDeactivate } from '@angular/router';

@Injectable()
export class PreventUnsavedChanges
  implements CanDeactivate<MemberEditComponent> {
  canDeactivate(component: MemberEditComponent) {
    if (component.userForm.dirty) {
      return confirm('Are you sure you want to continue');
    }
    return true;
  }
}
