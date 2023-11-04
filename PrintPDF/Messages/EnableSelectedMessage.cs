using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PrintPDF.Messages;

public sealed class EnableSelectedMessage : ValueChangedMessage<bool>
{
    public EnableSelectedMessage(bool value) : base(value)
    {
    }
}