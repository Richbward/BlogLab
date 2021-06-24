export class BlogComment {
  /**
   *
   */
  constructor(
    public blogCommentId: number,
    public blogId: number,
    public content: string,
    public username: string,
    public applicaitonUserId: number,
    public publishDate: Date,
    public updatehDate: Date,
    public parentBlogCommentId?: number
  ) {}

}
