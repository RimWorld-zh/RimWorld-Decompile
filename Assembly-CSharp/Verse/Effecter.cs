using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F21 RID: 3873
	public class Effecter
	{
		// Token: 0x04003D9F RID: 15775
		public EffecterDef def;

		// Token: 0x04003DA0 RID: 15776
		public List<SubEffecter> children = new List<SubEffecter>();

		// Token: 0x06005CD4 RID: 23764 RVA: 0x002F1B3C File Offset: 0x002EFF3C
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x06005CD5 RID: 23765 RVA: 0x002F1BA0 File Offset: 0x002EFFA0
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x06005CD6 RID: 23766 RVA: 0x002F1BE0 File Offset: 0x002EFFE0
		public void Trigger(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B);
			}
		}

		// Token: 0x06005CD7 RID: 23767 RVA: 0x002F1C20 File Offset: 0x002F0020
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}
	}
}
