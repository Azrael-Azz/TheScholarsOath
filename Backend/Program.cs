using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// You can leave this commented during local testing
// app.UseHttpsRedirection();

app.MapGet("/api/questions/random", async (
    string subject,
    string difficulty,
    string area,
    int count,
    IConfiguration config) =>
{
    var connectionString = config.GetConnectionString("DefaultConnection");

    var questions = new List<object>();

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
        questions.Add(new
        {
            id = reader.GetInt32(0),
            subject = reader.GetString(1),
            difficulty = reader.GetString(2),
            area = reader.GetString(3),
            prompt = reader.GetString(4),
            optionA = reader.GetString(5),
            optionB = reader.GetString(6),
            optionC = reader.GetString(7),
            optionD = reader.GetString(8),
            correctIndex = reader.GetInt32(9)
        });
    }

    return Results.Ok(questions);
});

app.Run();