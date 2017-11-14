using RimWorld;

namespace Verse
{
	public class DamageWorker_Extinguish : DamageWorker
	{
		private const float DamageAmountToFireSizeRatio = 0.01f;

		public override DamageResult Apply(DamageInfo dinfo, Thing victim)
		{
			DamageResult result = DamageResult.MakeNew();
			Fire fire = victim as Fire;
			if (fire != null && !fire.Destroyed)
			{
				base.Apply(dinfo, victim);
				fire.fireSize -= (float)((float)dinfo.Amount * 0.0099999997764825821);
				if (fire.fireSize <= 0.10000000149011612)
				{
					fire.Destroy(DestroyMode.Vanish);
				}
				return result;
			}
			return result;
		}
	}
}
