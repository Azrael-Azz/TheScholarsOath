using Microsoft.AspNetCore.Mvc;
using Npgsql;
using QuizGameApi.Models;

namespace QuizGameApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public QuestionsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomQuestions(
        [FromQuery] string subject,
        [FromQuery] string difficulty,
        [FromQuery] string area,
        [FromQuery] int count = 3)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection");
        var questions = new List<QuestionDto>();

        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();

        string sql = @"
            SELECT id, subject, difficulty, area, prompt, option_a, option_b, option_c, option_d, correct_index
            FROM questions
            WHERE (@subject = 'Any' OR subject = @subject)
              AND (@difficulty = 'Any' OR difficulty = @difficulty)
              AND (@area = 'Any' OR area = @area)
            ORDER BY RANDOM()
            LIMIT @count;";

        await using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("subject", subject);
        cmd.Parameters.AddWithValue("difficulty", difficulty);
        cmd.Parameters.AddWithValue("area", area);
        cmd.Parameters.AddWithValue("count", count);

        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            questions.Add(new QuestionDto
            {
                Id = reader.GetInt32(0),
                Subject = reader.GetString(1),
                Difficulty = reader.GetString(2),
                Area = reader.GetString(3),
                Prompt = reader.GetString(4),
                OptionA = reader.GetString(5),
                OptionB = reader.GetString(6),
                OptionC = reader.GetString(7),
                OptionD = reader.GetString(8),
                CorrectIndex = reader.GetInt32(9)
            });
        }

        return Ok(questions);
    }
}