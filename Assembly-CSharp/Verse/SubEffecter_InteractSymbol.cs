using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000F1E RID: 3870
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x06005CAA RID: 23722 RVA: 0x002EF391 File Offset: 0x002ED791
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CAB RID: 23723 RVA: 0x002EF3A3 File Offset: 0x002ED7A3
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x06005CAC RID: 23724 RVA: 0x002EF3C9 File Offset: 0x002ED7C9
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04003D86 RID: 15750
		private Mote interactMote = null;
	}
}
