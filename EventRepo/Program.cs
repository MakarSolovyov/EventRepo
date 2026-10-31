
using EventRepo.Interfaces;
using EventRepo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace EventRepo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;

                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(x => x.Value?.Errors.Count > 0)
                        .SelectMany(x => x.Value!.Errors)
                        .Select(x => x.ErrorMessage)
                        .Distinct()
                        .ToList();

                    var errorMessage = string.Join("; ", errors);

                    var response = new ApiResult
                    {
                        Success = false,
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = $"Ошибка валидации: {errorMessage}"
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            builder.Services.AddSingleton<IEventService, EventService>();

            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
