using Microsoft.AspNetCore.Components;
using SisData.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SisCom
{
    public class Func
    {
        public static List<SisData.Model.SisType> GetAllComponent(List<System.Reflection.Assembly> ExtraAsse)
        {
            var assemblies = new List<Assembly>();
            assemblies.AddRange(ExtraAsse);
            List<SisData.Model.SisType> dictsis = new List<SisData.Model.SisType>();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(ComponentBase).IsAssignableFrom(type))
                    {
                        var routeAttributes = type.GetCustomAttributes<RouteAttribute>(true);
                        var res = routeAttributes.Min(a => a.Template);
                        if (!String.IsNullOrEmpty(res))
                        {
                            dictsis.Add(new SisData.Model.SisType() { Component = res.Replace("/", "").ToLower(), Type = type, Routes = routeAttributes.ToList() });
                        }
                    }
                }
            }
            return dictsis; 
        }
        public static bool CheckNumber(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return double.TryParse(
                input,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,out _);
        }
    }
}
