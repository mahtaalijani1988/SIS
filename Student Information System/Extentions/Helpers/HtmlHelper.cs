using System;
using System.Web.Mvc;
using Persia;
using DbModel.Utilities;

namespace Student_Information_System.Extentions.Helpers
{
    public static class ConverterToPersian
    {
        public static MvcHtmlString ConvertToPersianString(this HtmlHelper htmlHelper, int digit)
        {
            return MvcHtmlString.Create(PersianWord.ToPersianString(digit));
        }

        public static MvcHtmlString ConvertToPersianString(this HtmlHelper htmlHelper, string str)
        {
            return MvcHtmlString.Create(PersianWord.ToPersianString(str));
        }

        public static MvcHtmlString ConvertToPersianDateTime(this HtmlHelper htmlHelper, DateTime dateTime,
            string mode = "")
        {
            return dateTime.Year == 1 ? null : MvcHtmlString.Create(DateAndTime.ConvertToPersian(dateTime, mode));
        }

        public static MvcHtmlString ConvertToShortDateTime(this HtmlHelper htmlHelper, DateTime? dateTime)
        {
            return !dateTime.HasValue ? null : MvcHtmlString.Create(dateTime.Value.ToShortDateString());
        }

        public static string ConvertBooleanToPersian(this HtmlHelper htmlHelper, bool? value)
        {
            return !Convert.ToBoolean(value) ? "آزاد" : "مسدود";
        }

        public static string ConvertGender(this HtmlHelper htmlHelper, bool? value)
        {
            return !Convert.ToBoolean(value) ? "Female" : "Male";
        }

        public static string ConvertBooleanToPersian(this HtmlHelper htmlHelper, bool value)
        {
            return !Convert.ToBoolean(value) ? "آزاد" : "مسدود";
        }
    }
}