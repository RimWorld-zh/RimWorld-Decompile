using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000664 RID: 1636
	public static class TaleUtility
	{
		// Token: 0x0600223F RID: 8767 RVA: 0x00122BC0 File Offset: 0x00120FC0
		public static void Notify_PawnDied(Pawn victim, DamageInfo? dinfo)
		{
			if (Current.ProgramState == ProgramState.Playing && dinfo != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				bool flag = pawn != null && pawn.CurJob != null && pawn.jobs.curDriver is JobDriver_Execute;
				if (!flag)
				{
					bool flag2 = !victim.RaceProps.Humanlike && dinfo.Value.Instigator != null && dinfo.Value.Instigator.Spawned && dinfo.Value.Instigator is Pawn && ((Pawn)dinfo.Value.Instigator).jobs.curDriver is JobDriver_Slaughter;
					if (pawn != null)
					{
						if (victim.IsColonist)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledColonist, new object[]
							{
								pawn,
								victim
							});
						}
						else if (victim.Faction == Faction.OfPlayer && victim.RaceProps.Animal && !flag2)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledColonyAnimal, new object[]
							{
								pawn,
								victim
							});
						}
					}
					if ((victim.Faction == Faction.OfPlayer || (pawn != null && pawn.Faction == Faction.OfPlayer)) && !flag2)
					{
						TaleRecorder.RecordTale(TaleDefOf.KilledBy, new object[]
						{
							victim,
							dinfo.Value
						});
					}
					if (pawn != null)
					{
						if (dinfo.Value.Weapon != null && dinfo.Value.Weapon.building != null && dinfo.Value.Weapon.building.IsMortar)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledMortar, new object[]
							{
								pawn,
								victim,
								dinfo.Value.Weapon
							});
						}
						else if (pawn != null && pawn.Position.DistanceTo(victim.Position) >= 35f)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledLongRange, new object[]
							{
								pawn,
								victim,
								dinfo.Value.Weapon
							});
						}
						else if (dinfo.Value.Weapon != null && dinfo.Value.Weapon.IsMeleeWeapon)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledMelee, new object[]
							{
								pawn,
								victim,
								dinfo.Value.Weapon
							});
						}
						bool flag3 = false;
						if (victim.Faction != null && victim.Faction.leader == victim && victim.Faction != pawn.Faction && victim.Faction.HostileTo(pawn.Faction))
						{
							flag3 = true;
							TaleRecorder.RecordTale(TaleDefOf.DefeatedHostileFactionLeader, new object[]
							{
								pawn,
								victim
							});
						}
						if (victim.HostileTo(pawn) && Rand.Chance(TaleUtility.MajorThreatCurve.Evaluate(victim.kindDef.combatPower)))
						{
							flag3 = true;
						}
						if (flag3)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledMajorThreat, new object[]
							{
								pawn,
								victim,
								dinfo.Value.Weapon
							});
						}
						PawnCapacityDef pawnCapacityDef = victim.health.ShouldBeDeadFromRequiredCapacity();
						if (pawnCapacityDef != null)
						{
							TaleRecorder.RecordTale(TaleDefOf.KilledCapacity, new object[]
							{
								pawn,
								victim,
								pawnCapacityDef
							});
						}
					}
				}
			}
		}

		// Token: 0x04001378 RID: 4984
		private const float KilledTaleLongRangeThreshold = 35f;

		// Token: 0x04001379 RID: 4985
		private const float KilledTaleMeleeRangeThreshold = 2f;

		// Token: 0x0400137A RID: 4986
		private const float MajorEnemyThreshold = 250f;

		// Token: 0x0400137B RID: 4987
		private static readonly SimpleCurve MajorThreatCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 0f),
				true
			},
			{
				new CurvePoint(400f, 1f),
				true
			}
		};
	}
}
