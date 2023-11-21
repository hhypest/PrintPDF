using System.Text.RegularExpressions;

namespace PrintPDF.Shell.Converter;

public static partial class NameExpression
{
    [GeneratedRegex("^(.+)\\.\\w+$")]
    public static partial Regex NameRegEx();
}