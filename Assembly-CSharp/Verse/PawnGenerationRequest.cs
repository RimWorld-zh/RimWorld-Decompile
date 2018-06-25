using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using RimWorld;

namespace Verse
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PawnGenerationRequest
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private PawnKindDef <KindDef>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private PawnGenerationContext <Context>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Faction <Faction>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private int <Tile>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <ForceGenerateNewPawn>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <Newborn>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <AllowDead>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <AllowDowned>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <CanGeneratePawnRelations>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <MustBeCapableOfViolence>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private float <ColonistRelationChanceFactor>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <ForceAddFreeWarmLayerIfNeeded>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <AllowGay>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <AllowFood>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <Inhabitant>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <CertainlyBeenInCryptosleep>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <ForceRedressWorldPawnIfFormerColonist>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private bool <WorldPawnFactionDoesntMatter>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Predicate<Pawn> <ValidatorPreGear>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Predicate<Pawn> <ValidatorPostGear>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private float? <MinChanceToRedressWorldPawn>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private float? <FixedBiologicalAge>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private float? <FixedChronologicalAge>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private Gender? <FixedGender>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private float? <FixedMelanin>k__BackingField;

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		[CompilerGenerated]
		private string <FixedLastName>k__BackingField;

		public PawnGenerationRequest(PawnKindDef kind, Faction faction = null, PawnGenerationContext context = PawnGenerationContext.NonPlayer, int tile = -1, bool forceGenerateNewPawn = false, bool newborn = false, bool allowDead = false, bool allowDowned = false, bool canGeneratePawnRelations = true, bool mustBeCapableOfViolence = false, float colonistRelationChanceFactor = 1f, bool forceAddFreeWarmLayerIfNeeded = false, bool allowGay = true, bool allowFood = true, bool inhabitant = false, bool certainlyBeenInCryptosleep = false, bool forceRedressWorldPawnIfFormerColonist = false, bool worldPawnFactionDoesntMatter = false, Predicate<Pawn> validatorPreGear = null, Predicate<Pawn> validatorPostGear = null, float? minChanceToRedressWorldPawn = null, float? fixedBiologicalAge = null, float? fixedChronologicalAge = null, Gender? fixedGender = null, float? fixedMelanin = null, string fixedLastName = null)
		{
			this = default(PawnGenerationRequest);
			if (context == PawnGenerationContext.All)
			{
				Log.Error("Should not generate pawns with context 'All'", false);
				context = PawnGenerationContext.NonPlayer;
			}
			if (inhabitant && (tile == -1 || Current.Game.FindMap(tile) == null))
			{
				Log.Error("Trying to generate an inhabitant but map is null.", false);
				inhabitant = false;
			}
			this.KindDef = kind;
			this.Context = context;
			this.Faction = faction;
			this.Tile = tile;
			this.ForceGenerateNewPawn = forceGenerateNewPawn;
			this.Newborn = newborn;
			this.AllowDead = allowDead;
			this.AllowDowned = allowDowned;
			this.CanGeneratePawnRelations = canGeneratePawnRelations;
			this.MustBeCapableOfViolence = mustBeCapableOfViolence;
			this.ColonistRelationChanceFactor = colonistRelationChanceFactor;
			this.ForceAddFreeWarmLayerIfNeeded = forceAddFreeWarmLayerIfNeeded;
			this.AllowGay = allowGay;
			this.AllowFood = allowFood;
			this.Inhabitant = inhabitant;
			this.CertainlyBeenInCryptosleep = certainlyBeenInCryptosleep;
			this.ForceRedressWorldPawnIfFormerColonist = forceRedressWorldPawnIfFormerColonist;
			this.WorldPawnFactionDoesntMatter = worldPawnFactionDoesntMatter;
			this.ValidatorPreGear = validatorPreGear;
			this.ValidatorPostGear = validatorPostGear;
			this.MinChanceToRedressWorldPawn = minChanceToRedressWorldPawn;
			this.FixedBiologicalAge = fixedBiologicalAge;
			this.FixedChronologicalAge = fixedChronologicalAge;
			this.FixedGender = fixedGender;
			this.FixedMelanin = fixedMelanin;
			this.FixedLastName = fixedLastName;
		}

		public PawnKindDef KindDef
		{
			[CompilerGenerated]
			get
			{
				return this.<KindDef>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<KindDef>k__BackingField = value;
			}
		}

		public PawnGenerationContext Context
		{
			[CompilerGenerated]
			get
			{
				return this.<Context>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Context>k__BackingField = value;
			}
		}

		public Faction Faction
		{
			[CompilerGenerated]
			get
			{
				return this.<Faction>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Faction>k__BackingField = value;
			}
		}

		public int Tile
		{
			[CompilerGenerated]
			get
			{
				return this.<Tile>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Tile>k__BackingField = value;
			}
		}

		public bool ForceGenerateNewPawn
		{
			[CompilerGenerated]
			get
			{
				return this.<ForceGenerateNewPawn>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ForceGenerateNewPawn>k__BackingField = value;
			}
		}

		public bool Newborn
		{
			[CompilerGenerated]
			get
			{
				return this.<Newborn>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Newborn>k__BackingField = value;
			}
		}

		public bool AllowDead
		{
			[CompilerGenerated]
			get
			{
				return this.<AllowDead>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<AllowDead>k__BackingField = value;
			}
		}

		public bool AllowDowned
		{
			[CompilerGenerated]
			get
			{
				return this.<AllowDowned>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<AllowDowned>k__BackingField = value;
			}
		}

		public bool CanGeneratePawnRelations
		{
			[CompilerGenerated]
			get
			{
				return this.<CanGeneratePawnRelations>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CanGeneratePawnRelations>k__BackingField = value;
			}
		}

		public bool MustBeCapableOfViolence
		{
			[CompilerGenerated]
			get
			{
				return this.<MustBeCapableOfViolence>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<MustBeCapableOfViolence>k__BackingField = value;
			}
		}

		public float ColonistRelationChanceFactor
		{
			[CompilerGenerated]
			get
			{
				return this.<ColonistRelationChanceFactor>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ColonistRelationChanceFactor>k__BackingField = value;
			}
		}

		public bool ForceAddFreeWarmLayerIfNeeded
		{
			[CompilerGenerated]
			get
			{
				return this.<ForceAddFreeWarmLayerIfNeeded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ForceAddFreeWarmLayerIfNeeded>k__BackingField = value;
			}
		}

		public bool AllowGay
		{
			[CompilerGenerated]
			get
			{
				return this.<AllowGay>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<AllowGay>k__BackingField = value;
			}
		}

		public bool AllowFood
		{
			[CompilerGenerated]
			get
			{
				return this.<AllowFood>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<AllowFood>k__BackingField = value;
			}
		}

		public bool Inhabitant
		{
			[CompilerGenerated]
			get
			{
				return this.<Inhabitant>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Inhabitant>k__BackingField = value;
			}
		}

		public bool CertainlyBeenInCryptosleep
		{
			[CompilerGenerated]
			get
			{
				return this.<CertainlyBeenInCryptosleep>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CertainlyBeenInCryptosleep>k__BackingField = value;
			}
		}

		public bool ForceRedressWorldPawnIfFormerColonist
		{
			[CompilerGenerated]
			get
			{
				return this.<ForceRedressWorldPawnIfFormerColonist>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ForceRedressWorldPawnIfFormerColonist>k__BackingField = value;
			}
		}

		public bool WorldPawnFactionDoesntMatter
		{
			[CompilerGenerated]
			get
			{
				return this.<WorldPawnFactionDoesntMatter>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<WorldPawnFactionDoesntMatter>k__BackingField = value;
			}
		}

		public Predicate<Pawn> ValidatorPreGear
		{
			[CompilerGenerated]
			get
			{
				return this.<ValidatorPreGear>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ValidatorPreGear>k__BackingField = value;
			}
		}

		public Predicate<Pawn> ValidatorPostGear
		{
			[CompilerGenerated]
			get
			{
				return this.<ValidatorPostGear>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ValidatorPostGear>k__BackingField = value;
			}
		}

		public float? MinChanceToRedressWorldPawn
		{
			[CompilerGenerated]
			get
			{
				return this.<MinChanceToRedressWorldPawn>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<MinChanceToRedressWorldPawn>k__BackingField = value;
			}
		}

		public float? FixedBiologicalAge
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedBiologicalAge>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FixedBiologicalAge>k__BackingField = value;
			}
		}

		public float? FixedChronologicalAge
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedChronologicalAge>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FixedChronologicalAge>k__BackingField = value;
			}
		}

		public Gender? FixedGender
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedGender>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FixedGender>k__BackingField = value;
			}
		}

		public float? FixedMelanin
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedMelanin>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FixedMelanin>k__BackingField = value;
			}
		}

		public string FixedLastName
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedLastName>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FixedLastName>k__BackingField = value;
			}
		}

		public void SetFixedLastName(string fixedLastName)
		{
			if (this.FixedLastName != null)
			{
				Log.Error("Last name is already a fixed value: " + this.FixedLastName + ".", false);
			}
			else
			{
				this.FixedLastName = fixedLastName;
			}
		}

		public void SetFixedMelanin(float fixedMelanin)
		{
			if (this.FixedMelanin != null)
			{
				Log.Error("Melanin is already a fixed value: " + this.FixedMelanin + ".", false);
			}
			else
			{
				this.FixedMelanin = new float?(fixedMelanin);
			}
		}

		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"kindDef=",
				this.KindDef,
				", context=",
				this.Context,
				", faction=",
				this.Faction,
				", tile=",
				this.Tile,
				", forceGenerateNewPawn=",
				this.ForceGenerateNewPawn,
				", newborn=",
				this.Newborn,
				", allowDead=",
				this.AllowDead,
				", allowDowned=",
				this.AllowDowned,
				", canGeneratePawnRelations=",
				this.CanGeneratePawnRelations,
				", mustBeCapableOfViolence=",
				this.MustBeCapableOfViolence,
				", colonistRelationChanceFactor=",
				this.ColonistRelationChanceFactor,
				", forceAddFreeWarmLayerIfNeeded=",
				this.ForceAddFreeWarmLayerIfNeeded,
				", allowGay=",
				this.AllowGay,
				", allowFood=",
				this.AllowFood,
				", inhabitant=",
				this.Inhabitant,
				", certainlyBeenInCryptosleep=",
				this.CertainlyBeenInCryptosleep,
				", validatorPreGear=",
				this.ValidatorPreGear,
				", validatorPostGear=",
				this.ValidatorPostGear,
				", fixedBiologicalAge=",
				this.FixedBiologicalAge,
				", fixedChronologicalAge=",
				this.FixedChronologicalAge,
				", fixedGender=",
				this.FixedGender,
				", fixedMelanin=",
				this.FixedMelanin,
				", fixedLastName=",
				this.FixedLastName
			});
		}
	}
}
