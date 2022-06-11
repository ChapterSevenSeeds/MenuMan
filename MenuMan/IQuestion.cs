namespace MenuMan
{
    public interface IQuestion
    {
        string Key { get; set; }
        string QuestionText { get; set; }
        object Ask();
    }
}
