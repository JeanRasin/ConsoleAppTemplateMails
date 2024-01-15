using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleAppTemplateMails;

public static class EmailTempleteWork
{
    public static EmailTemplate Build<S, B>(EmailTemplate mail, IEmailTemplate<S, B> data)
    {
        HashSet<string> tegs = GetTegs(mail);

        if (!tegs.Any())
            return mail;

        var newTegs = new HashSet<string>();

        (HashSet<string> subjectNewTegs, string subjectText) = RewritingTags(mail.Subject, data.Subject);

        newTegs.AddRange(subjectNewTegs);

        (HashSet<string> bodyNewTegs, string bodyText) = RewritingTags(mail.Body, data.Body);

        newTegs.AddRange(bodyNewTegs);

        if (tegs.Count - newTegs.Count != 0)
        {
            IEnumerable<string> tegsExcept = tegs.Except(newTegs).Select(GetTeg);

            var strError = $"Теги в шаблоне которые не зполнились: {string.Join(",", tegsExcept)}";

            throw new ArgumentException(strError);
        }

        return new EmailTemplate(subjectText, bodyText);
    }

    private static (HashSet<string> newTegs, string text) RewritingTags<T>(string text, T data)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException($"\"{nameof(text)}\" не может быть неопределенным или пустым.", nameof(text));

        var newTegs = new HashSet<string>();

        PropertyInfo[] props = typeof(T).GetProperties();

        foreach (PropertyInfo prop in props)
        {
            // Получаем значение свойства
            string? val = prop.GetValue(data)?.ToString();

            // Получаем атрибуты
            IList<CustomAttributeData> attributeData = prop.GetCustomAttributesData();

            foreach (CustomAttributeData attr in attributeData)
            {
                // Если атрибут не типа MailTemplateAttribute пропускаем
                if (attr.AttributeType.Name is not nameof(EmailTemplateAttribute))
                    continue;

                // Получаем первый аргумент атрибута
                CustomAttributeTypedArgument? attributeTypedArgument = attr.ConstructorArguments.FirstOrDefault();

                if (attributeTypedArgument is not null)
                {
                    // Получаем значение атрибута
                    string? attrVal = attributeTypedArgument.Value.Value as string;

                    if (attrVal is null)
                        throw new ArgumentNullException(nameof(attrVal));

                    string teg = GetTeg(attrVal);

                    int tegStartIndex = text.IndexOf(teg);

                    int bracketStartIndex = text.LastIndexOf('[', tegStartIndex);
                    int bracketEndIndex = text.IndexOf(']', tegStartIndex);

                    if (bracketEndIndex != -1 && bracketStartIndex != -1 && val is null)
                    {
                        // Удаляем текст внутри квадратных скобок, как и сами скобки если свойство is null 
                        text = text.Remove(bracketStartIndex, bracketEndIndex - bracketStartIndex + 1);
                    }
                    else
                    {
                        text = text.Replace(teg, val);
                    }

                    newTegs.Add(attrVal);
                }
            };
        }

        return (newTegs, text);
    }

    private static string GetTeg(string val)
    {
        return "{#" + val + "}";
    }

    private static HashSet<string> GetTegs(EmailTemplate mail)
    {
        var result = new HashSet<string>();

        result.AddRange(GetTegs(mail.Subject));
        result.AddRange(GetTegs(mail.Body));

        return result;
    }

    private static HashSet<string> GetTegs(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new ArgumentException($"\"{nameof(text)}\" не может быть неопределенным или пустым.", nameof(text));

        var result = new HashSet<string>();

        const string pattern = @"{#\w+}";

        var options = RegexOptions.Multiline;

        foreach (Match m in Regex.Matches(text, pattern, options))
        {
            string teg = m.Value.Remove(0, 2).Remove(m.Value.Length - 3);

            result.Add(teg);
        }

        return result;
    }

}
