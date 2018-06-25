using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000FB9 RID: 4025
	public struct ShotReport
	{
		// Token: 0x04003FAC RID: 16300
		private TargetInfo target;

		// Token: 0x04003FAD RID: 16301
		private float distance;

		// Token: 0x04003FAE RID: 16302
		private List<CoverInfo> covers;

		// Token: 0x04003FAF RID: 16303
		private float coversOverallBlockChance;

		// Token: 0x04003FB0 RID: 16304
		private ThingDef coveringGas;

		// Token: 0x04003FB1 RID: 16305
		private float factorFromShooterAndDist;

		// Token: 0x04003FB2 RID: 16306
		private float factorFromEquipment;

		// Token: 0x04003FB3 RID: 16307
		private float factorFromTargetSize;

		// Token: 0x04003FB4 RID: 16308
		private float factorFromWeather;

		// Token: 0x04003FB5 RID: 16309
		private float forcedMissRadius;

		// Token: 0x04003FB6 RID: 16310
		private ShootLine shootLine;

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06006157 RID: 24919 RVA: 0x00312E28 File Offset: 0x00311228
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

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06006158 RID: 24920 RVA: 0x00312E94 File Offset: 0x00311294
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

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06006159 RID: 24921 RVA: 0x00312F00 File Offset: 0x00311300
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

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x0600615A RID: 24922 RVA: 0x00312F44 File Offset: 0x00311344
		public float AimOnTargetChance_StandardTarget
		{
			get
			{
				float num = this.factorFromShooterAndDist * this.factorFromEquipment * this.factorFromWeather * this.FactorFromCoveringGas * this.FactorFromExecution;
				if (num < 0.0201f)
				{
					num = 0.0201f;
				}
				return num;
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x0600615B RID: 24923 RVA: 0x00312F90 File Offset: 0x00311390
		public float AimOnTargetChance_IgnoringPosture
		{
			get
			{
				return this.AimOnTargetChance_StandardTarget * this.factorFromTargetSize;
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x0600615C RID: 24924 RVA: 0x00312FB4 File Offset: 0x003113B4
		public float AimOnTargetChance
		{
			get
			{
				return this.AimOnTargetChance_IgnoringPosture * this.FactorFromPosture;
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x0600615D RID: 24925 RVA: 0x00312FD8 File Offset: 0x003113D8
		public float PassCoverChance
		{
			get
			{
				return 1f - this.coversOverallBlockChance;
			}
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x0600615E RID: 24926 RVA: 0x00312FFC File Offset: 0x003113FC
		public float TotalEstimatedHitChance
		{
			get
			{
				float value = this.AimOnTargetChance * this.PassCoverChance;
				return Mathf.Clamp01(value);
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x0600615F RID: 24927 RVA: 0x00313028 File Offset: 0x00311428
		public ShootLine ShootLine
		{
			get
			{
				return this.shootLine;
			}
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x00313044 File Offset: 0x00311444
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

		// Token: 0x06006161 RID: 24929 RVA: 0x00313344 File Offset: 0x00311744
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
				if (this.PassCoverChance < 1f)
				{
					stringBuilder.AppendLine("   " + "ShootingCover".Translate() + "        " + this.PassCoverChance.ToStringPercent());
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

		// Token: 0x06006162 RID: 24930 RVA: 0x00313660 File Offset: 0x00311A60
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
	}
}
