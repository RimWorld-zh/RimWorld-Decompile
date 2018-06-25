using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D17 RID: 3351
	public class HediffComp_Infecter : HediffComp
	{
		// Token: 0x04003215 RID: 12821
		private int ticksUntilInfect = -1;

		// Token: 0x04003216 RID: 12822
		private float infectionChanceFactorFromTendRoom = 1f;

		// Token: 0x04003217 RID: 12823
		private const int UninitializedValue = -1;

		// Token: 0x04003218 RID: 12824
		private const int WillNotInfectValue = -2;

		// Token: 0x04003219 RID: 12825
		private const int FailedToMakeInfectionValue = -3;

		// Token: 0x0400321A RID: 12826
		private const int AlreadyMadeInfectionValue = -4;

		// Token: 0x0400321B RID: 12827
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

		// Token: 0x17000BAF RID: 2991
		// (get) Token: 0x060049DA RID: 18906 RVA: 0x0026A868 File Offset: 0x00268C68
		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)this.props;
			}
		}

		// Token: 0x060049DB RID: 18907 RVA: 0x0026A888 File Offset: 0x00268C88
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
					this.ticksUntilInfect = HealthTuning.InfectionDelayRange.RandomInRange;
				}
				else
				{
					this.ticksUntilInfect = -2;
				}
			}
		}

		// Token: 0x060049DC RID: 18908 RVA: 0x0026A983 File Offset: 0x00268D83
		public override void CompExposeData()
		{
			Scribe_Values.Look<float>(ref this.infectionChanceFactorFromTendRoom, "infectionChanceFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksUntilInfect, "ticksUntilInfect", -2, false);
		}

		// Token: 0x060049DD RID: 18909 RVA: 0x0026A9AF File Offset: 0x00268DAF
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

		// Token: 0x060049DE RID: 18910 RVA: 0x0026A9E0 File Offset: 0x00268DE0
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

		// Token: 0x060049DF RID: 18911 RVA: 0x0026AA24 File Offset: 0x00268E24
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

		// Token: 0x060049E0 RID: 18912 RVA: 0x0026AB24 File Offset: 0x00268F24
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
	}
}
