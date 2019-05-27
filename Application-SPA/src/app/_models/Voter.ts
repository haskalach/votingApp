import { User } from './user';

export interface Voter {
  id: number;
  codeEngenere: number;
  codePharmacist: number;
  firstNameArabic: string;
  fatherNameArabic: string;
  familyArabic: string;
  firstName: string;
  fatherName: string;
  family: string;
  nationality: string;
  speciality: string;
  subChapter: string;
  birthDate: Date;
  birthCountry: string;
  birthPlace: string;
  civilIdMouhavaza: string;
  civilIdKadaa: string;
  civilIdRegion: string;
  registeryNumber: string;
  civilIdPlace: string;
  registration: Date;
  lastCoveredYear: string;
  graduation: Date;
  school: string;
  graduationCountry: string;
  addressWork: string;
  mobileWork: string;
  phoneWork: string;
  addressHome: string;
  mobileHome: string;
  phoneHome: string;
  email: string;
  religion: string;
  politic: string;
  referenceId: string;
  reference: User;
  voterTypeId: number;
  voterType: VoterType;
}

export interface VoterType {
  id: number;
  name: string;
}
