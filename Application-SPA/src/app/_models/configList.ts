export interface ConfigList {
  religion: string[];
  politics: string[];
  subChapter: string[];
  civilIdMouhavaza: string[];
  civilIdKadaa: string[];
  civilIdRegion: string[];
  referenceUsers: SelectOnject[];
}
export interface SelectOnject {
  value: number;
  name: string;
}
