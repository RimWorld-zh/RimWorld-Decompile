using System;
using RimWorld;

namespace Verse
{
	public class DamageWorker_Extinguish : DamageWorker
	{
		private const float DamageAmountToFireSizeRatio = 0.01f;

		public DamageWorker_Extinguish()
		{
		}

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
