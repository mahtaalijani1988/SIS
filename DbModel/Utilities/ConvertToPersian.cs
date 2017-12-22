using Persia;

namespace DbModel.Utilities
{
    public class ConvertToPersian
    {
        public static string ConvertToPersianString(object digit)
        {
            return PersianWord.ToPersianString(digit);
        }
    }
}