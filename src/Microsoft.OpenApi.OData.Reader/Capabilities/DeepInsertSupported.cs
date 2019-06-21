﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.OpenApi.OData.Edm;

namespace Microsoft.OpenApi.OData.Capabilities
{
    /// <summary>
    /// Org.OData.Capabilities.V1.DeepInsertSupport
    /// </summary>
    internal class DeepInsertSupported : SupportedRestrictions
    {
        /// <summary>
        /// The Term type kind.
        /// </summary>
        public override CapabilitesTermKind Kind => CapabilitesTermKind.DeepInsertSupport;

        /// <summary>
        /// Gets Annotation target supports accepting and returning nested entities annotated with the `Core.ContentID` instance annotation.
        /// </summary>
        public bool? ContentIDSupported { get; private set; }

        protected override bool Initialize(IEdmVocabularyAnnotation annotation)
        {
            if (annotation == null ||
                annotation.Value == null ||
                annotation.Value.ExpressionKind != EdmExpressionKind.Record)
            {
                return false;
            }

            IEdmRecordExpression record = (IEdmRecordExpression)annotation.Value;

            // Supported
            Supported = record.GetBoolean("Supported");

            // NonInsertableNavigationProperties
            ContentIDSupported = record.GetBoolean("ContentIDSupported");

            return true;
        }
    }
}
