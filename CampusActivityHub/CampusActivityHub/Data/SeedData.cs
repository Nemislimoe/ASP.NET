using CampusActivityHub.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusActivityHub.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (await db.Categories.AnyAsync()) return;

        var cat1 = new Category { Name = "Lectures", Description = "Academic talks", IconClass = "bi-journal" };
        var cat2 = new Category { Name = "Workshops", Description = "Hands-on", IconClass = "bi-tools" };
        var cat3 = new Category { Name = "Social", Description = "Meetups", IconClass = "bi-people" };
        db.Categories.AddRange(cat1, cat2, cat3);

        var admin = new ApplicationUser { Id = "admin", Name = "Admin", Email = "admin@campus.local", Role = "Admin" };
        var org1 = new ApplicationUser { Id = "org1", Name = "Organizer One", Email = "org1@campus.local", Role = "Organizer" };
        var org2 = new ApplicationUser { Id = "org2", Name = "Organizer Two", Email = "org2@campus.local", Role = "Organizer" };
        var student = new ApplicationUser { Id = "student1", Name = "Student One", Email = "student1@campus.local", Role = "Student" };
        db.Users.AddRange(admin, org1, org2, student);

        var events = new List<Event>
        {
            new Event
            {
                Title = "Intro to AI",
                StartAt = DateTime.UtcNow.AddDays(7),
                Capacity = 100,
                Category = cat1,
                Organizer = org1,
                PosterPath = "images/default-poster.png"
            },
            new Event
            {
                Title = "Web Dev Workshop",
                StartAt = DateTime.UtcNow.AddDays(10),
                Capacity = 30,
                Category = cat2,
                Organizer = org2,
                PosterPath = "images/default-poster.png"
            },
            new Event
            {
                Title = "Student Mixer",
                StartAt = DateTime.UtcNow.AddDays(3),
                Capacity = 200,
                Category = cat3,
                Organizer = org1,
                PosterPath = "images/default-poster.png"
            },
            new Event
            {
                Title = "Career Talk",
                StartAt = DateTime.UtcNow.AddDays(14),
                Capacity = 150,
                Category = cat1,
                Organizer = org2,
                PosterPath = "images/default-poster.png"
            },
            new Event
            {
                Title = "Design Sprint",
                StartAt = DateTime.UtcNow.AddDays(21),
                Capacity = 40,
                Category = cat2,
                Organizer = org1,
                PosterPath = "images/default-poster.png"
            }
        };
        db.Events.AddRange(events);

        // sample registration
        db.Registrations.Add(new Registration
        {
            Event = events[0],
            User = student,
            RegistrationDate = DateTime.UtcNow
        });

        await db.SaveChangesAsync();
    }
}
