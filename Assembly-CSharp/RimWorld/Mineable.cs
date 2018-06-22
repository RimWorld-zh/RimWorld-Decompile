using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200067D RID: 1661
	public class Mineable : Building
	{
		// Token: 0x060022FA RID: 8954 RVA: 0x0012D80C File Offset: 0x0012BC0C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.yieldPct, "yieldPct", 0f, false);
		}

		// Token: 0x060022FB RID: 8955 RVA: 0x0012D82C File Offset: 0x0012BC2C
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

		// Token: 0x060022FC RID: 8956 RVA: 0x0012D8C4 File Offset: 0x0012BCC4
		public void DestroyMined(Pawn pawn)
		{
			Map map = base.Map;
			base.Destroy(DestroyMode.KillFinalize);
			this.TrySpawnYield(map, this.yieldPct, true, pawn);
		}

		// Token: 0x060022FD RID: 8957 RVA: 0x0012D8F0 File Offset: 0x0012BCF0
		public override void Destroy(DestroyMode mode)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (mode == DestroyMode.KillFinalize)
			{
				this.TrySpawnYield(map, 0.2f, false, null);
			}
		}

		// Token: 0x060022FE RID: 8958 RVA: 0x0012D924 File Offset: 0x0012BD24
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

		// Token: 0x060022FF RID: 8959 RVA: 0x0012DA14 File Offset: 0x0012BE14
		public void Notify_TookMiningDamage(int amount, Pawn miner)
		{
			int num = Mathf.Min(amount, this.HitPoints);
			float num2 = (float)num / (float)base.MaxHitPoints;
			this.yieldPct += num2 * miner.GetStatValue(StatDefOf.MiningYield, true);
		}

		// Token: 0x0400139F RID: 5023
		private float yieldPct = 0f;

		// Token: 0x040013A0 RID: 5024
		private const float YieldChanceOnNonMiningKill = 0.2f;
	}
}
