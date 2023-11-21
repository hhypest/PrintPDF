namespace PrintPDF.Shell.Views;

public interface IShellView
{
    public void SetDataContext<T>(T dataContext);

    public void ShowView();
}