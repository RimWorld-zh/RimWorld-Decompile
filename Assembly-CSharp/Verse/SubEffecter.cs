using System;

namespace Verse
{
	// Token: 0x02000F22 RID: 3874
	public class SubEffecter
	{
		// Token: 0x04003DA1 RID: 15777
		public Effecter parent;

		// Token: 0x04003DA2 RID: 15778
		public SubEffecterDef def;

		// Token: 0x06005CD8 RID: 23768 RVA: 0x001D149D File Offset: 0x001CF89D
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x06005CD9 RID: 23769 RVA: 0x001D14B4 File Offset: 0x001CF8B4
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CDA RID: 23770 RVA: 0x001D14B7 File Offset: 0x001CF8B7
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CDB RID: 23771 RVA: 0x001D14BA File Offset: 0x001CF8BA
		public virtual void SubCleanup()
		{
		}
	}
}
