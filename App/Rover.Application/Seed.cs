using Rover.Domain;
using Rover.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Seed
{
    public static async Task SeedData(DataContext context)
    {
        if (context.Users.Any()) return;

        var users = new List<User>
{
            new User
            {
                Name = "Adam",
                Surname = "Kowalski",
                PhoneNunber = 957439583,
                Email = "akowalski@gmail.com",
                MainCity = "Gliwice",
                Birthday = new DateTime(1999, 4, 17)
            },
            new User
            {
                Name = "Ewa",
                Surname = "Nowak",
                PhoneNunber = 785634125,
                Email = "enowak@gmail.com",
                MainCity = "Warsaw",
                Birthday = new DateTime(1995, 11, 30)
            },
            new User
            {
                Name = "Jan",
                Surname = "Kowalczyk",
                PhoneNunber = 698745632,
                Email = "jkowalczyk@gmail.com",
                MainCity = "Krakow",
                Birthday = new DateTime(2001, 7, 15)
            },
            new User
            {
                Name = "Anna",
                Surname = "Wójcik",
                PhoneNunber = 912847562,
                Email = "awojcik@gmail.com",
                MainCity = "Poznan",
                Birthday = new DateTime(1988, 3, 8)
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }
}