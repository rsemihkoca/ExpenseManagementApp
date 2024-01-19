using Business.Entities;

namespace Business.Enums;

public enum UserRole
{
    Admin,
    Personnel
}
//
// public sealed class UserRole
// {
//     public const string Admin = "Admin";
//     public const string Personnel = "Personnel";
//     private readonly string _value;
//     
//     private UserRole(string value)
//     {
//         _value = value;
//     }
//     
//     public static implicit operator string(UserRole userRole)
//     {
//         return userRole._value;
//     }
//     public static implicit operator UserRole(string Role)
//     {
//         switch (Role)
//         {
//             case Admin:
//                 return new UserRole(Admin);
//             case Personnel:
//                 return new UserRole(Personnel);
//             default:
//                 throw new ArgumentException("Invalid Role");
//         }
//     }
// }

public enum ExpenseRequestStatus
{
    Pending,
    Approved,
    Rejected
}

public enum PaymentRequestStatus
{
    Pending,
    Completed,
    Failed
}


