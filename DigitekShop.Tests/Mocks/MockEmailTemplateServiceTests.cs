using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace DigitekShop.Tests.Mocks
{
    public class MockEmailTemplateServiceTests
    {
        [Fact]
        public async Task GetTemplateContentAsync_ExistingTemplate_ReturnsTemplateContent()
        {
            // Arrange
            var mockService = new MockEmailTemplateService();

            // Act
            string result = await mockService.GetTemplateContentAsync("WelcomeEmail");

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Welcome {{FirstName}}", result);
        }

        [Fact]
        public async Task GetTemplateContentAsync_NonExistingTemplate_ThrowsKeyNotFoundException()
        {
            // Arrange
            var mockService = new MockEmailTemplateService();

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => mockService.GetTemplateContentAsync("NonExistingTemplate"));
        }

        [Fact]
        public async Task GetProcessedTemplateAsync_WithReplacements_ReplacesPlaceholders()
        {
            // Arrange
            var mockService = new MockEmailTemplateService();
            var replacements = new Dictionary<string, string>
            {
                { "FirstName", "John" },
                { "Email", "john@example.com" }
            };

            // Act
            string result = await mockService.GetProcessedTemplateAsync("WelcomeEmail", replacements);

            // Assert
            Assert.NotNull(result);
            Assert.Contains("Welcome John", result);
            Assert.Contains("john@example.com", result);
        }

        [Fact]
        public async Task GetProcessedTemplateAsync_WithCommonReplacements_ReplacesCommonPlaceholders()
        {
            // Arrange
            var mockService = new MockEmailTemplateService(new Dictionary<string, string>
            {
                { "TestTemplate", "<html><body>{{CompanyName}} - {{CurrentYear}}</body></html>" }
            });

            // Act
            string result = await mockService.GetProcessedTemplateAsync("TestTemplate", new Dictionary<string, string>());

            // Assert
            Assert.NotNull(result);
            Assert.Contains("DigitekShop", result);
            Assert.Contains(DateTime.Now.Year.ToString(), result);
        }

        [Fact]
        public async Task ClearCache_RemovesTemplatesFromCache()
        {
            // Arrange
            var mockService = new MockEmailTemplateService();
            
            // First call to cache the template
            await mockService.GetTemplateContentAsync("WelcomeEmail");
            
            // Add a new template that would replace the cached one
            mockService.AddOrUpdateTemplate("WelcomeEmail", "<html><body>New Welcome Template</body></html>");
            
            // Act - Get the template again without clearing cache (should return cached version)
            string resultBeforeClear = await mockService.GetTemplateContentAsync("WelcomeEmail");
            
            // Clear cache
            mockService.ClearCache();
            
            // Get the template again after clearing cache
            string resultAfterClear = await mockService.GetTemplateContentAsync("WelcomeEmail");

            // Assert
            Assert.Contains("Welcome {{FirstName}}", resultBeforeClear); // Should return original cached version
            Assert.Contains("New Welcome Template", resultAfterClear); // Should return new version after cache clear
        }

        [Fact]
        public async Task AddOrUpdateTemplate_AddsNewTemplate()
        {
            // Arrange
            var mockService = new MockEmailTemplateService();
            string templateName = "NewTemplate";
            string templateContent = "<html><body>New Template Content</body></html>";

            // Act
            mockService.AddOrUpdateTemplate(templateName, templateContent);
            string result = await mockService.GetTemplateContentAsync(templateName);

            // Assert
            Assert.Equal(templateContent, result);
        }

        [Fact]
        public async Task AddOrUpdateTemplate_UpdatesExistingTemplate()
        {
            // Arrange
            var mockService = new MockEmailTemplateService();
            string templateName = "WelcomeEmail";
            string newTemplateContent = "<html><body>Updated Welcome Template</body></html>";

            // Act
            mockService.AddOrUpdateTemplate(templateName, newTemplateContent);
            string result = await mockService.GetTemplateContentAsync(templateName);

            // Assert
            Assert.Equal(newTemplateContent, result);
        }
    }
}