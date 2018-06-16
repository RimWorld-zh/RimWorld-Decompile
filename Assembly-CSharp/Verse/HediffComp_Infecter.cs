using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D18 RID: 3352
	public class HediffComp_Infecter : HediffComp
	{
		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x002690A0 File Offset: 0x002674A0
		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)this.props;
			}
		}

		// Token: 0x060049C9 RID: 18889 RVA: 0x002690C0 File Offset: 0x002674C0
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (this.parent.IsPermanent())
			{
				this.ticksUntilInfect = -2;
			}
			else if (this.parent.Part.def.IsSolid(this.parent.Part, base.Pawn.health.hediffSet.hediffs))
			{
				this.ticksUntilInfect = -2;
			}
			else if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(this.parent.Part))
			{
				this.ticksUntilInfect = -2;
			}
			else
			{
				float num = this.Props.infectionChance;
				if (base.Pawn.RaceProps.Animal)
				{
					num *= 0.2f;
				}
				if (Rand.Value <= num)
				{
					this.ticksUntilInfect = HealthTunings.InfectionDelayRange.RandomInRange;
				}
				else
				{
					this.ticksUntilInfect = -2;
				}
			}
		}

		// Token: 0x060049CA RID: 18890 RVA: 0x002691BB File Offset: 0x002675BB
		public override void CompExposeData()
		{
			Scribe_Values.Look<float>(ref this.infectionChanceFactorFromTendRoom, "infectionChanceFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksUntilInfect, "ticksUntilInfect", -2, false);
		}

		// Token: 0x060049CB RID: 18891 RVA: 0x002691E7 File Offset: 0x002675E7
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (this.ticksUntilInfect > 0)
			{
				this.ticksUntilInfect--;
				if (this.ticksUntilInfect == 0)
				{
					this.CheckMakeInfection();
				}
			}
		}

		// Token: 0x060049CC RID: 18892 RVA: 0x00269218 File Offset: 0x00267618
		public override void CompTended(float quality, int batchPosition = 0)
		{
			if (base.Pawn.Spawned)
			{
				Room room = base.Pawn.GetRoom(RegionType.Set_Passable);
				if (room != null)
				{
					this.infectionChanceFactorFromTendRoom = room.GetStat(RoomStatDefOf.InfectionChanceFactor);
				}
			}
		}

		// Token: 0x060049CD RID: 18893 RVA: 0x0026925C File Offset: 0x0026765C
		private void CheckMakeInfection()
		{
			if (base.Pawn.health.immunity.DiseaseContractChanceFactor(HediffDefOf.WoundInfection, this.parent.Part) <= 0.001f)
			{
				this.ticksUntilInfect = -3;
			}
			else
			{
				float num = 1f;
				HediffComp_TendDuration hediffComp_TendDuration = this.parent.TryGetComp<HediffComp_TendDuration>();
				if (hediffComp_TendDuration != null && hediffComp_TendDuration.IsTended)
				{
					num *= this.infectionChanceFactorFromTendRoom;
					num *= HediffComp_Infecter.InfectionChanceFactorFromTendQualityCurve.Evaluate(hediffComp_TendDuration.tendQuality);
				}
				if (base.Pawn.Faction == Faction.OfPlayer)
				{
					num *= Find.Storyteller.difficulty.playerPawnInfectionChanceFactor;
				}
				if (Rand.Value < num)
				{
					this.ticksUntilInfect = -4;
					base.Pawn.health.AddHediff(HediffDefOf.WoundInfection, this.parent.Part, null, null);
				}
				else
				{
					this.ticksUntilInfect = -3;
				}
			}
		}

		// Token: 0x060049CE RID: 18894 RVA: 0x0026935C File Offset: 0x0026775C
		public override string CompDebugString()
		{
			string result;
			if (this.ticksUntilInfect <= 0)
			{
				if (this.ticksUntilInfect == -4)
				{
					result = "already created infection";
				}
				else if (this.ticksUntilInfect == -3)
				{
					result = "failed to make infection";
				}
				else if (this.ticksUntilInfect == -2)
				{
					result = "will not make infection";
				}
				else if (this.ticksUntilInfect == -1)
				{
					result = "uninitialized data!";
				}
				else
				{
					result = "unexpected ticksUntilInfect = " + this.ticksUntilInfect;
				}
			}
			else
			{
				result = string.Concat(new object[]
				{
					"infection may appear in: ",
					this.ticksUntilInfect,
					" ticks\ninfectChnceFactorFromTendRoom: ",
					this.infectionChanceFactorFromTendRoom.ToStringPercent()
				});
			}
			return result;
		}

		// Token: 0x04003205 RID: 12805
		private int ticksUntilInfect = -1;

		// Token: 0x04003206 RID: 12806
		private float infectionChanceFactorFromTendRoom = 1f;

		// Token: 0x04003207 RID: 12807
		private const int UninitializedValue = -1;

		// Token: 0x04003208 RID: 12808
		private const int WillNotInfectValue = -2;

		// Token: 0x04003209 RID: 12809
		private const int FailedToMakeInfectionValue = -3;

		// Token: 0x0400320A RID: 12810
		private const int AlreadyMadeInfectionValue = -4;

		// Token: 0x0400320B RID: 12811
		private static readonly SimpleCurve InfectionChanceFactorFromTendQualityCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.85f),
				true
			},
			{
				new CurvePoint(1f, 0.05f),
				true
			}
		};
	}
}
