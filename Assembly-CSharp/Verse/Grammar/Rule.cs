using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE7 RID: 3047
	public abstract class Rule
	{
		// Token: 0x04002D7D RID: 11645
		[MayTranslate]
		public string keyword;

		// Token: 0x04002D7E RID: 11646
		public List<Rule.ConstantConstraint> constantConstraints;

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06004284 RID: 17028
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x06004285 RID: 17029
		public abstract string Generate();

		// Token: 0x06004286 RID: 17030 RVA: 0x00230DD4 File Offset: 0x0022F1D4
		public virtual void Init()
		{
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x00230DD8 File Offset: 0x0022F1D8
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

		// Token: 0x02000BE8 RID: 3048
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
