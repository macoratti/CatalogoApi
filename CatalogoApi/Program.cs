using CatalogoApi.ApiEndpoints;
using CatalogoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();

var environment = app.Environment;

app.UseExceptionHandling(environment)
   .UseSwaggerEndpoints()
   .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();