using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CFA RID: 3322
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x06004933 RID: 18739 RVA: 0x00267A54 File Offset: 0x00265E54
		public override DamageWorker.DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageWorker.DamageResult damageResult = new DamageWorker.DamageResult();
			Fire fire = victim as Fire;
			DamageWorker.DamageResult result;
			if (fire == null || fire.Destroyed)
			{
				result = damageResult;
			}
			else
			{
				base.Apply(dinfo, victim);
				fire.fireSize -= dinfo.Amount * 0.01f;
				if (fire.fireSize <= 0.1f)
				{
					fire.Destroy(DestroyMode.Vanish);
				}
				result = damageResult;
			}
			return result;
		}

		// Token: 0x04003196 RID: 12694
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}
