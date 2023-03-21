using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalJwtProject.Models;
using MinimalJwtProject.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Swagger yap�land�rmam�z Authorization yetkilendirmeleri i�in gerekli yetki atamas� yap�lan servislere jwt ile d�nen tokenlar� g�r�p aksiyon almas�n� sa�lamak i�in yap�yoruz bu i�lemleri.
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme ="Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name= "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type=SecuritySchemeType.Http

    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement

    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id="Bearer",
                    Type = ReferenceType.SecurityScheme
                }

            },

            new List<string>()

        }
    } );
});

builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey= true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))




    };
});


builder.Services.AddAuthorization();
//addsingleton dependices injeciton i�in gerekli

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddSingleton<IUserService, UserService>();

//asenkronlarda nas�l patlad���n� g�r sonra jwt ge�
//Mongo db ba�la

//endpointleri role bazl� filtrele authorize olarak git her bir endpoint i�in

//ve bu alttaki mapler mesela movie/get gibi olmal�


var app = builder.Build();

app.UseSwagger();
app.UseAuthorization();
app.UseAuthentication();

app.MapGet("/", () => "Simple MinimalApi Project.");

app.MapPost("/login",
    
(UserLogin user, IUserService service) => Login(user, service));

app.MapPost("/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Movie movie, IMovieService service) => Create(movie, service));

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standart, Administrator")]
(int id, IMovieService service) => Get(id, service));

app.MapGet("/List",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standart, Administrator")]
(IMovieService service) => List(service));

app.MapPut("/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Movie putMovie, IMovieService service) => Update(putMovie, service));

app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(int id, IMovieService service) => Delete(id, service));

// �imdi yukar�da haritalar�m�z� belirledik fakat bunlar�n i�erdi�i crud fonksiyonlar�n�da olu�turmam�z laz�m
// ENG- Now we have defined our maps above, but we need to create the crud functions they contain.


IResult Login(UserLogin user, IUserService service)
{
    if(!string.IsNullOrEmpty(user.Username)&&
            !string.IsNullOrEmpty(user.Password))
    {
        var loggedInUser = service.Get(user);
        if (loggedInUser is null) return Results.NotFound("User not found");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
            new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
            new Claim(ClaimTypes.GivenName, loggedInUser.GivenName),
            new Claim(ClaimTypes.Surname, loggedInUser.Surname),
            new Claim(ClaimTypes.Role, loggedInUser.Role)
        };
        var token = new JwtSecurityToken(
            issuer: builder.Configuration["Jwt:Issuer"],
            audience: builder.Configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(60),
            notBefore:DateTime.UtcNow,
            signingCredentials: new SigningCredentials( 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                SecurityAlgorithms.HmacSha256)

            );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(tokenString);
    }
    return Results.Ok(user);
}
IResult Create(Movie movie, IMovieService service)
{
    var result = service.Create(movie);
    return Results.Ok(result);

}

// bunlar�n normalde farkl� bir klas�r veya libraryden bas�lmas� laz�m �nemli



IResult Get(int id, IMovieService service)
{
    var movie = service.Get(id);

    if (movie is null) return Results.NotFound("Movie not found");

    return Results.Ok(movie);

}

IResult List(IMovieService service)
{
    var movies = service.List();

    return Results.Ok(movies);
}

IResult Update(Movie putMovie, IMovieService service)
{
    var updateMovie = service.Update(putMovie);
    if (updateMovie is null) Results.NotFound("Movie not found");
    return Results.Ok(updateMovie);


}


IResult Delete(int id, IMovieService service)
{

    var result = service.Delete(id);
    if (!result) Results.BadRequest("something went wrong");

    return Results.Ok(result);

}

app.UseSwaggerUI();



app.Run();
