export class CommentCreate {

  constructor(
      public commentId: number,
      public postId: number,
      public content: string,
      public parentCommentId?: number,

  ) {}

}
