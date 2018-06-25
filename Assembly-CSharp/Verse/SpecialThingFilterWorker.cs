using System;

namespace Verse
{
	// Token: 0x02000FCF RID: 4047
	public abstract class SpecialThingFilterWorker
	{
		// Token: 0x060061CD RID: 25037
		public abstract bool Matches(Thing t);

		// Token: 0x060061CE RID: 25038 RVA: 0x001D8894 File Offset: 0x001D6C94
		public virtual bool AlwaysMatches(ThingDef def)
		{
			return false;
		}

		// Token: 0x060061CF RID: 25039 RVA: 0x001D88AC File Offset: 0x001D6CAC
		public virtual bool CanEverMatch(ThingDef def)
		{
			return true;
		}
	}
}
