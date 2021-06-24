export class BlogCommentViewModel {
  /**
   *
   */
  constructor(
    public parentBlogCommentId: number,
    public blogCommentId: number,
    public blogId: number,
    public content: string,
    public username: string,
    public applicaitonUserId: number,
    public publishDate: Date,
    public updatehDate: Date,
    public isEditable: boolean = false,
    public deleteConfirm: boolean = false,
    public isReplying: boolean = false,
    public comments: BlogCommentViewModel[]
  ) {}

}
