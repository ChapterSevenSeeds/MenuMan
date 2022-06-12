namespace MenuMan
{
    public interface IQuestion
    {
        string Key { get; }
        string QuestionText { get; }
        object Ask();
    }
}
