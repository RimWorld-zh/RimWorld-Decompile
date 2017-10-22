using RimWorld;

namespace Verse
{
	public class HediffComp_Infecter : HediffComp
	{
		private const int UninitializedValue = -1;

		private const int WillNotInfectValue = -2;

		private const int FailedToMakeInfectionValue = -3;

		private const int AlreadyMadeInfectionValue = -4;

		private int ticksUntilInfect = -1;

		private float infectionChanceFactorFromTendRoom = 1f;

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

		public HediffCompProperties_Infecter Props
		{
			get
			{
				return (HediffCompProperties_Infecter)base.props;
			}
		}

		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			if (base.parent.IsOld())
			{
				this.ticksUntilInfect = -2;
			}
			else if (base.parent.Part.def.IsSolid(base.parent.Part, base.Pawn.health.hediffSet.hediffs))
			{
				this.ticksUntilInfect = -2;
			}
			else if (base.Pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.parent.Part))
			{
				this.ticksUntilInfect = -2;
			}
			else
			{
				float num = this.Props.infectionChance;
				if (base.Pawn.RaceProps.Animal)
				{
					num = (float)(num * 0.20000000298023224);
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
			if (base.Pawn.health.immunity.DiseaseContractChanceFactor(HediffDefOf.WoundInfection, base.parent.Part) <= 0.0010000000474974513)
			{
				this.ticksUntilInfect = -3;
			}
			else
			{
				float num = 1f;
				HediffComp_TendDuration hediffComp_TendDuration = base.parent.TryGetComp<HediffComp_TendDuration>();
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
					base.Pawn.health.AddHediff(HediffDefOf.WoundInfection, base.parent.Part, default(DamageInfo?));
				}
				else
				{
					this.ticksUntilInfect = -3;
				}
			}
		}

		public override string CompDebugString()
		{
			if (this.ticksUntilInfect <= 0)
			{
				if (this.ticksUntilInfect == -4)
				{
					return "already created infection";
				}
				if (this.ticksUntilInfect == -3)
				{
					return "failed to make infection";
				}
				if (this.ticksUntilInfect == -2)
				{
					return "will not make infection";
				}
				if (this.ticksUntilInfect == -1)
				{
					return "uninitialized data!";
				}
				return "unexpected ticksUntilInfect = " + this.ticksUntilInfect;
			}
			return "infection may appear in: " + this.ticksUntilInfect + " ticks\ninfectChnceFactorFromTendRoom: " + this.infectionChanceFactorFromTendRoom.ToStringPercent();
		}
	}
}
