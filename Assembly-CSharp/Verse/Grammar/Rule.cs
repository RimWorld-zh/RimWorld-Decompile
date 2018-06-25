using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	public abstract class Rule
	{
		[MayTranslate]
		public string keyword;

		public List<Rule.ConstantConstraint> constantConstraints;

		protected Rule()
		{
		}

		public abstract float BaseSelectionWeight { get; }

		public abstract string Generate();

		public virtual void Init()
		{
		}

		public void AddConstantConstraint(string key, string value, bool equality)
		{
			if (this.constantConstraints == null)
			{
				this.constantConstraints = new List<Rule.ConstantConstraint>();
			}
			this.constantConstraints.Add(new Rule.ConstantConstraint
			{
				key = key,
				value = value,
				equality = equality
			});
		}

		public struct ConstantConstraint
		{
			[MayTranslate]
			public string key;

			[MayTranslate]
			public string value;

			public bool equality;
		}
	}
}
