using ConsoleAppTemplateMails;

var mailTemplate = new EmailTemplate(Subject: "Обращение в техническую поддержку «{#nameCompany}»", default)
{
    Body = @"
                    <div style='font-family: Calibri; font-size: 11pt;'>
                        Здравствуйте!<br><br>
                        Пользователь {#userName} отправил сообщение в техническую поддержку в {#datetime}.<br><br>
                        Роли пользователя: {#rolesStr}.<br><br>
                        [Url: {#urlStr}]
                        [Ошибка: {#errorTemplStr}]
                        Место в системе, где произошла ошибка, во вложенном файле по дате {#datetime}.<br><br>
                        Описание проблемы:<br>{#text}
                    </div>"
};

var mail1 = new EmailCompany(default, default)
{
    Subject = new EmailCompany.EmailSubject(NameCompany : "Атлас"),
    Body = new EmailCompany.EmailBody(default, default, default, default, default, default)
    {
        UserName = "Иван",
        Date = DateTime.Now,
        RolesStr = "Администратор",
        Text = "Text"
    }
};

EmailTemplate data = EmailTempleteWork.Build(mailTemplate, mail1);

Console.WriteLine(data);
