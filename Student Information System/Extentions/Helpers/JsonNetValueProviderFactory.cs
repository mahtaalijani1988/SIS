﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Student_Information_System.Extentions.Helpers
{
    public class JsonNetValueProviderFactory : ValueProviderFactory
    {
        public JsonNetValueProviderFactory()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                Converters = { new ExpandoObjectConverter() }
            };
        }

        public JsonSerializerSettings Settings { get; set; }

        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (controllerContext.HttpContext == null ||
                controllerContext.HttpContext.Request == null ||
                controllerContext.HttpContext.Request.ContentType == null)
            {
                return null;
            }

            if (!controllerContext.HttpContext.Request.ContentType.StartsWith(
                    "application/json", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (!controllerContext.HttpContext.Request.IsAjaxRequest())
            {
                return null;
            }
            
            using (var streamReader = new StreamReader(controllerContext.HttpContext.Request.InputStream))
            {
                using (var jsonReader = new JsonTextReader(streamReader))
                {
                    if (!jsonReader.Read())
                        return null;

                    var jsonSerializer = JsonSerializer.Create(this.Settings);

                    Object jsonObject;
                    switch (jsonReader.TokenType)
                    {
                        case JsonToken.StartArray:
                            jsonObject = jsonSerializer.Deserialize<List<ExpandoObject>>(jsonReader);
                            break;
                        default:
                            jsonObject = jsonSerializer.Deserialize<ExpandoObject>(jsonReader);
                            break;
                    }

                    var backingStore = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                    addToBackingStore(backingStore, String.Empty, jsonObject);
                    return new DictionaryValueProvider<object>(backingStore, CultureInfo.CurrentCulture);
                }
            }
        }

        private static void addToBackingStore(IDictionary<string, object> backingStore, string prefix, object value)
        {
            var dictionary = value as IDictionary<string, object>;
            if (dictionary != null)
            {
                foreach (var entry in dictionary)
                {
                    addToBackingStore(backingStore, makePropertyKey(prefix, entry.Key), entry.Value);
                }
                return;
            }

            var list = value as IList;
            if (list != null)
            {
                for (var index = 0; index < list.Count; index++)
                {
                    addToBackingStore(backingStore, makeArrayKey(prefix, index), list[index]);
                }
                return;
            }

            backingStore[prefix] = value;
        }

        private static string makeArrayKey(string prefix, int index)
        {
            return prefix + "[" + index.ToString(CultureInfo.InvariantCulture) + "]";
        }

        private static string makePropertyKey(string prefix, string propertyName)
        {
            return (string.IsNullOrWhiteSpace(prefix)) ? propertyName : prefix + "." + propertyName;
        }
    }
}