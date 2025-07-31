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
                var messages = brokenRules.Select(rule => rule.Message);
                throw new DomainException(string.Join("; ", messages));
            }
        }

        public bool IsValid()
        {
            return !_rules.Any(rule => rule.IsBroken());
        }

        public List<string> GetBrokenRuleMessages()
        {
            return _rules.Where(rule => rule.IsBroken())
                        .Select(rule => rule.Message)
                        .ToList();
        }

        public void Clear()
        {
            _rules.Clear();
        }
    }
} 