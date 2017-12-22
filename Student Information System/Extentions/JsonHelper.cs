using System.Web.Script.Serialization;

namespace Student_Information_System.Extentions
{
    public static class JsonHelper
    {
        public static string ToJson(this object data)
        {
            return new JavaScriptSerializer().Serialize(data);
        }
    }
}