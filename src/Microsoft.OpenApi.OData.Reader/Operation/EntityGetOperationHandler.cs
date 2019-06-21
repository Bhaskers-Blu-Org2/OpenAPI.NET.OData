﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.OData.Common;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.OData.Generator;
using Microsoft.OpenApi.OData.Capabilities;
using System.Linq;

namespace Microsoft.OpenApi.OData.Operation
{
    /// <summary>
    /// Retrieve an Entity:
    /// The Path Item Object for the entity contains the keyword get with an Operation Object as value
    /// that describes the capabilities for retrieving a single entity.
    /// </summary>
    internal class EntityGetOperationHandler : EntitySetOperationHandler
    {
        /// <inheritdoc/>
        public override OperationType OperationType => OperationType.Get;

        /// <inheritdoc/>
        protected override void SetBasicInfo(OpenApiOperation operation)
        {
            // Summary
            operation.Summary = "Get entity from " + EntitySet.Name + " by key";

            // OperationId
            if (Context.Settings.EnableOperationId)
            {
                string typeName = EntitySet.EntityType().Name;
                operation.OperationId = EntitySet.Name + "." + typeName + ".Get" + Utils.UpperFirstChar(typeName);
            }

            base.SetBasicInfo(operation);
        }

        /// <inheritdoc/>
        protected override void SetParameters(OpenApiOperation operation)
        {
            base.SetParameters(operation);

            // $select
            OpenApiParameter parameter = Context.CreateSelect(EntitySet);
            if (parameter != null)
            {
                operation.Parameters.Add(parameter);
            }

            // $expand
            parameter = Context.CreateExpand(EntitySet);
            if (parameter != null)
            {
                operation.Parameters.Add(parameter);
            }
        }

        /// <inheritdoc/>
        protected override void SetResponses(OpenApiOperation operation)
        {
            operation.Responses = new OpenApiResponses
            {
                {
                    Constants.StatusCode200,
                    new OpenApiResponse
                    {
                        Description = "Retrieved entity",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            {
                                Constants.ApplicationJsonMediaType,
                                new OpenApiMediaType
                                {
                                    Schema = new OpenApiSchema
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.Schema,
                                            Id = EntitySet.EntityType().FullName()
                                        }
                                    }
                                }
                            }
                        },
                        Links = Context.CreateLinks(EntitySet)
                    }
                }
            };
            operation.Responses.Add(Constants.StatusCodeDefault, Constants.StatusCodeDefault.GetResponse());

            base.SetResponses(operation);
        }

        protected override void SetSecurity(OpenApiOperation operation)
        {
            ReadRestrictions read = Context.Model.GetReadRestrictions(EntitySet);
            if (read == null)
            {
                return;
            }

            ReadRestrictionsBase readBase = read;
            if (read.ReadByKeyRestrictions != null)
            {
                readBase = read.ReadByKeyRestrictions;
            }

            if (readBase == null && readBase.Permission == null)
            {
                return;
            }

            // the Permission should be collection, however current ODL supports the single permission.
            // Will update after ODL change.
            operation.Security = Context.CreateSecurityRequirements(new[] { readBase.Permission.Scheme }).ToList();
        }

        protected override void AppendCustomParameters(OpenApiOperation operation)
        {
            ReadRestrictions read = Context.Model.GetReadRestrictions(EntitySet);
            if (read == null)
            {
                return;
            }

            ReadRestrictionsBase readBase = read;
            if (read.ReadByKeyRestrictions != null)
            {
                readBase = read.ReadByKeyRestrictions;
            }

            if (readBase.CustomQueryOptions != null)
            {
                AppendCustomParameters(operation.Parameters, readBase.CustomQueryOptions, ParameterLocation.Query);
            }

            if (readBase.CustomHeaders != null)
            {
                AppendCustomParameters(operation.Parameters, readBase.CustomHeaders, ParameterLocation.Header);
            }
        }
    }
}
