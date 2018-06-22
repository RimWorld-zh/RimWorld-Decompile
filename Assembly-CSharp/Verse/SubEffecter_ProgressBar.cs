using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000977 RID: 2423
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x06003683 RID: 13955 RVA: 0x001D12B7 File Offset: 0x001CF6B7
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x001D12C4 File Offset: 0x001CF6C4
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.exactScale.x = 0.68f;
				this.mote.exactScale.z = 0.12f;
			}
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x001D1326 File Offset: 0x001CF726
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0400232C RID: 9004
		public MoteProgressBar mote;

		// Token: 0x0400232D RID: 9005
		private const float Width = 0.68f;

		// Token: 0x0400232E RID: 9006
		private const float Height = 0.12f;
	}
}
