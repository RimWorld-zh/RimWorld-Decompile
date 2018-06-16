using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE9 RID: 3049
	public abstract class Rule
	{
		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x0600427D RID: 17021
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x0600427E RID: 17022
		public abstract string Generate();

		// Token: 0x0600427F RID: 17023 RVA: 0x002303CC File Offset: 0x0022E7CC
		public virtual void Init()
		{
		}

		// Token: 0x06004280 RID: 17024 RVA: 0x002303D0 File Offset: 0x0022E7D0
		public void AddConstantConstraint(string key, string value, bool equality)
		{
			if (this.constantConstraints == null)
			{
				this.constantConstraints = new List<Rule.ConstrantConstraint>();
			}
			this.constantConstraints.Add(new Rule.ConstrantConstraint
			{
				key = key,
				value = value,
				equality = equality
			});
		}

		// Token: 0x04002D77 RID: 11639
		[NoTranslate]
		public string keyword;

		// Token: 0x04002D78 RID: 11640
		public List<Rule.ConstrantConstraint> constantConstraints;

		// Token: 0x02000BEA RID: 3050
		public struct ConstrantConstraint
		{
			// Token: 0x04002D79 RID: 11641
			[NoTranslate]
			public string key;

			// Token: 0x04002D7A RID: 11642
			[NoTranslate]
			public string value;

			// Token: 0x04002D7B RID: 11643
			public bool equality;
		}
	}
}
