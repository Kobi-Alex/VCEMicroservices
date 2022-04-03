﻿using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using Applicant.Domain.Entities;


namespace Applicant.Infrasructure
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            //if (isProd)
            //{
            //    Console.WriteLine("\n---> Attempting to apply migrations...\n");

            //    try
            //    {
            //        context.Database.Migrate();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"\n---> Could not run migrations: {ex.Message}\n");
            //    }
            //}


            if (!context.Roles.Any())
            {

                List<Role> roles = new List<Role>();

                foreach (var item in Enum.GetValues(typeof(UserRoles)))
                {
                    roles.Add(new Role()
                    {
                        Name = item.ToString(),
                    });
                }

                context.Roles.AddRange(roles);
                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                Console.WriteLine("\n--> Seeding Data... \n");

                var hasher = new PasswordHasher<User>();


                //Add Admind and Manager
                var supremeAdmin = new User
                {
                    FirstName = "Supreme",
                    LastName = "Admin",
                    Email = "supreme@google.com",
                    Password = hasher.HashPassword(null, "Admin1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                    // Roles = new List<Role>() { adminRole, managerRole }
                };
                supremeAdmin.Roles.Add(context.Roles.FirstOrDefault(x => x.Name == "Admin"));
                supremeAdmin.Roles.Add(context.Roles.FirstOrDefault(x => x.Name == "Manager"));
                supremeAdmin.Roles.Add(context.Roles.FirstOrDefault(x => x.Name == "Teacher"));
                context.Users.Add(supremeAdmin);

                //Add Admin 
                User adminUser = new User
                {
                    FirstName = "Alpha Admin",
                    LastName = "Mr. Alpha Admin",
                    AdditionalInfo = "AdditionaInfo Mr Admin",
                    Email = "admin@google.com",
                    Password = hasher.HashPassword(null, "Admin1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                    // Roles = new List<Role>() { adminRole }
                };
                adminUser.Roles.Add(context.Roles.FirstOrDefault(x => x.Name == "Admin"));
                context.Users.Add(adminUser);

                //Add teacher

                User teacher = new User
                {
                    FirstName = "Bravo Teacher",
                    LastName = "Mr. Bravo Teacher",
                    AdditionalInfo = "AdditionaInfo Bravo Teacher",
                    Email = "teacher@google.com",
                    Password = hasher.HashPassword(null, "Teacher1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                    // Roles = new List<Role>() { teacherRole }
                };

                teacher.Roles.Add(context.Roles.FirstOrDefault(x => x.Name == "Teacher"));

                context.Users.Add(teacher);

                //Add Manager
                User manager = new User
                {
                    FirstName = "Charlie Manager",
                    LastName = "Mr. Charlie Manager",
                    AdditionalInfo = "AdditionaInfo Charlie Manager",
                    Email = "manager@google.com",
                    Password = hasher.HashPassword(null, "Manager1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                    //Roles = new List<Role>() { managerRole }

                };
                manager.Roles.Add(context.Roles.FirstOrDefault(x => x.Name == "Manager"));
                context.Users.Add(manager);

                //Add Users
                var user1 = new User()
                {
                    FirstName = "Delta User",
                    LastName = "Mr. Delta User",
                    AdditionalInfo = "AdditionaInfo Delta User",
                    Email = "user1@google.com",
                    Password = hasher.HashPassword(null, "User1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                    //Roles = new List<Role>() { userRole }

                };

                user1.Roles.Add(context.Roles.FirstOrDefault(x => x.Name == "Student"));
                context.Users.Add(user1);
                var user2 = new User()
                {
                    FirstName = "Echo User_2",
                    LastName = "Mr. Echo User_2",
                    AdditionalInfo = "AdditionaInfo  Echo User_2",
                    Email = "user2@google.com",
                    Password = hasher.HashPassword(null, "User2!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                    //Roles = new List<Role>() { userRole }

                };
                user2.Roles.Add(context.Roles.FirstOrDefault(x => x.Name == "Student"));
                context.Users.Add(user2);
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("\n--> We already have data\n");
            }
        }
    }
}
