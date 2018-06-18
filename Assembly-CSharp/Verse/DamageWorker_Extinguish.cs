using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000CFD RID: 3325
	public class DamageWorker_Extinguish : DamageWorker
	{
		// Token: 0x06004922 RID: 18722 RVA: 0x0026663C File Offset: 0x00264A3C
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

		// Token: 0x0400318B RID: 12683
		private const float DamageAmountToFireSizeRatio = 0.01f;
	}
}
