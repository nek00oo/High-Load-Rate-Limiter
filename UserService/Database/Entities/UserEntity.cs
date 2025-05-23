﻿using UserService.Models;
using UserService.Models.DomainInterfaces;

namespace UserService.Database.Entities;

public class UserEntity : IUser
{
    public int Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public int Age { get; set; }
}