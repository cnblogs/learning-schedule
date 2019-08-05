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
}

export enum TextType {
  PlainText,
  Markdown,
  Html
}

export class ScheduleDetail extends Schedule {
  user: AcademyUser;
  stage: Stage
}

export enum Stage {
  Starting,
  Started,
  Completed
}

export class ScheduleItemDetail extends ScheduleItem {
  subtasks: Subtask[];
  references: Reference[];
  feedbacks: Feedback[];
}

export class Subtask {
  id: number;
  content: string;
  dateAdded: Date;
  dateEnd: Date;
  previousId: number;
}

export class Reference {
  id: number;
  url: string;
  dateAdded: Date;
}

export class CommentItem {
  id: number;
  content: string;
  dateAdded: Date;
}

export class Feedback {
  id: number;
  difficulty: number;
  content: string;

  constructor(public itemId: number) {
  }
}
