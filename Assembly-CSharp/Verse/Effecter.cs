using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1C RID: 3868
	public class Effecter
	{
		// Token: 0x06005CA2 RID: 23714 RVA: 0x002EF270 File Offset: 0x002ED670
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x06005CA3 RID: 23715 RVA: 0x002EF2D4 File Offset: 0x002ED6D4
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x06005CA4 RID: 23716 RVA: 0x002EF314 File Offset: 0x002ED714
		public void Trigger(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B);
			}
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x002EF354 File Offset: 0x002ED754
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}

		// Token: 0x04003D82 RID: 15746
		public EffecterDef def;

		// Token: 0x04003D83 RID: 15747
		public List<SubEffecter> children = new List<SubEffecter>();
	}
}
