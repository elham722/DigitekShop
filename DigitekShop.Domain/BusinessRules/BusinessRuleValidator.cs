using System;
using System.Collections.Generic;
using System.Linq;
using DigitekShop.Domain.Exceptions;

namespace DigitekShop.Domain.BusinessRules
{
    public class BusinessRuleValidator
    {
        private readonly List<IBusinessRule> _rules = new List<IBusinessRule>();

        public void AddRule(IBusinessRule rule)
        {
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            _rules.Add(rule);
        }

        public void AddRules(IEnumerable<IBusinessRule> rules)
        {
            if (rules == null)
                throw new ArgumentNullException(nameof(rules));

            _rules.AddRange(rules);
        }

        public void Validate()
        {
            var brokenRules = _rules.Where(rule => rule.IsBroken()).ToList();

            if (brokenRules.Any())
            {
                var criticalRules = brokenRules.Where(r => r.IsCritical).ToList();
                var nonCriticalRules = brokenRules.Where(r => !r.IsCritical).ToList();

                var messages = new List<string>();

                if (criticalRules.Any())
                {
                    messages.AddRange(criticalRules.Select(rule => $"[CRITICAL] {rule.Message} (Code: {rule.ErrorCode})"));
                }

                if (nonCriticalRules.Any())
                {
                    messages.AddRange(nonCriticalRules.Select(rule => $"[WARNING] {rule.Message} (Code: {rule.ErrorCode})"));
                }

                var errorMessage = string.Join("; ", messages);
                
                if (criticalRules.Any())
                {
                    throw new DomainException(errorMessage);
                }
                else
                {
                    // Log warnings but don't throw exception
                    // TODO: Add logging here
                }
            }
        }

        public bool IsValid()
        {
            return !_rules.Any(rule => rule.IsBroken());
        }

        public bool IsValidForCriticalRules()
        {
            return !_rules.Where(r => r.IsCritical).Any(rule => rule.IsBroken());
        }

        public ValidationResult ValidateWithResult()
        {
            var brokenRules = _rules.Where(rule => rule.IsBroken()).ToList();
            var criticalRules = brokenRules.Where(r => r.IsCritical).ToList();
            var nonCriticalRules = brokenRules.Where(r => !r.IsCritical).ToList();

            return new ValidationResult
            {
                IsValid = !criticalRules.Any(),
                HasWarnings = nonCriticalRules.Any(),
                CriticalErrors = criticalRules.Select(r => new ValidationError
                {
                    Message = r.Message,
                    ErrorCode = r.ErrorCode,
                    RuleName = r.RuleName,
                    Priority = r.Priority
                }).ToList(),
                Warnings = nonCriticalRules.Select(r => new ValidationError
                {
                    Message = r.Message,
                    ErrorCode = r.ErrorCode,
                    RuleName = r.RuleName,
                    Priority = r.Priority
                }).ToList()
            };
        }

        public List<string> GetBrokenRuleMessages()
        {
            return _rules.Where(rule => rule.IsBroken())
                        .Select(rule => rule.Message)
                        .ToList();
        }

        public List<string> GetCriticalErrorMessages()
        {
            return _rules.Where(rule => rule.IsBroken() && rule.IsCritical)
                        .Select(rule => rule.Message)
                        .ToList();
        }

        public List<string> GetWarningMessages()
        {
            return _rules.Where(rule => rule.IsBroken() && !rule.IsCritical)
                        .Select(rule => rule.Message)
                        .ToList();
        }

        public void Clear()
        {
            _rules.Clear();
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public bool HasWarnings { get; set; }
        public List<ValidationError> CriticalErrors { get; set; } = new List<ValidationError>();
        public List<ValidationError> Warnings { get; set; } = new List<ValidationError>();

        public string GetErrorMessage()
        {
            if (!CriticalErrors.Any())
                return string.Empty;

            var messages = CriticalErrors.Select(e => $"{e.Message} (Code: {e.ErrorCode})");
            return string.Join("; ", messages);
        }

        public string GetWarningMessage()
        {
            if (!Warnings.Any())
                return string.Empty;

            var messages = Warnings.Select(e => $"{e.Message} (Code: {e.ErrorCode})");
            return string.Join("; ", messages);
        }
    }

    public class ValidationError
    {
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string RuleName { get; set; }
        public int Priority { get; set; }
    }
} 