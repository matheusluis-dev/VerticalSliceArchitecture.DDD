namespace Domain.Inventories.Errors;

public static class AdjustmentError
{
    public static Error Adj001InventoryIdMustBeInformed => new("ADJ001", "Inventory ID must be informed");
    public static Error Adj002ReasonMustBeInformed => new("ADJ002", "Reason must be informed");

    public static Error Adj003ReasonMustHaveAtLeast15Characters =>
        new("ADJ003", "Reason must have at least 15 characters");
}
