﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Xunit;

namespace Microsoft.OpenApi.OData.Reader.Capabilities.Tests
{
#if false
    public class TopSupportedTests
    {
        [Fact]
        public void KindPropertyReturnsTopSupportedEnumMember()
        {
            // Arrange & Act
            TopSupported top = new TopSupported();

            // Assert
            Assert.Equal(CapabilitesTermKind.TopSupported, top.Kind);
        }

        [Fact]
        public void UnknownAnnotatableTargetReturnsDefaultTopSupportedValues()
        {
            // Arrange
            TopSupported top = new TopSupported();
            EdmEntityType entityType = new EdmEntityType("NS", "Entity");

            //  Act
            bool result = top.Load(EdmCoreModel.Instance, entityType);

            // Assert
            Assert.False(result);
            Assert.True(top.IsSupported);
            Assert.Null(top.Supported);
        }

        [Theory]
        [InlineData(EdmVocabularyAnnotationSerializationLocation.Inline)]
        [InlineData(EdmVocabularyAnnotationSerializationLocation.OutOfLine)]
        public void TargetOnEntityTypeReturnsCorrectTopSupportedValue(EdmVocabularyAnnotationSerializationLocation location)
        {
            // Arrange
            const string template = @"
                <Annotations Target=""NS.Calendar"">
                  {0}
                </Annotations>";

            IEdmModel model = GetEdmModel(template, location);
            Assert.NotNull(model); // guard

            IEdmEntityType calendar = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Calendar");
            Assert.NotNull(calendar); // guard

            // Act
            TopSupported top = new TopSupported();
            bool result = top.Load(model, calendar);

            // Assert
            Assert.True(result);
            Assert.False(top.IsSupported);
            Assert.NotNull(top.Supported);
            Assert.False(top.Supported.Value);
        }

        [Theory]
        [InlineData(EdmVocabularyAnnotationSerializationLocation.Inline)]
        [InlineData(EdmVocabularyAnnotationSerializationLocation.OutOfLine)]
        public void TargetOnEntitySetReturnsCorrectTopSupportedValue(EdmVocabularyAnnotationSerializationLocation location)
        {
            // Arrange
            const string template = @"
                <Annotations Target=""NS.Default/Calendars"">
                  {0}
                </Annotations>";

            IEdmModel model = GetEdmModel(template, location);
            Assert.NotNull(model); // guard

            IEdmEntitySet calendars = model.EntityContainer.FindEntitySet("Calendars");
            Assert.NotNull(calendars); // guard

            // Act
            TopSupported top = new TopSupported();
            bool result = top.Load(model, calendars);

            // Assert
            Assert.True(result);
            Assert.False(top.IsSupported);
            Assert.NotNull(top.Supported);
            Assert.False(top.Supported.Value);
        }

        private static IEdmModel GetEdmModel(string template, EdmVocabularyAnnotationSerializationLocation location)
        {
            string topAnnotation = @"<Annotation Term=""Org.OData.Capabilities.V1.TopSupported"" Bool=""false"" />";

            if (location == EdmVocabularyAnnotationSerializationLocation.OutOfLine)
            {
                topAnnotation = string.Format(template, topAnnotation);
                return CapabilitiesModelHelper.GetEdmModelOutline(topAnnotation);
            }
            else
            {
                return CapabilitiesModelHelper.GetEdmModelTypeInline(topAnnotation);
            }
        }
    }
#endif
}
