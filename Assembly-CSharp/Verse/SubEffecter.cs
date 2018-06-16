using System;

namespace Verse
{
	// Token: 0x02000F1E RID: 3870
	public class SubEffecter
	{
		// Token: 0x06005CA8 RID: 23720 RVA: 0x001D0DD9 File Offset: 0x001CF1D9
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x001D0DF0 File Offset: 0x001CF1F0
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x001D0DF3 File Offset: 0x001CF1F3
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CAB RID: 23723 RVA: 0x001D0DF6 File Offset: 0x001CF1F6
		public virtual void SubCleanup()
		{
		}

		// Token: 0x04003D85 RID: 15749
		public Effecter parent;

		// Token: 0x04003D86 RID: 15750
		public SubEffecterDef def;
	}
}
