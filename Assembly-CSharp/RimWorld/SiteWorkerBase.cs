using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D1 RID: 721
	public class SiteWorkerBase
	{
		// Token: 0x06000BF1 RID: 3057 RVA: 0x00069E11 File Offset: 0x00068211
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06000BF2 RID: 3058 RVA: 0x00069E14 File Offset: 0x00068214
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		// Token: 0x06000BF3 RID: 3059 RVA: 0x00069E2C File Offset: 0x0006822C
		public virtual string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLabel = this.def.arrivedLetterLabel;
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = null;
			return this.def.arrivedLetter;
		}

		// Token: 0x04000733 RID: 1843
		public SiteDefBase def;
	}
}
