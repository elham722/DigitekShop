using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigitekShop.Application.Interfaces.Infrastructure;

namespace DigitekShop.Tests.Mocks
{
    /// <summary>
    /// Mock implementation of IEmailTemplateService for testing purposes
    /// </summary>
    public class MockEmailTemplateService : IEmailTemplateService
    {
        private readonly Dictionary<string, string> _templates;
        private readonly Dictionary<string, string> _cachedTemplates = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the MockEmailTemplateService class
        /// </summary>
        /// <param name="templates">Dictionary of template names and their content</param>
        public MockEmailTemplateService(Dictionary<string, string> templates = null)
        {
            _templates = templates ?? new Dictionary<string, string>
            {
                { "WelcomeEmail", "<html><body>Welcome {{FirstName}}! Your email is {{Email}}.</body></html>" },
                { "OrderConfirmation", "<html><body>Order #{{OrderNumber}} confirmed for {{CustomerName}}.</body></html>" }
            };
        }

        /// <summary>
        /// Gets an email template and replaces placeholders with actual values
        /// </summary>
        /// <param name="templateName">Name of the template</param>
        /// <param name="replacements">Dictionary of placeholder-value pairs</param>
        /// <returns>Processed template content</returns>
        public Task<string> GetProcessedTemplateAsync(string templateName, Dictionary<string, string> replacements)
        {
            string templateContent = GetTemplateContentAsync(templateName).Result;
            string processedContent = ProcessTemplate(templateContent, replacements);
            return Task.FromResult(processedContent);
        }

        /// <summary>
        /// Gets the raw content of an email template
        /// </summary>
        /// <param name="templateName">Name of the template</param>
        /// <returns>Raw template content</returns>
        public Task<string> GetTemplateContentAsync(string templateName)
        {
            // Check if template is already cached
            if (_cachedTemplates.TryGetValue(templateName, out string cachedTemplate))
            {
                return Task.FromResult(cachedTemplate);
            }

            // Check if template exists
            if (!_templates.TryGetValue(templateName, out string templateContent))
            {
                throw new KeyNotFoundException($"Template '{templateName}' not found");
            }

            // Cache the template
            _cachedTemplates[templateName] = templateContent;

            return Task.FromResult(templateContent);
        }

        /// <summary>
        /// Clears the template cache
        /// </summary>
        public void ClearCache()
        {
            _cachedTemplates.Clear();
        }

        /// <summary>
        /// Adds or updates a template
        /// </summary>
        /// <param name="templateName">Name of the template</param>
        /// <param name="templateContent">Content of the template</param>
        public void AddOrUpdateTemplate(string templateName, string templateContent)
        {
            _templates[templateName] = templateContent;
            _cachedTemplates.Remove(templateName); // Remove from cache if exists
        }

        /// <summary>
        /// Processes a template by replacing placeholders with actual values
        /// </summary>
        /// <param name="templateContent">Raw template content</param>
        /// <param name="replacements">Dictionary of placeholder-value pairs</param>
        /// <returns>Processed template content</returns>
        private string ProcessTemplate(string templateContent, Dictionary<string, string> replacements)
        {
            if (replacements == null || replacements.Count == 0)
            {
                return templateContent;
            }

            string processedContent = templateContent;

            // Replace all placeholders with their values
            foreach (var replacement in replacements)
            {
                processedContent = processedContent.Replace($"{{{{{replacement.Key}}}}}", replacement.Value);
            }

            // Add common replacements
            processedContent = processedContent.Replace("{{CompanyName}}", "DigitekShop");
            processedContent = processedContent.Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

            return processedContent;
        }
    }
}