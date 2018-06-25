using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F20 RID: 3872
	public class Effecter
	{
		// Token: 0x04003D97 RID: 15767
		public EffecterDef def;

		// Token: 0x04003D98 RID: 15768
		public List<SubEffecter> children = new List<SubEffecter>();

		// Token: 0x06005CD4 RID: 23764 RVA: 0x002F191C File Offset: 0x002EFD1C
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x06005CD5 RID: 23765 RVA: 0x002F1980 File Offset: 0x002EFD80
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x002F19C0 File Offset: 0x002EFDC0
		public void Trigger(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B);
			}
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x002F1A00 File Offset: 0x002EFE00
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}
	}
}
