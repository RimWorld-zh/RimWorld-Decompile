using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000681 RID: 1665
	public class Mineable : Building
	{
		// Token: 0x06002302 RID: 8962 RVA: 0x0012D6C4 File Offset: 0x0012BAC4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.yieldPct, "yieldPct", 0f, false);
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x0012D6E4 File Offset: 0x0012BAE4
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (!absorbed)
			{
				if (this.def.building.mineableThing != null && this.def.building.mineableYieldWasteable)
				{
					if (dinfo.Def == DamageDefOf.Mining && dinfo.Instigator != null && dinfo.Instigator is Pawn)
					{
						this.Notify_TookMiningDamage(GenMath.RoundRandom(dinfo.Amount), (Pawn)dinfo.Instigator);
					}
				}
				absorbed = false;
			}
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x0012D77C File Offset: 0x0012BB7C
		public void DestroyMined(Pawn pawn)
		{
			Map map = base.Map;
			base.Destroy(DestroyMode.KillFinalize);
			this.TrySpawnYield(map, this.yieldPct, true, pawn);
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x0012D7A8 File Offset: 0x0012BBA8
		public override void Destroy(DestroyMode mode)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				this.TrySpawnYield(map, 0.2f, false, null);
			}
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x0012D7DC File Offset: 0x0012BBDC
		private void TrySpawnYield(Map map, float yieldChance, bool moteOnWaste, Pawn pawn)
		{
			if (this.def.building.mineableThing != null)
			{
				if (Rand.Value <= this.def.building.mineableDropChance)
				{
					int num = this.def.building.mineableYield;
					if (this.def.building.mineableYieldWasteable)
					{
						num = Mathf.Max(1, GenMath.RoundRandom((float)num * this.yieldPct));
					}
					Thing thing = ThingMaker.MakeThing(this.def.building.mineableThing, null);
					thing.stackCount = num;
					GenSpawn.Spawn(thing, base.Position, map, WipeMode.Vanish);
					if (pawn == null || !pawn.IsColonist)
					{
						if (thing.def.EverHaulable && !thing.def.designateHaulable)
						{
							thing.SetForbidden(true, true);
						}
					}
				}
			}
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x0012D8CC File Offset: 0x0012BCCC
		public void Notify_TookMiningDamage(int amount, Pawn miner)
		{
			int num = Mathf.Min(amount, this.HitPoints);
			float num2 = (float)num / (float)base.MaxHitPoints;
			this.yieldPct += num2 * miner.GetStatValue(StatDefOf.MiningYield, true);
		}

		// Token: 0x040013A1 RID: 5025
		private float yieldPct = 0f;

		// Token: 0x040013A2 RID: 5026
		private const float YieldChanceOnNonMiningKill = 0.2f;
	}
}
