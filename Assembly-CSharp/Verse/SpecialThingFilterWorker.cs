using System;

namespace Verse
{
	// Token: 0x02000FCA RID: 4042
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x060061BD RID: 25021
		public abstract bool Matches(Thing t);

		// Token: 0x060061BE RID: 25022 RVA: 0x001D8480 File Offset: 0x001D6880
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x001D8498 File Offset: 0x001D6898
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
