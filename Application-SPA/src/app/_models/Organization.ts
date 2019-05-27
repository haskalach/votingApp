import { User } from './user';
import { VoterType } from './VoterType';

export interface Organization {
  id: number;
  name: string;
  users: User[];
  voterTypeId: number;
  voterType: VoterType;
}
