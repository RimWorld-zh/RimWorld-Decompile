using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000F1E RID: 3870
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x04003D98 RID: 15768
		private Mote interactMote = null;

		// Token: 0x06005CD2 RID: 23762 RVA: 0x002F13BD File Offset: 0x002EF7BD
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x002F13CF File Offset: 0x002EF7CF
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x002F13F5 File Offset: 0x002EF7F5
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
