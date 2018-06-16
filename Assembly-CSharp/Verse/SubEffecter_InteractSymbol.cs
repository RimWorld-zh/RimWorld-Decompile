using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000F1F RID: 3871
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x06005CAC RID: 23724 RVA: 0x002EF2B5 File Offset: 0x002ED6B5
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x002EF2C7 File Offset: 0x002ED6C7
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x002EF2ED File Offset: 0x002ED6ED
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04003D87 RID: 15751
		private Mote interactMote = null;
	}
}
