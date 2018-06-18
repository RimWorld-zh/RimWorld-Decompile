using System;

namespace Verse
{
	// Token: 0x02000FC9 RID: 4041
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x06006194 RID: 24980
		public abstract bool Matches(Thing t);

		// Token: 0x06006195 RID: 24981 RVA: 0x001D8284 File Offset: 0x001D6684
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x06006196 RID: 24982 RVA: 0x001D829C File Offset: 0x001D669C
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
