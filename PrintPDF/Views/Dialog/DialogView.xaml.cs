using System.Windows;

namespace PrintPDF.Views.Dialog;

public partial class DialogView : Window, IDialogView
{
    #region Конструктор
    public DialogView()
    {
        InitializeComponent();
    }
    #endregion

    #region Реализация интерфейса
    public void Show(string message)
    {
        MessageBlock.Text = message;
        ShowDialog();
    }

    public void SetOwner(Window owner)
    {
        Owner = owner;
    }
    #endregion

    #region Закрытие окна
    private void OnDialogResultClicked(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
    #endregion
}