namespace Domain.Products.Errors;

public static class ProductError
{
    public static Error Prd001CanNotUpdateNameToTheSameName => new("PRD001", "Can not update name to the same name");
}
