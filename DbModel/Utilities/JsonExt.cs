using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace DbModel.Utilities
{
    public static class JsonExt
    {
        public static string ToJson(this string[] initialTags)
        {
            if (initialTags == null || !initialTags.Any())
                return "[]";
            else
                return new JavaScriptSerializer().Serialize(initialTags);
        }
    }
}
