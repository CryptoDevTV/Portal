﻿using Dapper;
using Portal.Shared.Data.Db;
using Portal.Shared.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portal.Shared.Data
{
    public interface IDataRepository
    {
        Task<int> CreateUserAsync(User user);
        Task<User> GetUserInfoAsync(string userGuid);
        Task<User> GetUserAsync(string userEmail);
        Task<IEnumerable<Course>> GetAllCoursesByUserGuidAsync(string userGuid);
        Task<IEnumerable<Module>> GetAllModulesByUserGuidAndModuleIdAsync(string userGuid, int courseId);
        Task<Lesson> GetLessonByUserGuidAndLessonId(string userGuid, int lessonId);
        Task<int> CreateOrderHistoryAsync(Order order);
        Task<int> AddUserToCourseAsync(UserCourse userCourse);
    }

    public class DataRepository : IDataRepository
    {
        private readonly IDbAccess _dbAccess;

        public DataRepository(
            IDbAccess dbAccess)
        {
            _dbAccess = dbAccess;
        }

        public async Task<int> AddUserToCourseAsync(UserCourse userCourse)
        {
            using (var c = _dbAccess.Connection)
            {
                return await c.ExecuteAsync(
                    @"INSERT INTO UserCourses (UserId, CourseId, IsPurchased) VALUES (@UserId, @CourseId, @IsPurchased);
                    SELECT CAST(SCOPE_IDENTITY() as int)",
                    new
                    {
                        userCourse.UserId,
                        userCourse.CourseId,
                        IsPurchased = true
                    });
            }
        }

        public async Task<int> CreateOrderHistoryAsync(Order order)
        {
            using (var c = _dbAccess.Connection)
            {
                return await c.ExecuteAsync(
                    @"INSERT INTO Orders (OrderMnemonic, Email, Status, BtcPrice, Rate) VALUES (@OrderMnemonic, @Email, @Status, @BtcPrice, @Rate);
                    SELECT CAST(SCOPE_IDENTITY() as int)",
                    new
                    {
                        order.OrderMnemonic,
                        Email = order.Email.ToLowerInvariant(),
                        order.Status,
                        order.BtcPrice,
                        order.Rate
                    });
            }
        }

        public async Task<int> CreateUserAsync(User user)
        {
            using (var c = _dbAccess.Connection)
            {
                return await c.ExecuteAsync(
                    @"INSERT INTO Users (Email, Guid) VALUES(@Email, @Guid);
                    SELECT CAST(SCOPE_IDENTITY() as int)",
                    new
                    {
                        Email = user.Email.ToLowerInvariant(),
                        Guid = Guid.NewGuid().ToString()
                    });
            }
        }

        public async Task<IEnumerable<Course>> GetAllCoursesByUserGuidAsync(string userGuid)
        {
            using (var c = _dbAccess.Connection)
            {
                return await c.QueryAsync<Course>(@"SELECT 
                    C.CourseId,
                    C.Name,
                    C.Description,
                    C.PathMp3,
                    C.PathMp4 
                    FROM Courses as C 
                    INNER JOIN UserCourses UC ON C.CourseId = UC.CourseId
                    INNER JOIN Users U ON UC.UserId = U.UserId
                    WHERE U.Guid = @Guid AND UC.IsPurchased = 1",
                    new
                    {
                        Guid = userGuid
                    });
            }
        }

        public async Task<IEnumerable<Module>> GetAllModulesByUserGuidAndModuleIdAsync(string userGuid, int courseId)
        {
            using (var c = _dbAccess.Connection)
            {
                var modules = await c.QueryAsync<Module>(@"SELECT 
                    M.ModuleId,
                    M.Name,
                    M.Description
                    FROM Courses as C 
                    INNER JOIN UserCourses UC ON C.CourseId = UC.CourseId
                    INNER JOIN Users U ON UC.UserId = U.UserId
                    INNER JOIN Modules M ON M.CourseId = @CourseId
                    WHERE U.Guid = @Guid AND UC.IsPurchased = 1",
                    new
                    {
                        Guid = userGuid,
                        CourseId = courseId
                    });

                if (modules.Any())
                {
                    foreach (var module in modules)
                    {
                        module.Lessons = await c.QueryAsync<Lesson>(@"SELECT 
                            L.LessonId,
                            L.Name,
                            L.ContentUrl,
                            L.ContentRawUrl,
                            L.Duration 
                            FROM Lessons as L 
                            WHERE L.ModuleId = @ModuleId",
                            new
                            {
                                module.ModuleId
                            });
                    }
                }

                return modules;
            }
        }

        public async Task<Lesson> GetLessonByUserGuidAndLessonId(string userGuid, int lessonId)
        {
            using (var c = _dbAccess.Connection)
            {
                return await c.QueryFirstOrDefaultAsync<Lesson>(@"SELECT 
                    L.LessonId,
                    L.Name,
                    L.ContentUrl,
                    L.ContentRawUrl,
                    L.Duration,
                    C.CourseId 
                    FROM Courses as C 
                    INNER JOIN UserCourses UC ON C.CourseId = UC.CourseId
                    INNER JOIN Users U ON UC.UserId = U.UserId
                    INNER JOIN Modules M ON M.CourseId = C.CourseId
                    INNER JOIN Lessons L ON L.ModuleId = M.ModuleId
                    WHERE U.Guid = @Guid AND L.LessonId = @LessonId AND UC.IsPurchased = 1",
                    new
                    {
                        Guid = userGuid,
                        LessonId = lessonId
                    });
            }
        }

        public async Task<User> GetUserAsync(string userEmail)
        {
            using (var c = _dbAccess.Connection)
            {
                return await c.QueryFirstOrDefaultAsync<User>(@"SELECT 
                    U.UserId,
                    U.Email,
                    U.Guid 
                    FROM Users U 
                    WHERE U.Email = @Email",
                    new
                    {
                        Email = userEmail
                    });
            }
        }

        public async Task<User> GetUserInfoAsync(string userGuid)
        {
            using (var c = _dbAccess.Connection)
            {
                return await c.QueryFirstOrDefaultAsync<User>(@"SELECT 
                    U.UserId,
                    U.Email,
                    U.Guid 
                    FROM Users U 
                    WHERE U.Guid = @Guid",
                    new
                    {
                        Guid = userGuid
                    });
            }
        }
    }
}