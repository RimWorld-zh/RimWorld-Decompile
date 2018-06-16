using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020002B1 RID: 689
	public class NeedDef : Def
	{
		// Token: 0x06000B86 RID: 2950 RVA: 0x00067FC4 File Offset: 0x000663C4
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string e in this.<ConfigErrors>__BaseCallProxy0())
			{
				yield return e;
			}
			if (this.description.NullOrEmpty() && this.showOnNeedList)
			{
				yield return "no description";
			}
			if (this.needClass == null)
			{
				yield return "needClass is null";
			}
			if (this.needClass == typeof(Need_Seeker))
			{
				if (this.seekerRisePerHour == 0f || this.seekerFallPerHour == 0f)
				{
					yield return "seeker rise/fall rates not set";
				}
			}
			yield break;
		}

		// Token: 0x0400068B RID: 1675
		public Type needClass;

		// Token: 0x0400068C RID: 1676
		public Intelligence minIntelligence = Intelligence.Animal;

		// Token: 0x0400068D RID: 1677
		public bool colonistAndPrisonersOnly = false;

		// Token: 0x0400068E RID: 1678
		public bool colonistsOnly = false;

		// Token: 0x0400068F RID: 1679
		public bool onlyIfCausedByHediff = false;

		// Token: 0x04000690 RID: 1680
		public bool neverOnPrisoner = false;

		// Token: 0x04000691 RID: 1681
		public bool showOnNeedList = true;

		// Token: 0x04000692 RID: 1682
		public float baseLevel = 0.5f;

		// Token: 0x04000693 RID: 1683
		public bool major = false;

		// Token: 0x04000694 RID: 1684
		public int listPriority = 0;

		// Token: 0x04000695 RID: 1685
		[NoTranslate]
		public string tutorHighlightTag = null;

		// Token: 0x04000696 RID: 1686
		public bool showForCaravanMembers = false;

		// Token: 0x04000697 RID: 1687
		public bool scaleBar = false;

		// Token: 0x04000698 RID: 1688
		public float fallPerDay = 0.5f;

		// Token: 0x04000699 RID: 1689
		public float seekerRisePerHour = 0f;

		// Token: 0x0400069A RID: 1690
		public float seekerFallPerHour = 0f;

		// Token: 0x0400069B RID: 1691
		public bool freezeWhileSleeping = false;
	}
}
