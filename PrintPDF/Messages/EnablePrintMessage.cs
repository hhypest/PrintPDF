using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PrintPDF.Messages;

public sealed class EnablePrintMessage : ValueChangedMessage<bool>
{
    public EnablePrintMessage(bool value) : base(value)
    {
    }
}