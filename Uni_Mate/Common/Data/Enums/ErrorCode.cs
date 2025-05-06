namespace Uni_Mate.Common.Data.Enums;

public enum ErrorCode
{
    None,
    InvalidRequest,
    InvalidData,
    NotFound,
    Unauthorized,
    Forbidden,
    InternalServerError,
    ServiceUnavailable,
    UnknownError,
    UserAlreadyExists,
    UserCreationFailed,
    EmailSendingFailed,
    ExpectionHappend,
    UserNotFound,
    EmailNotConfirmed,
    InvalidPassword,
	MissingUserName,
	MissingNationalID,
    InvalidNationalId,
    PasswordChangeFailed,
    InvalidOTP,
    OwnerNotAuthried,
    ApartmentAlreadyExist,
    ApartmentNotFound,
}
