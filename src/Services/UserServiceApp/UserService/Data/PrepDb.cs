using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Data
{
    public static class PrepDb
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
            if (isProd)
            {
                Console.WriteLine("\n---> Attempting to apply migrations...\n");

                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n---> Could not run migrations: {ex.Message}\n");
                }
            }


            if (!context.Roles.Any())
            {

                List<IdentityRole> roles = new List<IdentityRole>();

                foreach (var item in Enum.GetValues(typeof(UserRolesEnum)))
                {
                    roles.Add(new IdentityRole()
                    {
                        Name = item.ToString(),
                        NormalizedName = item.ToString().ToUpper()
                    });
                }

                context.Roles.AddRange(roles);

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                Console.WriteLine("\n--> Seeding Data... \n");

                var hasher = new PasswordHasher<IdentityUser>();

                //Add Admind and Manager
                User adminAndManager = new User
                {
                    FirstName = "Alpha Admin Manager",
                    LastName = "Mr. Alpha Admin Manager",
                    AdditionalInfo = "AdditionaInfo Mr Admin Manger",
                    UserName = "admin-manager",
                    NormalizedUserName = "ADMIN-MANAGER",
                    Email = "adminmanager@google.com",
                    NormalizedEmail = "ADMINMANAGER@GOOGLE.COM",
                    PasswordHash = hasher.HashPassword(null, "AdminManager1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                };

                context.Users.Add(adminAndManager);

                IdentityUserRole<string> adminRole = new IdentityUserRole<string>();
                adminRole.UserId = adminAndManager.Id;
                adminRole.RoleId = context.Roles.FirstOrDefault(r => r.Name == "Admin").Id;

                IdentityUserRole<string> managerRole = new IdentityUserRole<string>();
                managerRole.UserId = adminAndManager.Id;
                managerRole.RoleId = context.Roles.FirstOrDefault(r => r.Name == "Manager").Id;


                context.UserRoles.Add(adminRole);
                context.UserRoles.Add(managerRole);


                context.SaveChanges();



                //Add Admin 
                User adminUser = new User
                {
                    FirstName = "Alpha Admin",
                    LastName = "Mr. Alpha Admin",
                    AdditionalInfo = "AdditionaInfo Mr Admin",
                    UserName = "admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@google.com",
                    NormalizedEmail = "ADMIN@GOOGLE.COM",
                    PasswordHash = hasher.HashPassword(null, "Admin1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                };

                context.Users.Add(adminUser);

                IdentityUserRole<string> admin = new IdentityUserRole<string>();
                admin.UserId = adminUser.Id;
                admin.RoleId = context.Roles.FirstOrDefault(r => r.Name == "Admin").Id;

                context.UserRoles.Add(admin);
                context.SaveChanges();

                //Add teacher

                User teacher = new User
                {
                    FirstName = "Bravo Teacher",
                    LastName = "Mr. Bravo Teacher",
                    AdditionalInfo = "AdditionaInfo Bravo Teacher",
                    UserName = "Teacher",
                    NormalizedUserName = "Teacher".ToUpper(),
                    Email = "teacher@google.com",
                    NormalizedEmail = "teacher@google.com".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "Teacher1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                };

                context.Users.Add(teacher);

                IdentityUserRole<string> teacherUserRole = new IdentityUserRole<string>();
                teacherUserRole.UserId = teacher.Id;
                teacherUserRole.RoleId = context.Roles.FirstOrDefault(r => r.Name == "Teacher").Id;

                context.UserRoles.Add(teacherUserRole);
                context.SaveChanges();

                //Add Manager
                User manager = new User
                {
                    FirstName = "Charlie Manager",
                    LastName = "Mr. Charlie Manager",
                    AdditionalInfo = "AdditionaInfo Charlie Manager",
                    UserName = "Manager",
                    NormalizedUserName = "Manager".ToUpper(),
                    Email = "manager@google.com",
                    NormalizedEmail = "manager@google.com".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "Manager1!"),
                    CreatedAt = new DateTimeOffset(DateTime.Now),
                    UpdatedAt = new DateTimeOffset(DateTime.Now),
                };

                context.Users.Add(manager);

                IdentityUserRole<string> managerUserRole = new IdentityUserRole<string>();
                managerUserRole.UserId = manager.Id;
                managerUserRole.RoleId = context.Roles.FirstOrDefault(r => r.Name == "Manager").Id;

                context.UserRoles.Add(managerUserRole);
                context.SaveChanges();


                //Add Users
                User[] users = new User[]
                {
                    new User()
                    {
                        FirstName = "Delta User",
                        LastName = "Mr. Delta User",
                        AdditionalInfo = "AdditionaInfo Delta User",
                        UserName = "User_1",
                        NormalizedUserName = "USER_1",
                        Email = "user1@google.com",
                        NormalizedEmail = "user1@google.com".ToUpper(),
                        PasswordHash = hasher.HashPassword(null, "User1!"),
                        CreatedAt = new DateTimeOffset(DateTime.Now),
                        UpdatedAt = new DateTimeOffset(DateTime.Now),
                    },
                    new User()
                    {
                        FirstName = "Echo User_2",
                        LastName = "Mr. Echo User_2",
                        AdditionalInfo = "AdditionaInfo  Echo User_2",
                        UserName = "User_2",
                        NormalizedUserName = "USER_2",
                        Email = "user2@google.com",
                        NormalizedEmail = "user2@google.com".ToUpper(),
                        PasswordHash = hasher.HashPassword(null, "User2!"),
                        CreatedAt = new DateTimeOffset(DateTime.Now),
                        UpdatedAt = new DateTimeOffset(DateTime.Now),
                    },
                    new User()
                    {
                        FirstName = "Foxtor User_3",
                        LastName = "Mr. Foxtor User_3",
                        AdditionalInfo = "AdditionaInfo Foxtor User_3",
                        UserName = "User_3",
                        NormalizedUserName = "USER_3",
                        Email = "user3@google.com",
                        NormalizedEmail = "user3@google.com".ToUpper(),
                        PasswordHash = hasher.HashPassword(null, "User3!"),
                        CreatedAt = new DateTimeOffset(DateTime.Now),
                        UpdatedAt = new DateTimeOffset(DateTime.Now),
                    }
                };

                context.Users.AddRange(users);

                foreach (var u in users)
                {
                    IdentityUserRole<string> ur = new IdentityUserRole<string>();
                    ur.UserId = u.Id;
                    ur.RoleId = context.Roles.FirstOrDefault(r => r.Name == "Student").Id;

                    context.UserRoles.Add(ur);
                }

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("\n--> We already have data\n");
            }
        }
    }
}
