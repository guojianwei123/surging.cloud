﻿using Surging.Core.CPlatform.Filters.Implementation;
using Surging.Core.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Surging.Core.Swagger.Swagger.Filters
{
    public class AddAuthorizationOperationFilter : IOperationFilter
    {

        public AddAuthorizationOperationFilter()
        {
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            if (context.ServiceEntry.Descriptor.GetMetadata<bool>("IsTokenPoint")) 
            {
                operation.Parameters.Add(new BodyParameter
                {
                    Name = "x-terminal",
                    In = "header",
                    Required = false,
                    Schema = new Schema
                    {
                        Type = "string"
                    }
                });
            }

            var attribute =
                 context.ServiceEntry.Attributes.Where(p => p is AuthorizationAttribute)
                 .Select(p => p as AuthorizationAttribute).FirstOrDefault();
            if ((attribute != null && attribute.AuthType == AuthorizationType.JWT) || (context.ServiceEntry.Descriptor.GetMetadata<bool>("EnableAuthorization") && context.ServiceEntry.Descriptor.GetMetadata<AuthorizationType>("AuthType") == AuthorizationType.JWT))
            {
                operation.Parameters.Add(new BodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Required = false,
                    Schema = new Schema
                    {
                        Type = "string"
                    }
                });
            }
            else if ((attribute != null && attribute.AuthType == AuthorizationType.AppSecret) || (context.ServiceEntry.Descriptor.GetMetadata<bool>("EnableAuthorization") && context.ServiceEntry.Descriptor.GetMetadata<AuthorizationType>("AuthType") == AuthorizationType.AppSecret))
            {
                operation.Parameters.Add(new BodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Required = false,
                    Schema = new Schema
                    {
                        Type = "string"
                    }
                });
                operation.Parameters.Add(new BodyParameter
                {
                    Name = "timeStamp",
                    In = "query",
                    Required = false,
                    Schema = new Schema
                    {
                        Type = "string"
                    }
                });
            }
        }
    }
}