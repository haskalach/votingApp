import { Organization } from './Organization';
import { Photo } from './photo';

export interface User {
  id: number;
  userName: string;
  email: string;
  knowAs: string;
  age: number;
  gender: string;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  city: string;
  country: string;
  interests?: string;
  introduction?: string;
  lookingFor?: string;
  photos?: Photo[];
  roles?: string[];
  organizationId?: number;
  organization: Organization;
}
