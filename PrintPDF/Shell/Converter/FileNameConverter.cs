using PrintPDF.Shell.Converter.Base;
using System.Globalization;
using System.Windows.Data;

namespace PrintPDF.Shell.Converter;

[ValueConversion(typeof(string), typeof(string))]
public sealed class FileNameConverter : BaseConverter
{
    protected override object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string name)
            return null;

        var regEx = NameExpression.NameRegEx();
        var match = regEx.Match(name);

        return match.Groups[1].Value;
    }
}