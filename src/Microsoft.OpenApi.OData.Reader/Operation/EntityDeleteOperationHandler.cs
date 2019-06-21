﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.OData.Capabilities;
using Microsoft.OpenApi.OData.Common;
using Microsoft.OpenApi.OData.Generator;
using System.Linq;

namespace Microsoft.OpenApi.OData.Operation
{
    /// <summary>
    /// Delete an Entity
    /// The Path Item Object for the entity set contains the keyword delete with an Operation Object as value
    /// that describes the capabilities for deleting the entity.
    /// </summary>
    internal class EntityDeleteOperationHandler : EntitySetOperationHandler
    {
        /// <inheritdoc/>
        public override OperationType OperationType => OperationType.Delete;

        /// <inheritdoc/>
        protected override void SetBasicInfo(OpenApiOperation operation)
        {
            // Summary
            operation.Summary = "Delete entity from " + EntitySet.Name;

            // OperationId
            if (Context.Settings.EnableOperationId)
            {
                string typeName = EntitySet.EntityType().Name;
                operation.OperationId = EntitySet.Name + "." + typeName + ".Delete" + Utils.UpperFirstChar(typeName);
            }

            base.SetBasicInfo(operation);
        }

        /// <inheritdoc/>
        protected override void SetParameters(OpenApiOperation operation)
        {
            base.SetParameters(operation);

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "If-Match",
                In = ParameterLocation.Header,
                Description = "ETag",
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }

        /// <inheritdoc/>
        protected override void SetResponses(OpenApiOperation operation)
        {
            operation.Responses = new OpenApiResponses
            {
                { Constants.StatusCode204, Constants.StatusCode204.GetResponse() },
                { Constants.StatusCodeDefault, Constants.StatusCodeDefault.GetResponse() }
            };

            base.SetResponses(operation);
        }

        protected override void SetSecurity(OpenApiOperation operation)
        {
            DeleteRestrictions delete = Context.Model.GetDeleteRestrictions(EntitySet);
            if (delete == null || delete.Permission == null)
            {
                return;
            }

            // the Permission should be collection, however current ODL supports the single permission.
            // Will update after ODL change.
            operation.Security = Context.CreateSecurityRequirements(new[] { delete.Permission.Scheme }).ToList();
        }

        protected override void AppendCustomParameters(OpenApiOperation operation)
        {
            DeleteRestrictions delete = Context.Model.GetDeleteRestrictions(EntitySet);
            if (delete == null)
            {
                return;
            }

            if (delete.CustomQueryOptions != null)
            {
                AppendCustomParameters(operation.Parameters, delete.CustomQueryOptions, ParameterLocation.Query);
            }

            if (delete.CustomHeaders != null)
            {
                AppendCustomParameters(operation.Parameters, delete.CustomHeaders, ParameterLocation.Header);
            }
        }
    }
}
