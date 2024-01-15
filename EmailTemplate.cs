namespace ConsoleAppTemplateMails;

public record EmailTemplate(string Subject, string Body) : IEmailTemplate<string, string>;
