using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Student_Information_System.Extentions.Helpers
{
    public static class ScoreValues
    {
        public static MvcHtmlString ConvertToScoreToGrade(this System.Web.Mvc.HtmlHelper htmlHelper, decimal? d)
        {
            string res = "";
            if (d.HasValue)
            {
                if (d.Value >= 0 && d.Value <= 49)
                {
                    res = "F";
                }
                else if (d.Value >= 50 && d.Value <= 52)
                {
                    res = "D-";
                }
                else if (d.Value >= 53 && d.Value <= 56)
                {
                    res = "D";
                }
                else if (d.Value >= 57 && d.Value <= 59)
                {
                    res = "D+";
                }
                else if (d.Value >= 60 && d.Value <= 62)
                {
                    res = "C-";
                }
                else if (d.Value >= 63 && d.Value <= 66)
                {
                    res = "C";
                }
                else if (d.Value >= 67 && d.Value <= 69)
                {
                    res = "C+";
                }
                else if (d.Value >= 70 && d.Value <= 72)
                {
                    res = "B-";
                }
                else if (d.Value >= 73 && d.Value <= 76)
                {
                    res = "B";
                }
                else if (d.Value >= 77 && d.Value <= 79)
                {
                    res = "B+";
                }
                else if (d.Value >= 80 && d.Value <= 84)
                {
                    res = "A-";
                }
                else if (d.Value >= 85 && d.Value <= 89)
                {
                    res = "A";
                }
                else if (d.Value >= 90 && d.Value <= 100)
                {
                    res = "A+";
                }
            }
            return MvcHtmlString.Create(res);
        }
        public static MvcHtmlString ConvertToScoreTodrs(this System.Web.Mvc.HtmlHelper htmlHelper, decimal? d)
        {
            string ret = "";
            if (d.HasValue)
            {
                switch (d.Value.ToString())
                {
                    case "4.30": ret = "A+"; break;
                    case "4.00": ret = "A"; break;
                    case "3.70": ret = "A-"; break;
                    case "3.30": ret = "B+"; break;
                    case "3.00": ret = "B"; break;
                    case "2.70": ret = "B-"; break;
                    case "2.30": ret = "C+"; break;
                    case "2.00": ret = "C"; break;
                    case "1.70": ret = "C-"; break;
                    case "1.30": ret = "D+"; break;
                    case "1.00": ret = "D"; break;
                    case "0.70": ret = "D-"; break;
                    case "0": ret = "F"; break;
                    default: ret = ""; break;
                }
            }
            return MvcHtmlString.Create(ret);
        }
    }
}