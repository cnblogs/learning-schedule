export class Feed {
  icon: string;
  userName: string;
  dateAdded: Date;
  feedTitle: string;
  link: string;
  feedType: FeedType;
  alias: string;
  isPrivate: boolean;
}

export enum FeedType {
  ScheduleNew = 20,

  ScheduleItemNew = 21,

  ScheduleItemDone = 22,

  ScheduleCompleted = 23
}

export class User {
  name: string;
  avatar: string;
}

export function subPath(link: string) {
  let fullLink = link;
  if (link.startsWith('//')) {
    fullLink = 'http:' + link;
  }
  try {
    const url = new URL(fullLink);
    return url.pathname;
  } catch (error) {
    return link;
  }
}
