using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CFE RID: 3326
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x06004924 RID: 18724 RVA: 0x00266664 File Offset: 0x00264A64
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

		// Token: 0x0400318D RID: 12685
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}
