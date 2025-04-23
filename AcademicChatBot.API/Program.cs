using AcademicChatBot.DAL.Contract;
using System.Text;
using AcademicChatBot.DAL.DBContext;
using AcademicChatBot.DAL.Implementation;
using AcademicChatBot.Service.Contract;
using AcademicChatBot.Service.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using AcademicChatBot.Service.HubService;
using AcademicChatBot.Common.BussinessModel.Accounts;
using AcademicChatBot.Common.MLModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
//Initial Model AI
builder.Services.AddSingleton<IMLModel, MLModel>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProgramService, ProgramService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IComboSubjectService, ComboSubjectService>();
builder.Services.AddScoped<IToolForSubjectService, ToolForSubjectService>();
builder.Services.AddScoped<IClassificationService, ClassificationService>();
builder.Services.AddScoped<IPOMappingPLOService, POMappingPLOService>();
builder.Services.AddHttpClient<IGeminiAPIService, GeminiAPIService>();
builder.Services.AddScoped<IIntentDetectorService, IntentDetectorService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IAIChatLogService, AIChatLogService>();
builder.Services.AddScoped<IHubService, HubService>();
builder.Services.AddScoped<ICurriculumService, CurriculumService>();
builder.Services.AddScoped<IToolService, ToolService>();
builder.Services.AddScoped<IProgramingOutcomeService, ProgramingOutcomeService>();
builder.Services.AddScoped<IProgramingLearningOutcomeService, ProgramingLearningOutcomeService>();
builder.Services.AddScoped<IMajorService, MajorService>();
builder.Services.AddScoped<ICourseLearningOutcomeService, CourseLearningOutcomeService>();
builder.Services.AddScoped<IPrerequisiteConstraintService, PrerequisiteConstraintService>();
builder.Services.AddScoped<IPrerequisiteSubjectService, PrerequisiteSubjectService>();
var secretKey = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey)
        };
    });
builder.Services.Configure<AdminAccountOptions>(
    builder.Configuration.GetSection("AdminAccount"));
builder.Services.AddAuthorization();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "AcademicChatBot_API", Version = "v1" });
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Bearer",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<AcademicChatBotDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("AcademicChatBotDB"),
        b => b.MigrationsAssembly("AcademicChatBot.DAL")));

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/notifications");

app.MapControllers();

app.UseCors("AllowAll");

app.Run();
