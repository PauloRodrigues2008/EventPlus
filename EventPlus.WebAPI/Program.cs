using Azure.AI.ContentSafety;
using EventPlus.WebAPI.BdContextEvent;
using EventPlus.WebAPI.Interfaces;
using EventPlus.WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var endpoint = "https://moderatorservice-marcos.cognitiveservices.azure.com/";
var apiKey = "";

var client = new ContentSafetyClient(new Uri(endpoint),
    new Azure.AzureKeyCredential
    (apiKey)) ;
builder.Services.AddSingleton(client);  

builder.Services.AddDbContext<EventContext>(
    options => options.UseSqlServer
    (builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Registrar as  Repositories (Injecao de Dependencia
builder.Services.AddScoped<ITipoEventoRepository, TipoEventoRepository>();
builder.Services.AddScoped<ITipoUsuarioRepository, TipoUsuarioRepository>();  
builder.Services.AddScoped<IInstituicaoRepository, InstituicaoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IPresencaRepository, PresencaRepository>();  
builder.Services.AddScoped<IComentarioEventoRepository, ComentarioEventoRepository>();


//////////////////////////////////////////////////////////////////////////////////////////////

//Adiciona o serviço de autenticação JWT Bearer (forma de autenticação)
object valeu = builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
    .AddJwtBearer("JwtBearer", options =>


    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //valida quem está solicitando
            ValidateIssuer = true,

            //valida quem está recebendo
            ValidateAudience = true,

            //define se o tempo de expiração será validado
            ValidateLifetime = true,

            //forma de criptografia e valida a chave de autenticação
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("filmes-chave-autenticacao-webapi-dev")),

            //valida o tempo de expiração do token
            ClockSkew = TimeSpan.FromMinutes(5),

            //nome do issuer (de onde está vindo)
            ValidIssuer = "api_event",

            //nome do audience (para onde ele está indo)
            ValidAudience = "api_event"
        };
    });

//////////////////////////////////////////////////////////////////////////////////////



// adiciona Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
    Version = "v1",
    Title = "API de Eventos", 
    Description = "Aplicação para gerenciamento de eventos",  
    TermsOfService = new Uri("https://example.com/terms"),
    Contact = new OpenApiContact
    {
        Name = "Paulo André",
        Url = new Uri("https://www.linkedin.com/in/marcaumdev")
    },
        License = new OpenApiLicense  
        {   
            Name = "Exemplo de licensa",
            Url = new Uri("https://opensource.org/licenses/MIT")        
        }

    });
    // Usuando a autenticacao no swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http, 
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT:"

    });

    options.AddSecurityRequirement(document => new
    OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer",
        document)] = Array.Empty<string>().ToList()

    });
});   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger(options => { });
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", " v1");
        options.RoutePrefix = string.Empty; // Define a raiz para acessar o Swagger UI
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
