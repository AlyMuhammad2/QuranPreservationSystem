using Microsoft.EntityFrameworkCore;
using QuranPreservationSystem.Application.Interfaces;
using QuranPreservationSystem.Application.Interfaces.IRepositories;
using QuranPreservationSystem.Infrastructure.Data;
using QuranPreservationSystem.Infrastructure.Repositories;
using QuranPreservationSystem.Infrastructure.UnitOfWork;

namespace QuranPreservationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // إضافة DbContext للتطبيق
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("QuranPreservationSystem")));

            // تسجيل Repositories
            builder.Services.AddScoped<ICenterRepository, CenterRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IStudentCourseRepository, StudentCourseRepository>();

            // تسجيل Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
