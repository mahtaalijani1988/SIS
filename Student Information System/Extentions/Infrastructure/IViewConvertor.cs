
namespace Student_Information_System.Extentions.Infrastructure
{
    public interface IViewConvertor
    {
        string RenderRazorViewToString(string viewName, object model);
    }
}