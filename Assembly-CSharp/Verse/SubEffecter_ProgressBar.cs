using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200097B RID: 2427
	public class SubEffecter_ProgressBar : SubEffecter
	{
		// Token: 0x0600368A RID: 13962 RVA: 0x001D10CF File Offset: 0x001CF4CF
		public SubEffecter_ProgressBar(SubEffecterDef def, Effecter parent) : base(def, parent)
		{
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x001D10DC File Offset: 0x001CF4DC
		public override void SubEffectTick(TargetInfo A, TargetInfo B)
		{
			if (this.mote == null)
			{
				this.mote = (MoteProgressBar)MoteMaker.MakeInteractionOverlay(this.def.moteDef, A, B);
				this.mote.exactScale.x = 0.68f;
				this.mote.exactScale.z = 0.12f;
			}
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x001D113E File Offset: 0x001CF53E
		public override void SubCleanup()
		{
			if (this.mote != null && !this.mote.Destroyed)
			{
				this.mote.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0400232E RID: 9006
		public MoteProgressBar mote;

		// Token: 0x0400232F RID: 9007
		private const float Width = 0.68f;

		// Token: 0x04002330 RID: 9008
		private const float Height = 0.12f;
	}
}
