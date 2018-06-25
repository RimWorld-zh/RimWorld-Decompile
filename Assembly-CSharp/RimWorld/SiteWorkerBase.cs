using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D3 RID: 723
	public class SiteWorkerBase
	{
		// Token: 0x04000734 RID: 1844
		public SiteDefBase def;

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00069FC5 File Offset: 0x000683C5
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00069FC8 File Offset: 0x000683C8
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00069FE0 File Offset: 0x000683E0
		public virtual string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLabel = this.def.arrivedLetterLabel;
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = null;
			return this.def.arrivedLetter;
		}
	}
}
