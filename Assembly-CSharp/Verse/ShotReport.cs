using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FB5 RID: 4021
	public struct ShotReport
	{
		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06006149 RID: 24905 RVA: 0x00312394 File Offset: 0x00310794
		private float FactorFromPosture
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null)
					{
						if (this.distance >= 4.5f && pawn.GetPosture() != PawnPosture.Standing)
						{
							return 0.2f;
						}
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x0600614A RID: 24906 RVA: 0x00312400 File Offset: 0x00310800
		private float FactorFromExecution
		{
			get
			{
				if (this.target.HasThing)
				{
					Pawn pawn = this.target.Thing as Pawn;
					if (pawn != null)
					{
						if (this.distance <= 3.9f && pawn.GetPosture() != PawnPosture.Standing)
						{
							return 7.5f;
						}
					}
				}
				return 1f;
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x0600614B RID: 24907 RVA: 0x0031246C File Offset: 0x0031086C
		private float FactorFromCoveringGas
		{
			get
			{
				float result;
				if (this.coveringGas != null)
				{
					result = 1f - this.coveringGas.gas.accuracyPenalty;
				}
				else
				{
					result = 1f;
				}
				return result;
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x0600614C RID: 24908 RVA: 0x003124B0 File Offset: 0x003108B0
		public float ChanceToNotHitCover
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x0600614D RID: 24909 RVA: 0x003124D4 File Offset: 0x003108D4
		public float ChanceToNotGoWild_IgnoringPosture
		{
			get
			{
				return this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.factorFromTargetSize * this.FactorFromCoveringGas * this.FactorFromExecution;
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x0600614E RID: 24910 RVA: 0x00312514 File Offset: 0x00310914
		public float TotalEstimatedHitChance
		{
			get
			{
				float value = this.ChanceToNotGoWild_IgnoringPosture * this.FactorFromPosture * this.ChanceToNotHitCover;
				return Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x0600614F RID: 24911 RVA: 0x00312548 File Offset: 0x00310948
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x06006150 RID: 24912 RVA: 0x00312564 File Offset: 0x00310964
		public static ShotReport HitReportFor(Thing caster, Verb verb, LocalTargetInfo target)
		{
			Pawn pawn = caster as Pawn;
			IntVec3 cell = target.Cell;
			ShotReport result;
			result.distance = (cell - caster.Position).LengthHorizontal;
			result.target = target.ToTargetInfo(caster.Map);
			float f;
			if (pawn != null)
			{
				f = pawn.GetStatValue(StatDefOf.ShootingAccuracy, true);
			}
			else
			{
				f = 0.96f;
			}
			result.factorFromShooterAndDist = Mathf.Pow(f, result.distance);
			if (result.factorFromShooterAndDist < 0.0201f)
			{
				result.factorFromShooterAndDist = 0.0201f;
			}
			result.factorFromEquipment = verb.verbProps.GetHitChanceFactor(verb.ownerEquipment, result.distance);
			result.covers = CoverUtility.CalculateCoverGiverSet(target, caster.Position, caster.Map);
			result.coversOverallBlockChance = CoverUtility.CalculateOverallBlockChance(target, caster.Position, caster.Map);
			result.coveringGas = null;
			if (verb.TryFindShootLineFromTo(verb.caster.Position, target, out result.shootLine))
			{
				foreach (IntVec3 c in result.shootLine.Points())
				{
					Thing gas = c.GetGas(caster.Map);
					if (gas != null && (result.coveringGas == null || result.coveringGas.gas.accuracyPenalty < gas.def.gas.accuracyPenalty))
					{
						result.coveringGas = gas.def;
					}
				}
			}
			else
			{
				result.shootLine = new ShootLine(IntVec3.Invalid, IntVec3.Invalid);
			}
			if (!caster.Position.Roofed(caster.Map) || !target.Cell.Roofed(caster.Map))
			{
				result.factorFromWeather = caster.Map.weatherManager.CurWeatherAccuracyMultiplier;
			}
			else
			{
				result.factorFromWeather = 1f;
			}
			if (target.HasThing)
			{
				Pawn pawn2 = target.Thing as Pawn;
				if (pawn2 != null)
				{
					result.factorFromTargetSize = pawn2.BodySize;
				}
				else
				{
					result.factorFromTargetSize = target.Thing.def.fillPercent * (float)target.Thing.def.size.x * (float)target.Thing.def.size.z * 1.7f;
				}
				result.factorFromTargetSize = Mathf.Clamp(result.factorFromTargetSize, 0.5f, 2f);
			}
			else
			{
				result.factorFromTargetSize = 1f;
			}
			result.forcedMissRadius = verb.verbProps.forcedMissRadius;
			return result;
		}

		// Token: 0x06006151 RID: 24913 RVA: 0x00312864 File Offset: 0x00310C64
		public string GetTextReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.forcedMissRadius > 0.5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("WeaponMissRadius".Translate() + "   " + this.forcedMissRadius.ToString("F1"));
				stringBuilder.AppendLine("DirectHitChance".Translate() + "   " + (1f / (float)GenRadial.NumCellsInRadius(this.forcedMissRadius)).ToStringPercent());
			}
			else
			{
				stringBuilder.AppendLine(" " + this.TotalEstimatedHitChance.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportShooterAbility".Translate() + "  " + this.factorFromShooterAndDist.ToStringPercent());
				stringBuilder.AppendLine("   " + "ShootReportWeapon".Translate() + "        " + this.factorFromEquipment.ToStringPercent());
				if (this.target.HasThing)
				{
					if (this.factorFromTargetSize != 1f)
					{
						stringBuilder.AppendLine("   " + "TargetSize".Translate() + "       " + this.factorFromTargetSize.ToStringPercent());
					}
				}
				if (this.factorFromWeather < 0.99f)
				{
					stringBuilder.AppendLine("   " + "Weather".Translate() + "         " + this.factorFromWeather.ToStringPercent());
				}
				if (this.FactorFromCoveringGas < 0.99f)
				{
					stringBuilder.AppendLine("   " + this.coveringGas.label.CapitalizeFirst() + "         " + this.FactorFromCoveringGas.ToStringPercent());
				}
				if (this.FactorFromPosture < 0.9999f)
				{
					stringBuilder.AppendLine("   " + "TargetProne".Translate() + "  " + this.FactorFromPosture.ToStringPercent());
				}
				if (this.FactorFromExecution != 1f)
				{
					stringBuilder.AppendLine("   " + "Execution".Translate() + "   " + this.FactorFromExecution.ToStringPercent());
				}
				if (this.ChanceToNotHitCover < 1f)
				{
					stringBuilder.AppendLine("   " + "ShootingCover".Translate() + "        " + this.ChanceToNotHitCover.ToStringPercent());
					for (int i = 0; i < this.covers.Count; i++)
					{
						CoverInfo coverInfo = this.covers[i];
						stringBuilder.AppendLine("     " + "CoverThingBlocksPercentOfShots".Translate(new object[]
						{
							coverInfo.Thing.LabelCap,
							coverInfo.BlockChance.ToStringPercent()
						}));
					}
				}
				else
				{
					stringBuilder.AppendLine("   (" + "NoCoverLower".Translate() + ")");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006152 RID: 24914 RVA: 0x00312B80 File Offset: 0x00310F80
		public Thing GetRandomCoverToMissInto()
		{
			CoverInfo coverInfo;
			Thing result;
			if (this.covers.TryRandomElementByWeight((CoverInfo c) => c.BlockChance, out coverInfo))
			{
				result = coverInfo.Thing;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04003F9C RID: 16284
		private TargetInfo target;

		// Token: 0x04003F9D RID: 16285
		private float distance;

		// Token: 0x04003F9E RID: 16286
		private List<CoverInfo> covers;

		// Token: 0x04003F9F RID: 16287
		private float coversOverallBlockChance;

		// Token: 0x04003FA0 RID: 16288
		private ThingDef coveringGas;

		// Token: 0x04003FA1 RID: 16289
		private float factorFromShooterAndDist;

		// Token: 0x04003FA2 RID: 16290
		private float factorFromEquipment;

		// Token: 0x04003FA3 RID: 16291
		private float factorFromTargetSize;

		// Token: 0x04003FA4 RID: 16292
		private float factorFromWeather;

		// Token: 0x04003FA5 RID: 16293
		private float forcedMissRadius;

		// Token: 0x04003FA6 RID: 16294
		private ShootLine shootLine;

		// Token: 0x04003FA7 RID: 16295
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x04003FA8 RID: 16296
		public const float HitChanceFactorIfLayingDown = 0.2f;

		// Token: 0x04003FA9 RID: 16297
		private const float NonPawnShooterHitFactorPerDistance = 0.96f;

		// Token: 0x04003FAA RID: 16298
		private const float ExecutionMaxDistance = 3.9f;

		// Token: 0x04003FAB RID: 16299
		private const float ExecutionFactor = 7.5f;

		// Token: 0x04003FAC RID: 16300
		private const float TargetSizeFactorFromFillPercentFactor = 1.7f;

		// Token: 0x04003FAD RID: 16301
		private const float TargetSizeFactorMin = 0.5f;

		// Token: 0x04003FAE RID: 16302
		private const float TargetSizeFactorMax = 2f;
	}
}
