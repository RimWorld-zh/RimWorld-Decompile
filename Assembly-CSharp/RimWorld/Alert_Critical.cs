using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200078A RID: 1930
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x04001721 RID: 5921
		private int lastActiveFrame = -1;

		// Token: 0x04001722 RID: 5922
		private const float PulseFreq = 0.5f;

		// Token: 0x04001723 RID: 5923
		private const float PulseAmpCritical = 0.6f;

		// Token: 0x04001724 RID: 5924
		private const float PulseAmpTutorial = 0.2f;

		// Token: 0x06002AD8 RID: 10968 RVA: 0x0016A3D6 File Offset: 0x001687D6
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x0016A3F0 File Offset: 0x001687F0
		protected override Color BGColor
		{
			get
			{
				float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
				return new Color(num, num, num) * Color.red;
			}
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x0016A434 File Offset: 0x00168834
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
