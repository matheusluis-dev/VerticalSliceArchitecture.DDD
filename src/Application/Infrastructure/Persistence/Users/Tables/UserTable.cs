namespace Application.Infrastructure.Persistence.Users.Tables;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.Common.ValueObjects;
using Application.Domain.User.ValueObjects;

public sealed class UserTable : IAuditable
{
    public UserId Id { get; set; }
    public UserName Name { get; set; }
    public Email Email { get; set; }

    public DateTime Created { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserId LastModifiedBy { get; set; }
}
