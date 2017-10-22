using System.Collections.Generic;

namespace Verse.Grammar
{
	public abstract class Rule
	{
		public struct ConstrantConstraint
		{
			public string key;

			public string value;
		}

		public string keyword;

		public List<ConstrantConstraint> constantConstraints;

		public abstract float BaseSelectionWeight
		{
			get;
		}

		public abstract string Generate();

		public virtual void Init()
		{
		}

		public void AddConstantConstraint(string key, string value)
		{
			if (this.constantConstraints == null)
			{
				this.constantConstraints = new List<ConstrantConstraint>();
			}
			this.constantConstraints.Add(new ConstrantConstraint
			{
				key = key,
				value = value
			});
		}
	}
}
