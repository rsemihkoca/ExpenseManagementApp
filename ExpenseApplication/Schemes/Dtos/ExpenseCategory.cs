namespace Schemes.Dtos;

public class CreateExpenseCategoryRequest
{
    public string CategoryName { get; set; }
}

public class UpdateExpenseCategoryRequest
{
    public string CategoryName { get; set; }
}

public class ExpenseCategoryResponse
{
    public int CategoryId { get; set; } // (Primary Key)
    public string CategoryName { get; set; }
}