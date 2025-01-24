namespace Application.Domain.Common.Entities;

using Application.Domain.Common.ValueObjects;

public interface IAuditable
{
    DateTime Created { get; set; }

    UserName CreatedBy { get; set; }

    DateTime? LastModified { get; set; }

    UserName LastModifiedBy { get; set; }
}
