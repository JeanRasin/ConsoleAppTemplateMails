namespace ConsoleAppTemplateMails;

public interface IEmailTemplate<S, B>
{
    public S Subject { get; init; }
    public B Body { get; init; }
}