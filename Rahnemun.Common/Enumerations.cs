namespace Rahnemun.Common
{
    public enum Gender: byte
    {
        Male,
        Female
    }

    public enum EducationLevel: byte
    {
        PrimarySchool,
        MiddleSchool,
        HighSchool,
        Associate,
        Bachelor,
        Master,
        Doctorate
    }

    public enum MaritalStatus : byte
    {
        Single,
        Married
    }

    public enum SessionStopType : byte
    {
        ConsultantRequest,
        ConsulteeRequest,
        ConsulteeInactivity
    }

    public enum CustomerMessageSubject : byte
    {
        Other,
        Suggestion,
        Support,
        Feedback
    }
}