import { User } from './user';
import { OrganizationType } from './OrganizationType';

export interface Organization {
  id: number;
  name: string;
  users: User[];
  organizationTypeId: number;
  organizationType: OrganizationType;
}
