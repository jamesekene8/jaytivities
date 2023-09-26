import { User } from "./user";

export interface IProfile {
  username: string;
  displayName: string;
  image?: string;
  bio?: string;
  photos?: Photo[];
  followersCount: number;
  followingCount: number;
  following: boolean;
}

export class Profile implements IProfile {
  constructor(user: User) {
    this.username = user.username;
    this.displayName = user.displayName;
    this.image = user.image;
  }

  username: string;
  displayName: string;
  image?: string;
  bio?: string;
  photos?: Photo[];
  followersCount: number = 0;
  followingCount: number = 0;
  following: boolean = false;
}

export interface Photo {
  id: string;
  url: string;
  isMain: boolean;
}

export interface UserActivity {
  id: string;
  title: string;
  category: string;
  date: Date;
}
