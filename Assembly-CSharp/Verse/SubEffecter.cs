using System;

namespace Verse
{
	// Token: 0x02000F1D RID: 3869
	public class SubEffecter
	{
		// Token: 0x06005CA6 RID: 23718 RVA: 0x001D0EA1 File Offset: 0x001CF2A1
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x06005CA7 RID: 23719 RVA: 0x001D0EB8 File Offset: 0x001CF2B8
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x001D0EBB File Offset: 0x001CF2BB
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x001D0EBE File Offset: 0x001CF2BE
		public virtual void SubCleanup()
		{
		}

		// Token: 0x04003D84 RID: 15748
		public Effecter parent;

		// Token: 0x04003D85 RID: 15749
		public SubEffecterDef def;
	}
}
