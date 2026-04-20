namespace QuizGameApi.Models;

public class QuestionDto
{
    public int Id { get; set; }
    public string Subject { get; set; } = "";
    public string Difficulty { get; set; } = "";
    public string Area { get; set; } = "";
    public string Prompt { get; set; } = "";
    public string OptionA { get; set; } = "";
    public string OptionB { get; set; } = "";
    public string OptionC { get; set; } = "";
    public string OptionD { get; set; } = "";
    public int CorrectIndex { get; set; }
}