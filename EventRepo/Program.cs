
using EventRepo.Interfaces;
using EventRepo.Services;
using Microsoft.AspNetCore.Mvc;

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

            builder.Services.AddScoped<IEventService, EventService>();

            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
