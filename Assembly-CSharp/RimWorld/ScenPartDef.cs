using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002CD RID: 717
	public class ScenPartDef : Def
	{
		// Token: 0x04000711 RID: 1809
		public ScenPartCategory category = ScenPartCategory.Undefined;

		// Token: 0x04000712 RID: 1810
		public Type scenPartClass = null;

		// Token: 0x04000713 RID: 1811
		public float summaryPriority = -1f;

		// Token: 0x04000714 RID: 1812
		public float selectionWeight = 1f;

		// Token: 0x04000715 RID: 1813
		public int maxUses = 999999;

		// Token: 0x04000716 RID: 1814
		public Type pageClass;

		// Token: 0x04000717 RID: 1815
		public GameConditionDef gameCondition;

		// Token: 0x04000718 RID: 1816
		public bool gameConditionTargetsWorld;

		// Token: 0x04000719 RID: 1817
		public FloatRange durationRandomRange = new FloatRange(30f, 100f);

		// Token: 0x0400071A RID: 1818
		public Type designatorType;

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x00069814 File Offset: 0x00067C14
		public bool PlayerAddRemovable
		{
			get
			{
				return this.category != ScenPartCategory.Fixed;
			}
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x00069838 File Offset: 0x00067C38
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.scenPartClass == null)
			{
				yield return "scenPartClass is null";
			}
			yield break;
		}
	}
}
