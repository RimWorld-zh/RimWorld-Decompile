using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1D RID: 3869
	public class Effecter
	{
		// Token: 0x06005CA4 RID: 23716 RVA: 0x002EF194 File Offset: 0x002ED594
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x06005CA5 RID: 23717 RVA: 0x002EF1F8 File Offset: 0x002ED5F8
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x06005CA6 RID: 23718 RVA: 0x002EF238 File Offset: 0x002ED638
		public void Trigger(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B);
			}
		}

		// Token: 0x06005CA7 RID: 23719 RVA: 0x002EF278 File Offset: 0x002ED678
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}

		// Token: 0x04003D83 RID: 15747
		public EffecterDef def;

		// Token: 0x04003D84 RID: 15748
		public List<SubEffecter> children = new List<SubEffecter>();
	}
}
