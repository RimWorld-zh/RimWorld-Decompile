using System;

namespace Verse
{
	// Token: 0x02000F1D RID: 3869
	public class SubEffecter
	{
		// Token: 0x04003D96 RID: 15766
		public Effecter parent;

		// Token: 0x04003D97 RID: 15767
		public SubEffecterDef def;

		// Token: 0x06005CCE RID: 23758 RVA: 0x001D1089 File Offset: 0x001CF489
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x06005CCF RID: 23759 RVA: 0x001D10A0 File Offset: 0x001CF4A0
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CD0 RID: 23760 RVA: 0x001D10A3 File Offset: 0x001CF4A3
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CD1 RID: 23761 RVA: 0x001D10A6 File Offset: 0x001CF4A6
		public virtual void SubCleanup()
		{
		}
	}
}
