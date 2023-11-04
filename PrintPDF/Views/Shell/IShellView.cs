namespace PrintPDF.Views.Shell;

public interface IShellView
{
    public void ShowView();

    public void SetDataContext<T>(T dataContext);
}