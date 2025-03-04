namespace Domain.Products.Errors;

public static class ProductError
{
    public static Error Prd001CanNotUpdateNameToTheSameName => new("PRD001", "Can not update name to the same name");
    public static Error Prd002DoesNotHaveInventory => new("PRD002", "Product does not have Inventory");
    public static Error Prd003AlreadyHaveInventory => new("PRD003", "Product already have an Inventory");
}
