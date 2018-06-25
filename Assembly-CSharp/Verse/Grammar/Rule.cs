using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE8 RID: 3048
	public abstract class Rule
	{
		// Token: 0x04002D84 RID: 11652
		[MayTranslate]
		public string keyword;

		// Token: 0x04002D85 RID: 11653
		public List<Rule.ConstantConstraint> constantConstraints;

		// Token: 0x17000A76 RID: 2678
		// (get) Token: 0x06004284 RID: 17028
		public abstract float BaseSelectionWeight { get; }

		// Token: 0x06004285 RID: 17029
		public abstract string Generate();

		// Token: 0x06004286 RID: 17030 RVA: 0x002310B4 File Offset: 0x0022F4B4
		public virtual void Init()
		{
		}

		// Token: 0x06004287 RID: 17031 RVA: 0x002310B8 File Offset: 0x0022F4B8
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

		// Token: 0x02000BE9 RID: 3049
		public struct ConstantConstraint
		{
			// Token: 0x04002D86 RID: 11654
			[MayTranslate]
			public string key;

			// Token: 0x04002D87 RID: 11655
			[MayTranslate]
			public string value;

			// Token: 0x04002D88 RID: 11656
			public bool equality;
		}
	}
}
