namespace CommentsAPI.Model
{
    public class LinkFlowCommentsDatabaseSettings
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }

        public string CommentsCollectionName { get; set; }

        public string CommentsToCommentsCollectionName { get; set; }
    }
}
