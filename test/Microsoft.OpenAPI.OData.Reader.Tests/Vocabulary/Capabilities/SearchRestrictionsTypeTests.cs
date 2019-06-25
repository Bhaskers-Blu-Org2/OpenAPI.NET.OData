﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.OData.Edm;
using Microsoft.OpenApi.OData.Vocabulary.Capabilities;
using Xunit;

namespace Microsoft.OpenApi.OData.Reader.Vocabulary.Capabilities.Tests
{
    public class SearchRestrictionsTypeTests
    {
        [Fact]
        public void KindPropertyReturnsSearchRestrictionsEnumMember()
        {
            // Arrange & Act
            SearchRestrictionsType search = new SearchRestrictionsType();

            // Assert
           // Assert.Equal(CapabilitesTermKind.SearchRestrictions, search.Kind);
        }

        [Fact]
        public void UnknownAnnotatableTargetReturnsDefaultPropertyValues()
        {
            // Arrange
            EdmEntityType entityType = new EdmEntityType("NS", "Entity");

            //  Act
            SearchRestrictionsType search = EdmCoreModel.Instance.GetRecord<SearchRestrictionsType>(entityType);

            // Assert
            Assert.Null(search);
        }

        [Fact]
        public void AnnotatableTargetOnEntityTypeReturnsCorrectPropertyValue()
        {
            // Arrange
            const string searchAnnotation = @"
              <Annotations Target=""NS.Calendar"" >
                  <Annotation Term=""Org.OData.Capabilities.V1.SearchRestrictions"">
                    <Record>
                        <PropertyValue Property=""Searchable"" Bool=""false"" />
                        <PropertyValue Property=""UnsupportedExpressions"">
                            <EnumMember>Org.OData.Capabilities.V1.SearchExpressions/phrase</EnumMember>
                        </PropertyValue >
                    </Record>
                  </Annotation>
              </Annotations>";

            IEdmModel model = CapabilitiesModelHelper.GetModelOutline(searchAnnotation);
            IEdmEntityType calendar = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Calendar");

            // Act
            SearchRestrictionsType search = model.GetRecord<SearchRestrictionsType>(calendar);

            // Assert
            Assert.NotNull(search);
            Assert.False(search.Searchable);
            Assert.NotNull(search.UnsupportedExpressions);
            Assert.Equal(SearchExpressions.phrase, search.UnsupportedExpressions.Value);

            Assert.False(search.IsUnsupportedExpressions(SearchExpressions.AND));
            Assert.True(search.IsUnsupportedExpressions(SearchExpressions.phrase));
        }

        [Fact]
        public void AnnotatableTargetOnEntitySetReturnsCorrectPropertyValue()
        {
            // Arrange
            const string searchAnnotation = @"
              <Annotations Target=""NS.Default/Calendars"" >
                  <Annotation Term=""Org.OData.Capabilities.V1.SearchRestrictions"">
                    <Record>
                        <PropertyValue Property=""Searchable"" Bool=""false"" />
                        <PropertyValue Property=""UnsupportedExpressions"">
                            <EnumMember>Org.OData.Capabilities.V1.SearchExpressions/group</EnumMember>
                        </PropertyValue >
                    </Record>
                  </Annotation>
              </Annotations>";

            IEdmModel model = CapabilitiesModelHelper.GetModelOutline(searchAnnotation);
            IEdmEntitySet calendars = model.EntityContainer.FindEntitySet("Calendars");

            // Act
            SearchRestrictionsType search = model.GetRecord<SearchRestrictionsType>(calendars);

            // Assert
            Assert.NotNull(search);
            Assert.False(search.Searchable);
            Assert.NotNull(search.UnsupportedExpressions);
            Assert.Equal(SearchExpressions.group, search.UnsupportedExpressions.Value);

            Assert.False(search.IsUnsupportedExpressions(SearchExpressions.AND));
            Assert.True(search.IsUnsupportedExpressions(SearchExpressions.group));
        }

        [Fact]
        public void AnnotatableTargetOnEntityTypeWithUnknownEnumMemberDoesnotReturnsUnsupportedExpressions()
        {
            // Arrange
            const string searchAnnotation = @"
              <Annotations Target=""NS.Calendar"" >
                  <Annotation Term=""Org.OData.Capabilities.V1.SearchRestrictions"">
                    <Record>
                        <PropertyValue Property=""Searchable"" Bool=""false"" />
                        <PropertyValue Property=""UnsupportedExpressions"">
                            <EnumMember>Org.OData.Capabilities.V1.SearchExpressions/Unknown</EnumMember>
                        </PropertyValue >
                    </Record>
                  </Annotation>
              </Annotations>";

            IEdmModel model = CapabilitiesModelHelper.GetModelOutline(searchAnnotation);
            IEdmEntityType calendar = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Calendar");

            // Act
            SearchRestrictionsType search = model.GetRecord<SearchRestrictionsType>(calendar);

            // Assert
            Assert.NotNull(search);
            Assert.False(search.Searchable);
            Assert.Null(search.UnsupportedExpressions);
        }

        [Fact]
        public void AnnotatableTargetOnEntitySetWithUnknownEnumMemberDoesnotReturnsUnsupportedExpressions()
        {
            // Arrange
            const string searchAnnotation = @"
              <Annotations Target=""NS.Default/Calendars"" >
                  <Annotation Term=""Org.OData.Capabilities.V1.SearchRestrictions"">
                    <Record>
                        <PropertyValue Property=""Searchable"" Bool=""false"" />
                        <PropertyValue Property=""UnsupportedExpressions"">
                            <EnumMember>Org.OData.Capabilities.V1.SearchExpressions/Unknown</EnumMember>
                        </PropertyValue >
                    </Record>
                  </Annotation>
              </Annotations>";

            IEdmModel model = CapabilitiesModelHelper.GetModelOutline(searchAnnotation);
            IEdmEntitySet calendars = model.EntityContainer.FindEntitySet("Calendars");

            // Act
            SearchRestrictionsType search = model.GetRecord<SearchRestrictionsType>(calendars);

            // Assert
            Assert.NotNull(search);
            Assert.False(search.Searchable);
            Assert.Null(search.UnsupportedExpressions);
        }

        [Fact]
        public void AnnotatableTargetOnEntityTypeWithMultipleEnumMemberReturnsCorrectPropertyValue()
        {
            // Arrange
            const string searchAnnotation = @"
              <Annotations Target=""NS.Calendar"" >
                  <Annotation Term=""Org.OData.Capabilities.V1.SearchRestrictions"">
                    <Record>
                        <PropertyValue Property=""Searchable"" Bool=""false"" />
                        <PropertyValue Property=""UnsupportedExpressions"">
                            <EnumMember>Org.OData.Capabilities.V1.SearchExpressions/AND Org.OData.Capabilities.V1.SearchExpressions/OR</EnumMember>
                        </PropertyValue >
                    </Record>
                  </Annotation>
              </Annotations>";

            IEdmModel model = CapabilitiesModelHelper.GetModelOutline(searchAnnotation);
            IEdmEntityType calendar = model.SchemaElements.OfType<IEdmEntityType>().First(c => c.Name == "Calendar");

            // Act
            SearchRestrictionsType search = model.GetRecord<SearchRestrictionsType>(calendar);

            // Assert
            Assert.NotNull(search);
            Assert.False(search.Searchable);
            Assert.NotNull(search.UnsupportedExpressions);
            Assert.Equal(SearchExpressions.AND | SearchExpressions.OR, search.UnsupportedExpressions.Value);
        }

        [Fact]
        public void AnnotatableTargetOnEntitySetWithMultipleEnumMemberReturnsCorrectPropertyValue()
        {
            // Arrange
            const string searchAnnotation = @"
              <Annotations Target=""NS.Default/Calendars"" >
                  <Annotation Term=""Org.OData.Capabilities.V1.SearchRestrictions"">
                    <Record>
                        <PropertyValue Property=""Searchable"" Bool=""false"" />
                        <PropertyValue Property=""UnsupportedExpressions"">
                            <EnumMember>Org.OData.Capabilities.V1.SearchExpressions/AND Org.OData.Capabilities.V1.SearchExpressions/OR</EnumMember>
                        </PropertyValue >
                    </Record>
                  </Annotation>
              </Annotations>";

            IEdmModel model = CapabilitiesModelHelper.GetModelOutline(searchAnnotation);
            IEdmEntitySet calendars = model.EntityContainer.FindEntitySet("Calendars");

            // Act
            SearchRestrictionsType search = model.GetRecord<SearchRestrictionsType>(calendars);

            // Assert
            Assert.NotNull(search);
            Assert.False(search.Searchable);
            Assert.NotNull(search.UnsupportedExpressions);
            Assert.Equal(SearchExpressions.AND | SearchExpressions.OR, search.UnsupportedExpressions.Value);

            Assert.True(search.IsUnsupportedExpressions(SearchExpressions.AND));
            Assert.True(search.IsUnsupportedExpressions(SearchExpressions.OR));
            Assert.False(search.IsUnsupportedExpressions(SearchExpressions.group));
        }
    }
}
