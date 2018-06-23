using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020002D1 RID: 721
	public class SiteWorkerBase
	{
		// Token: 0x04000732 RID: 1842
		public SiteDefBase def;

		// Token: 0x06000BEF RID: 3055 RVA: 0x00069E79 File Offset: 0x00068279
		public virtual void PostMapGenerate(Map map)
		{
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x00069E7C File Offset: 0x0006827C
		public virtual bool FactionCanOwn(Faction faction)
		{
			return true;
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x00069E94 File Offset: 0x00068294
		public virtual string GetArrivedLetterPart(Map map, out string preferredLabel, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			preferredLabel = this.def.arrivedLetterLabel;
			preferredLetterDef = this.def.arrivedLetterDef;
			lookTargets = null;
			return this.def.arrivedLetter;
		}
	}
}
