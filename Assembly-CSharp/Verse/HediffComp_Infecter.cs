using System;
using RimWorld;

namespace Verse
{
	public class HediffComp_Infecter : HediffComp
	{
		private int ticksUntilInfect = -1;

		private float infectionChanceFactorFromTendRoom = 1f;

		private const int UninitializedValue = -1;

		private const int WillNotInfectValue = -2;

		private const int FailedToMakeInfectionValue = -3;

		private const int AlreadyMadeInfectionValue = -4;

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

		public HediffComp_Infecter()
		{
		}

		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)this.props;
			}
		}

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
					num *= 0.1f;
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

		public override void CompExposeData()
		{
			Scribe_Values.Look<float>(ref this.infectionChanceFactorFromTendRoom, "infectionChanceFactor", 0f, false);
			Scribe_Values.Look<int>(ref this.ticksUntilInfect, "ticksUntilInfect", -2, false);
		}

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

		// Note: this type is marked as 'beforefieldinit'.
		static HediffComp_Infecter()
		{
		}
	}
}
