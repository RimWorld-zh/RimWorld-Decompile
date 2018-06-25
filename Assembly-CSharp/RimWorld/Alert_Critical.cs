using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078A RID: 1930
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x04001725 RID: 5925
		private int lastActiveFrame = -1;

		// Token: 0x04001726 RID: 5926
		private const float PulseFreq = 0.5f;

		// Token: 0x04001727 RID: 5927
		private const float PulseAmpCritical = 0.6f;

		// Token: 0x04001728 RID: 5928
		private const float PulseAmpTutorial = 0.2f;

		// Token: 0x06002AD7 RID: 10967 RVA: 0x0016A63A File Offset: 0x00168A3A
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002AD8 RID: 10968 RVA: 0x0016A654 File Offset: 0x00168A54
		protected override Color BGColor
		{
			get
			{
				float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
				return new Color(num, num, num) * Color.red;
			}
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x0016A698 File Offset: 0x00168A98
		public override void AlertActiveUpdate()
		{
			if (this.lastActiveFrame < Time.frameCount - 1)
			{
				Messages.Message("MessageCriticalAlert".Translate(new object[]
				{
					this.GetLabel().CapitalizeFirst()
				}), new LookTargets(this.GetReport().culprits), MessageTypeDefOf.ThreatBig, true);
			}
			this.lastActiveFrame = Time.frameCount;
		}
	}
}
