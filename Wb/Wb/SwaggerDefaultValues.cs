using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Wb
{
    public class SwaggerDefaultValues
    {

    }
  [ AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SwaggerDefaultValue : Attribute
    {
        public string ParameterName { get; set; }
        public string Value { get; set; }

        public SwaggerDefaultValue(string parameterName, string value)
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }
    }

    public class AddDefaultValues : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                return;
            foreach (var param in operation.parameters)
            {
                var actionParam = apiDescription.ActionDescriptor.GetParameters().First(p => p.ParameterName == param.name);

                if (actionParam != null)
                {
                    var customAttribute = actionParam.ActionDescriptor.GetCustomAttributes<SwaggerDefaultValue>().FirstOrDefault(p => p.ParameterName == param.name  );
                    if (customAttribute != null)
                    {
                        param.@default = customAttribute.Value;
                        
                    }
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SwaggerEnumValue : Attribute
    {
        public string ParameterName { get; set; }
        public string Value { get; set; }

        public SwaggerEnumValue(string parameterName, string value)
        {
            this.ParameterName = parameterName;
            this.Value = value;
        }
    }

    public class AddEnumValues : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                return;
            foreach (var param in operation.parameters)
            {
                var actionParam = apiDescription.ActionDescriptor.GetParameters().First(p => p.ParameterName == param.name);

                if (actionParam != null)
                {
                    var customAttribute = actionParam.ActionDescriptor.GetCustomAttributes<SwaggerEnumValue>().Where(p => p.ParameterName == param.name);
                    if (customAttribute != null)
                    {
                        foreach (SwaggerEnumValue e in customAttribute)
                        {
                            if (param.@enum == null)
                            {
                                param.@enum = new List<object>();
                            }
                           param.@enum.Add(e.Value);  
                        }
                        
                        

                    }
                }
            }
        }
    }
}