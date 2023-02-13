export class IComment {

  constructor(
    public commentId: number,
    public postId: number,
    public content: string,
    public username: string,
    public applicationUserId: number,
    public publishDate: Date,
    public updateDate: Date,
    public parentCommentId?: number,

  ) {}

}
