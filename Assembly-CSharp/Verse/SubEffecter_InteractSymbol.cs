using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000F23 RID: 3875
	public class SubEffecter_InteractSymbol : SubEffecter
	{
		// Token: 0x04003DA3 RID: 15779
		private Mote interactMote = null;

		// Token: 0x06005CDC RID: 23772 RVA: 0x002F1C5D File Offset: 0x002F005D
		public SubEffecter_InteractSymbol(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06005CDD RID: 23773 RVA: 0x002F1C6F File Offset: 0x002F006F
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.interactMote == null)
			{
				this.interactMote = MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
			}
		}

		// Token: 0x06005CDE RID: 23774 RVA: 0x002F1C95 File Offset: 0x002F0095
		public override void SubCleanup()
		{
			if (this.interactMote != null && !this.interactMote.Destroyed)
			{
				this.interactMote.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
