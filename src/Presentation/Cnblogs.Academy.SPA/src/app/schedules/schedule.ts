import { Privacy } from "../services/auth.service";

export class AcademyUser {
  constructor(public alias: string, public icon: string, public userName: string) {
  }

  static CreateUser(privacy: Privacy): AcademyUser {
    return new AcademyUser(privacy.alias, privacy.icon, privacy.name);
  }
}

export class Schedule {
  id = 0;
  title = '';
  items: ScheduleItem[];
  dateEnd: Date;
  dateAdded: Date;
  dateUpdated: Date;
  isPrivate = false;
  parentId: number;
  followingCount: number;
}

export class ScheduleItem {
  id = 0;
  title = '';
  completed: boolean;
  textType: TextType;
  html: string;
  dateAdded: Date;
  dateEnd: Date;
  user: AcademyUser;
  parentId: number;
}

export enum TextType {
  PlainText,
  Markdown,
  Html
}

export class ScheduleDetail extends Schedule {
  user: AcademyUser;
  stage: Stage;
  parentId: number;
  parent: ScheduleIntro;
}

export class ScheduleIntro {
  id: number;
  title: string;
  userName: string;
  alias: string;
}

export enum Stage {
  Starting,
  Started,
  Completed
}

export class ScheduleItemDetail extends ScheduleItem {
}

export class Summary {
  itemId: number;
  note: SummaryNote;
  links: SummaryLink[];
}

export class SummaryNote {
  id: number;
  note: string;
  html: string;
}

export class SummaryLink {
  id: number;
  link: string;
  title: string;
  postId: number;
}

export class Following {
  scheduleId: number;
  user: AcademyUser;
}
