namespace Schemes.Constants;

public static class Constants
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Personnel = "Personnel";
        public const string AdminOrPersonnel = "Admin, Personnel";
    }

    public static class ExpenseRequestStatus
    {
        public const string Pending = "Pending";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
    }

    public static class PaymentRequestStatus
    {
        public const string Pending = "Pending";
        public const string Declined = "Declined";
        public const string OnProcess = "OnProcess";
        public const string Completed = "Completed";
        public const string Failed = "Failed";
    }

    public static class ContentType
    {
        public const string Json = "application/json";
    }

    public static class ErrorMessages
    {
        public const string InvalidUserInformation = "Invalid user information";
        public const string ContactAdministrator = "Please contact your administrator, your account is locked.";
        public const string NoRecordFound = "No record found";
        public const string RoleNotFound = "Role not found";
        public const string EmailNotFound = "Email not found";
        public const string UserNotFound = "User not found";
        public const string NotOwnId = "Please enter your own id";
        public const string UnauthorizedRole = "Unauthorized role";
        public const string MadePayment = "Expense already approved and payment completed";
        public const string InProgressPayment = "Expense already approved and payment is in progress, please wait";
        public const string IdLessThanZero = "Id must be greater than zero";
    }

    public static class ExpenseValidationMessages
    {
        public const string UserIdRequired = "User id is required.";
        public const string UserIdGreaterThanZero = "User id must be greater than zero.";

        public const string AmountRequired = "Amount is required.";
        public const string AmountGreaterThanZero = "Amount must be greater than zero.";

        public const string CategoryIdRequired = "Category is required.";
        public const string CategoryIdGreaterThanZero = "Category must be greater than zero.";

        public const string PaymentMethodRequired = "Payment method is required.";
        public const string PaymentMethodMaxLength = "Payment method cannot exceed 50 characters.";

        public const string PaymentLocationRequired = "Payment location is required.";
        public const string PaymentLocationMaxLength = "Payment location cannot exceed 255 characters.";

        public const string DocumentsPathRequired = "Documents path is required.";
        public const string DocumentsPathMaxLength = "Documents path cannot exceed 255 characters.";

        public const string StatusRequired = "Status is required.";
        public const string StatusInvalid = "Status must be one of the following: Pending, Approved, Rejected.";

        public const string DescriptionRequired = "Description is required.";
        public const string DescriptionMaxLength = "Description cannot exceed 255 characters.";

        public const string PaymentStatusRequired = "Payment status is required.";

        public const string PaymentStatusInvalid =
            "Payment status must be one of the following: Pending, Declined, Completed, Failed.";

        public const string PaymentDescriptionRequired = "Payment description is required.";
        public const string PaymentDescriptionMaxLength = "Payment description cannot exceed 255 characters.";

        public const string UserIdGreaterThanZeroWhenProvided = "UserId must be greater than 0 when provided.";
        public const string CategoryIdGreaterThanZeroWhenProvided = "CategoryId must be greater than 0 when provided.";
    }

    public static class ReportValidationMessages
    {
        public const string TypeRequired = "Type is required.";
        public const string InvalidType = "Type must be daily, weekly, or monthly.";
    }

    public static class PersonnelSummaryValidationMessages
    {
        public const string TypeRequired = "Type is required.";
        public const string InvalidType = "Type must be daily, weekly, or monthly.";

        public const string UserIdGreaterThanZero = "UserId must be greater than 0.";
    }
    
    public static class UserValidationMessages
    {
        public const string RequiredMessage = "{0} is required.";
        public const string InvalidLengthMessage = "{0} must be between {1} and {2} characters.";
        public const string InvalidEmailMessage = "Invalid email address.";
        public const string InvalidIbanMessage = "Invalid IBAN.";
        public const string InvalidRoleMessage = "Invalid role.";
        public const string InvalidPasswordMessage =
            "Password must contain at least 1 special character, 1 number, 1 uppercase letter, and 1 lowercase letter.";

        public static string Required(string fieldName) => string.Format(RequiredMessage, fieldName);
        public static string InvalidLength(string fieldName, int minLength, int maxLength) =>
            string.Format(InvalidLengthMessage, fieldName, minLength, maxLength);
    }
    public static class Credentials
    {
        public const string Id = "Id";
        public const string Email = "Email";
        public const string Username = "Username";
    }
    
    public static class DefaultValues
    {
        public const string PaymentDescription = "Payment Not Made";
    }
    
    public static class Database
    {
        public const string Schema = "CaseDb";
        public const string ExpenseCategoryTable = "ExpenseCategory";
        public const string ExpenseTable = "Expense";
        public const string UserTable = "Users";
    }
    
    public static class Frequency
    {
        public const string Daily = "daily";
        public const string Weekly = "weekly";
        public const string Monthly = "monthly";
    }

    public static class DateFormats
    {
        public const string DateFormat = "dd/MM/yyyy";
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
    }

    public static class UserStatus
    {
        public const string Active = "Active";
        public const string Inactive = "Inactive";
    }
}