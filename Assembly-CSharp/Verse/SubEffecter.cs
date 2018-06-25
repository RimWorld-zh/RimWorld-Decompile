using System;

namespace Verse
{
	// Token: 0x02000F21 RID: 3873
	public class SubEffecter
	{
		// Token: 0x04003D99 RID: 15769
		public Effecter parent;

		// Token: 0x04003D9A RID: 15770
		public SubEffecterDef def;

		// Token: 0x06005CD8 RID: 23768 RVA: 0x001D11C9 File Offset: 0x001CF5C9
		public SubEffecter(SubEffecterDef subDef, Effecter parent)
		{
			this.def = subDef;
			this.parent = parent;
		}

		// Token: 0x06005CD9 RID: 23769 RVA: 0x001D11E0 File Offset: 0x001CF5E0
		public virtual void SubEffectTick(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CDA RID: 23770 RVA: 0x001D11E3 File Offset: 0x001CF5E3
		public virtual void SubTrigger(TargetInfo A, TargetInfo B)
		{
		}

		// Token: 0x06005CDB RID: 23771 RVA: 0x001D11E6 File Offset: 0x001CF5E6
		public virtual void SubCleanup()
		{
		}
	}
}
