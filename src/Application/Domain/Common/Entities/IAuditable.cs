namespace Application.Domain.Common.Entities;

using Application.Domain.User.ValueObjects;

public interface IAuditable
{
    DateTime Created { get; set; }

    UserId CreatedBy { get; set; }

    DateTime? LastModified { get; set; }

    UserId LastModifiedBy { get; set; }
}
