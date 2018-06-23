using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE5 RID: 3045
	public abstract class Rule
	{
		// Token: 0x04002D7D RID: 11645
		[MayTranslate]
		public string keyword;

		// Token: 0x04002D7E RID: 11646
		public List<Rule.ConstantConstraint> constantConstraints;

		// Token: 0x17000A77 RID: 2679
		// (get) Token: 0x06004281 RID: 17025
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x06004282 RID: 17026
		public abstract string Generate();

		// Token: 0x06004283 RID: 17027 RVA: 0x00230CF8 File Offset: 0x0022F0F8
		public virtual void Init()
		{
		}

		// Token: 0x06004284 RID: 17028 RVA: 0x00230CFC File Offset: 0x0022F0FC
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

		// Token: 0x02000BE6 RID: 3046
		public struct ConstantConstraint
		{
			// Token: 0x04002D7F RID: 11647
			[MayTranslate]
			public string key;

			// Token: 0x04002D80 RID: 11648
			[MayTranslate]
			public string value;

			// Token: 0x04002D81 RID: 11649
			public bool equality;
		}
	}
}
