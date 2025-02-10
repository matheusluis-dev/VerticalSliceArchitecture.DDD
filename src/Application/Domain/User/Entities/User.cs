namespace Application.Domain.User.Entities;

using System;
using Application.Domain.Common.Entities;
using Application.Domain.User.ValueObjects;

public sealed class User : IEntity, IAuditable
{
    public DateTime Created { get; set; }
    public UserId CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public UserId LastModifiedBy { get; set; }
}
