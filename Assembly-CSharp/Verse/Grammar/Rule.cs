using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE9 RID: 3049
	public abstract class Rule
	{
		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x0600427F RID: 17023
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x06004280 RID: 17024
		public abstract string Generate();

		// Token: 0x06004281 RID: 17025 RVA: 0x00230444 File Offset: 0x0022E844
		public virtual void Init()
		{
		}

		// Token: 0x06004282 RID: 17026 RVA: 0x00230448 File Offset: 0x0022E848
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
