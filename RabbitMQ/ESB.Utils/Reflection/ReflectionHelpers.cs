using EasyNetQ.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ESB.Utils.Serializers;

namespace ESB.Utils.Reflection
{
    public static class ReflectionHelpers
    {
        //public static Response ExecuteMethod(this LogisticMethod dynamicMethod)
        //{
        //    Response response = new Response();
        //    try
        //    {
        //        object result = null;

        //        Assembly assembly = null;
        //        assembly = Assembly.Load(dynamicMethod.Library);

        //        Type type = assembly.GetType(dynamicMethod.Namespace);

        //        MethodInfo methodInfo = type.GetMethod(dynamicMethod.Method);

        //        object instance = Activator.CreateInstance(type, null);
        //        List<object> parameters = new List<object>();
        //        parameters.Add(dynamicMethod.JSON);
        //        if (methodInfo.GetParameters().Length>0)
        //        {
        //            result = methodInfo.Invoke(instance, parameters.ToArray());
        //        }
        //        else
        //        {
        //            result = methodInfo.Invoke(instance,null);
        //        }
                

        //        response.JsonResponse = result.ToJSON();
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Error = ex.Message;
        //    }
        //    return response;
        //}
    }
}
