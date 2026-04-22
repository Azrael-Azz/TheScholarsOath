using System;

[Serializable]
public class ApiQuestion
{
    public int id;
    public string subject;
    public string difficulty;
    public string area;
    public string prompt;
    public string optionA;
    public string optionB;
    public string optionC;
    public string optionD;
    public int correctIndex;
}