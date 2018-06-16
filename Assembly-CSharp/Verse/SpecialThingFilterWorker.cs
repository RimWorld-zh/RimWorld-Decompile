using System;

namespace Verse
{
	// Token: 0x02000FCA RID: 4042
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x06006196 RID: 24982
		public abstract bool Matches(Thing t);

		// Token: 0x06006197 RID: 24983 RVA: 0x001D81B0 File Offset: 0x001D65B0
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x06006198 RID: 24984 RVA: 0x001D81C8 File Offset: 0x001D65C8
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
