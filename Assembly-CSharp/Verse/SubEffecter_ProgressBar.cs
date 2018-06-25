using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000979 RID: 2425
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x04002334 RID: 9012
		public MoteProgressBar mote;

		// Token: 0x04002335 RID: 9013
		private const float Width = 0.68f;

		// Token: 0x04002336 RID: 9014
		private const float Height = 0.12f;

		// Token: 0x06003687 RID: 13959 RVA: 0x001D16CB File Offset: 0x001CFACB
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x001D16D8 File Offset: 0x001CFAD8
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.exactScale.x = 0.68f;
				this.mote.exactScale.z = 0.12f;
			}
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x001D173A File Offset: 0x001CFB3A
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
