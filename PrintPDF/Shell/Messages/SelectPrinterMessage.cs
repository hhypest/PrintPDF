using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PrintPDF.Shell.Messages;

public sealed class SelectPrinterMessage(bool value) : ValueChangedMessage<bool>(value);