using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000F22 RID: 3874
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x04003D9B RID: 15771
		private Mote interactMote = null;

		// Token: 0x06005CDC RID: 23772 RVA: 0x002F1A3D File Offset: 0x002EFE3D
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x002F1A4F File Offset: 0x002EFE4F
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x002F1A75 File Offset: 0x002EFE75
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
