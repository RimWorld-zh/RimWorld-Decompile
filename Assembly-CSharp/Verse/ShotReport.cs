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
		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06006122 RID: 24866 RVA: 0x00310214 File Offset: 0x0030E614
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

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06006123 RID: 24867 RVA: 0x00310280 File Offset: 0x0030E680
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

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06006124 RID: 24868 RVA: 0x003102EC File Offset: 0x0030E6EC
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

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06006125 RID: 24869 RVA: 0x00310330 File Offset: 0x0030E730
		public float ChanceToNotHitCover
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06006126 RID: 24870 RVA: 0x00310354 File Offset: 0x0030E754
		public float ChanceToNotGoWild_IgnoringPosture
		{
			get
			{
				return this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.factorFromTargetSize * this.FactorFromCoveringGas * this.FactorFromExecution;
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06006127 RID: 24871 RVA: 0x00310394 File Offset: 0x0030E794
		public float TotalEstimatedHitChance
		{
			get
			{
				float value = this.ChanceToNotGoWild_IgnoringPosture * this.FactorFromPosture * this.ChanceToNotHitCover;
				return Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06006128 RID: 24872 RVA: 0x003103C8 File Offset: 0x0030E7C8
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x06006129 RID: 24873 RVA: 0x003103E4 File Offset: 0x0030E7E4
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

		// Token: 0x0600612A RID: 24874 RVA: 0x003106E4 File Offset: 0x0030EAE4
		public string GetTextReadout()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.forcedMissRadius > 0.5f)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("WeaponMissRadius".Translate() + "   " + this.forcedMissRadius.ToString("F1"));
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

		// Token: 0x0600612B RID: 24875 RVA: 0x003109D0 File Offset: 0x0030EDD0
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

		// Token: 0x04003F80 RID: 16256
		private TargetInfo target;

		// Token: 0x04003F81 RID: 16257
		private float distance;

		// Token: 0x04003F82 RID: 16258
		private List<CoverInfo> covers;

		// Token: 0x04003F83 RID: 16259
		private float coversOverallBlockChance;

		// Token: 0x04003F84 RID: 16260
		private ThingDef coveringGas;

		// Token: 0x04003F85 RID: 16261
		private float factorFromShooterAndDist;

		// Token: 0x04003F86 RID: 16262
		private float factorFromEquipment;

		// Token: 0x04003F87 RID: 16263
		private float factorFromTargetSize;

		// Token: 0x04003F88 RID: 16264
		private float factorFromWeather;

		// Token: 0x04003F89 RID: 16265
		private float forcedMissRadius;

		// Token: 0x04003F8A RID: 16266
		private ShootLine shootLine;

		// Token: 0x04003F8B RID: 16267
		public const float LayingDownHitChanceFactorMinDistance = 4.5f;

		// Token: 0x04003F8C RID: 16268
		public const float HitChanceFactorIfLayingDown = 0.2f;

		// Token: 0x04003F8D RID: 16269
		private const float NonPawnShooterHitFactorPerDistance = 0.96f;

		// Token: 0x04003F8E RID: 16270
		private const float ExecutionMaxDistance = 3.9f;

		// Token: 0x04003F8F RID: 16271
		private const float ExecutionFactor = 7.5f;

		// Token: 0x04003F90 RID: 16272
		private const float TargetSizeFactorFromFillPercentFactor = 1.7f;

		// Token: 0x04003F91 RID: 16273
		private const float TargetSizeFactorMin = 0.5f;

		// Token: 0x04003F92 RID: 16274
		private const float TargetSizeFactorMax = 2f;
	}
}
