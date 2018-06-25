using System;

namespace Verse
{
	// Token: 0x02000FCE RID: 4046
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x060061CD RID: 25037
		public abstract bool Matches(Thing t);

		// Token: 0x060061CE RID: 25038 RVA: 0x001D85C0 File Offset: 0x001D69C0
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x060061CF RID: 25039 RVA: 0x001D85D8 File Offset: 0x001D69D8
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
