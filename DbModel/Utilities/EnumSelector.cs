using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DbModel.Utilities
{
    public static class EnumSelector
    {
        public static string GetEnumValues(Type enum1, object val)
        {
            //val = "1,2,4";
            string res = "";
            if (val == null)
                return "";
            else if (!string.IsNullOrEmpty(val.ToString()))
            {
                if (val.ToString().Contains(","))
                {
                    string[] v = val.ToString().Split(',');
                    foreach (string cc in v)
                    {
                        if (!string.IsNullOrEmpty(cc))
                            res += Enum.GetName(enum1, int.Parse(cc));
                    }
                }
                else
                    res = Enum.GetName(enum1, int.Parse(val.ToString()));
            }
            return res;
        }
        public static List<string> GetEnumValuesList(Type enum1, string val)
        {
            //val = "1,2,4";
            List<string> res = new List<string>();
            if (!string.IsNullOrEmpty(val))
            {
                if (val.Contains(","))
                {
                    string[] v = val.Split(',');
                    foreach (string cc in v)
                    {
                        if (!string.IsNullOrEmpty(cc))
                            res.Add(Enum.GetName(enum1, int.Parse(cc)));
                    }
                }
                else
                    res.Add(Enum.GetName(enum1, int.Parse(val)));
            }
            return res;
        }
        public static IEnumerable<SelectListItem> GetEnumItems(
             this Type enumType, object selectedValue)
        {
            //selectedValue = "1,2,4";
            var names = Enum.GetNames(enumType);
            var values = Enum.GetValues(enumType).Cast<int>();
            
            if (selectedValue != null && !string.IsNullOrEmpty(selectedValue.ToString()))
            {
                if (selectedValue.ToString().Contains(","))
                {
                    IList<string> selectedvals = selectedValue.ToString().Split(',').ToList();
                    return names.Zip(values, (name, value) =>
                                new SelectListItem
                                {
                                    Text = GetName(enumType, name),
                                    Value = value.ToString(),
                                    Selected = selectedvals.Contains(value.ToString())//value == int.Parse(c)
                                });
                }
                else
                {
                    return names.Zip(values, (name, value) =>
                                new SelectListItem
                                {
                                    Text = GetName(enumType, name),
                                    Value = value.ToString(),
                                    Selected = value == int.Parse(selectedValue.ToString())
                                });
                }
            }
            else
                return names.Zip(values, (name, value) =>
                                new SelectListItem
                                {
                                    Text = GetName(enumType, name),
                                    Value = value.ToString(),
                                    Selected = false
                                });
        }
        static string GetName(Type enumType, string name)
        {
            var result = name;

            var attribute = enumType
                .GetField(name)
                .GetCustomAttributes(inherit: false)
                .OfType<DisplayAttribute>()
                .FirstOrDefault();

            if (attribute != null)
            {
                result = attribute.GetName();
            }

            return result;
        }


    }
}