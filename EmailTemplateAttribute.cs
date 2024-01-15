namespace ConsoleAppTemplateMails;

class EmailTemplateAttribute : Attribute
{
    public string Name { get; }

    public EmailTemplateAttribute(string name) => Name = name;
}