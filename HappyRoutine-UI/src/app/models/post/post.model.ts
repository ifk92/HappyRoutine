export class Post {

  constructor(
      public postId: number,
      public content: string,
      public username: string,
      public applicationUserId: number,
      public publishDate: Date,
      public updateDate: Date
  ) {}

}
