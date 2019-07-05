import {
  Directive,
  Input,
  ViewContainerRef,
  TemplateRef,
  OnInit
} from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[appHasOrgType]'
})
export class HasOrgTypeDirective implements OnInit {
  @Input() appHasOrgType: string[];
  isVisible = false;
  constructor(
    private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>,
    private authService: AuthService
  ) {}

  ngOnInit() {
    const userType = this.authService.decodedToken.organizationType as string;
    // console.log({ userType });
    // if no roles clear the containerRef
    if (!userType) {
      this.viewContainerRef.clear();
    }

    // if user has role needed show items
    if (this.authService.TypeMatch(this.appHasOrgType)) {
      if (!this.isVisible) {
        this.isVisible = true;
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      } else {
        this.isVisible = false;
        this.viewContainerRef.clear();
      }
    }
  }
}
