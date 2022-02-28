using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Data;
public static class SeedData
{
    private static Faker faker = null!;
    private static LmsApiContext _context = null!;

    public static async Task InitAsync(LmsApiContext context)
    {
        if (await context.Course.AnyAsync()) return;

        _context = context;

        faker = new Faker("sv");

        List<ICollection<Module>> modules = new List<ICollection<Module>>();
        var courses = new List<Course>();

        int AmountCourses = faker.Random.Int(6, 33);

        for (int i = 0; i < AmountCourses; i++)
        {
            modules.Add(GetModules(faker.Random.Int(3, 16)));

            await context.AddRangeAsync(modules.Last());

            courses.Add(GetCourse(modules.Last()));
        }


        //await context.AddRangeAsync(modules);
        await context.AddRangeAsync(courses);

        await context.SaveChangesAsync();
    }

    /*private static IEnumerable<Course> GetCourses(ICollection<Module> modules)
    {
        List<Course> courses = new List<Course>();
        for (int i = 0; i < AmountCourses; i++)
        {
            courses.Add(new Course
            {
                //Id = faker.Random.Int(),
                Title = faker.Company.CompanyName(),
                StartTime = faker.Date.Past(2),
                Modules = modules
            });
        }
        return courses;
    }*/
    private static Course GetCourse(ICollection<Module> modules)
    {
        return (new Course
        {
            Title = faker.Internet.DomainName(),
            StartDate = faker.Date.Past(2),
            Modules = modules
        });
    }

    private static ICollection<Module> GetModules(int ModMany)
    {
        List<Module> modules = new List<Module>();

        for (int i = 0; i < ModMany; i++)
        {
            modules.Add(new Module
            {
                //Id = faker.Random.Int(),
                Title = faker.Commerce.Department(),
                StartDate = faker.Date.Past()
                //CourseId = course.Id
            });
        }

        return modules;
    }

}