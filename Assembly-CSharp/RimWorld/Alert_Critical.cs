using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000788 RID: 1928
	public abstract class Alert_Critical : Alert
	{
		// Token: 0x06002AD4 RID: 10964 RVA: 0x0016A286 File Offset: 0x00168686
		public Alert_Critical()
		{
			this.defaultPriority = AlertPriority.Critical;
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x06002AD5 RID: 10965 RVA: 0x0016A2A0 File Offset: 0x001686A0
		protected override Color BGColor
		{
			get
			{
				float num = Pulser.PulseBrightness(0.5f, Pulser.PulseBrightness(0.5f, 0.6f));
				return new Color(num, num, num) * Color.red;
			}
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x0016A2E4 File Offset: 0x001686E4
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

		// Token: 0x04001721 RID: 5921
		private int lastActiveFrame = -1;

		// Token: 0x04001722 RID: 5922
		private const float PulseFreq = 0.5f;

		// Token: 0x04001723 RID: 5923
		private const float PulseAmpCritical = 0.6f;

		// Token: 0x04001724 RID: 5924
		private const float PulseAmpTutorial = 0.2f;
	}
}
