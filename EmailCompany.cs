using System.Text.Json.Serialization;
using static ConsoleAppTemplateMails.EmailCompany;

namespace ConsoleAppTemplateMails;

public record EmailCompany(EmailSubject Subject, EmailBody Body) : IEmailTemplate<EmailSubject, EmailBody>
{
    public record EmailSubject([property: EmailTemplate("nameCompany")] string? NameCompany);

    public record EmailBody(
        [property: JsonPropertyName("555")][property: EmailTemplate("userName")] string? UserName,
        [property: EmailTemplate("datetime")] DateTime Date,
        [property: EmailTemplate("rolesStr")] string? RolesStr,
        [property: EmailTemplate("urlStr")] string? UrlStr,
        [property: EmailTemplate("errorTemplStr")] string? ErrorTemplStr,
        [property: EmailTemplate("text")] string? Text);
}
