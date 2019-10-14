namespace RequestResponsePattern
{
    public interface IResponseMessage
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public bool IsSuccess { get; set; }
    }
}
