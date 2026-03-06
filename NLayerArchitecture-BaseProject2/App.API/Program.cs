using App.Services;
using App.Services.Extensions;
using APP.Repositories;
using APP.Repositories.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
	options.Filters.Add<FluentValidationFilter>();
	options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; //=> reference type'ler için null kontrolü gerçekleþtirmicek
});

//kendi ürettiði response modeli kapatmak için kullandýk(fluent validation eklenecek)
builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//dbcontext eklendi repository extension ile
builder.Services.AddRepositories(builder.Configuration).AddServices(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
