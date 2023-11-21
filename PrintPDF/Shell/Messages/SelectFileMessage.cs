using CommunityToolkit.Mvvm.Messaging.Messages;

namespace PrintPDF.Shell.Messages;

public sealed class SelectFileMessage(bool value) : ValueChangedMessage<bool>(value);