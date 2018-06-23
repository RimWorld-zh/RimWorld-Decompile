using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000F1C RID: 3868
	public class Effecter
	{
		// Token: 0x04003D94 RID: 15764
		public EffecterDef def;

		// Token: 0x04003D95 RID: 15765
		public List<SubEffecter> children = new List<SubEffecter>();

		// Token: 0x06005CCA RID: 23754 RVA: 0x002F129C File Offset: 0x002EF69C
		public Effecter(EffecterDef def)
		{
			this.def = def;
			for (int i = 0; i < def.children.Count; i++)
			{
				this.children.Add(def.children[i].Spawn(this));
			}
		}

		// Token: 0x06005CCB RID: 23755 RVA: 0x002F1300 File Offset: 0x002EF700
		public void EffectTick(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubEffectTick(A, B);
			}
		}

		// Token: 0x06005CCC RID: 23756 RVA: 0x002F1340 File Offset: 0x002EF740
		public void Trigger(TargetInfo A, TargetInfo B)
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubTrigger(A, B);
			}
		}

		// Token: 0x06005CCD RID: 23757 RVA: 0x002F1380 File Offset: 0x002EF780
		public void Cleanup()
		{
			for (int i = 0; i < this.children.Count; i++)
			{
				this.children[i].SubCleanup();
			}
		}
	}
}
