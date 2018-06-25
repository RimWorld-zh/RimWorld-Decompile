using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CFD RID: 3325
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x0400319D RID: 12701
		private const float DamageAmountToFireSizeRatio = 0.01f;

		// Token: 0x06004936 RID: 18742 RVA: 0x00267E10 File Offset: 0x00266210
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
	}
}
