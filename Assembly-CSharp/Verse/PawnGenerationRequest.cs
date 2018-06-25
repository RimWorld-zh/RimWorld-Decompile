using System;
using System.Runtime.InteropServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D51 RID: 3409
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PawnGenerationRequest
	{
		// Token: 0x06004BD7 RID: 19415 RVA: 0x0027AE08 File Offset: 0x00279208
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

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x0027AF30 File Offset: 0x00279330
		// (set) Token: 0x06004BD9 RID: 19417 RVA: 0x0027AF4A File Offset: 0x0027934A
		public PawnKindDef KindDef { get; private set; }

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x0027AF54 File Offset: 0x00279354
		// (set) Token: 0x06004BDB RID: 19419 RVA: 0x0027AF6E File Offset: 0x0027936E
		public PawnGenerationContext Context { get; private set; }

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x0027AF78 File Offset: 0x00279378
		// (set) Token: 0x06004BDD RID: 19421 RVA: 0x0027AF92 File Offset: 0x00279392
		public Faction Faction { get; private set; }

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x0027AF9C File Offset: 0x0027939C
		// (set) Token: 0x06004BDF RID: 19423 RVA: 0x0027AFB6 File Offset: 0x002793B6
		public int Tile { get; private set; }

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004BE0 RID: 19424 RVA: 0x0027AFC0 File Offset: 0x002793C0
		// (set) Token: 0x06004BE1 RID: 19425 RVA: 0x0027AFDA File Offset: 0x002793DA
		public bool ForceGenerateNewPawn { get; private set; }

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004BE2 RID: 19426 RVA: 0x0027AFE4 File Offset: 0x002793E4
		// (set) Token: 0x06004BE3 RID: 19427 RVA: 0x0027AFFE File Offset: 0x002793FE
		public bool Newborn { get; private set; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004BE4 RID: 19428 RVA: 0x0027B008 File Offset: 0x00279408
		// (set) Token: 0x06004BE5 RID: 19429 RVA: 0x0027B022 File Offset: 0x00279422
		public bool AllowDead { get; private set; }

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x0027B02C File Offset: 0x0027942C
		// (set) Token: 0x06004BE7 RID: 19431 RVA: 0x0027B046 File Offset: 0x00279446
		public bool AllowDowned { get; private set; }

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x0027B050 File Offset: 0x00279450
		// (set) Token: 0x06004BE9 RID: 19433 RVA: 0x0027B06A File Offset: 0x0027946A
		public bool CanGeneratePawnRelations { get; private set; }

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004BEA RID: 19434 RVA: 0x0027B074 File Offset: 0x00279474
		// (set) Token: 0x06004BEB RID: 19435 RVA: 0x0027B08E File Offset: 0x0027948E
		public bool MustBeCapableOfViolence { get; private set; }

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004BEC RID: 19436 RVA: 0x0027B098 File Offset: 0x00279498
		// (set) Token: 0x06004BED RID: 19437 RVA: 0x0027B0B2 File Offset: 0x002794B2
		public float ColonistRelationChanceFactor { get; private set; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x0027B0BC File Offset: 0x002794BC
		// (set) Token: 0x06004BEF RID: 19439 RVA: 0x0027B0D6 File Offset: 0x002794D6
		public bool ForceAddFreeWarmLayerIfNeeded { get; private set; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06004BF0 RID: 19440 RVA: 0x0027B0E0 File Offset: 0x002794E0
		// (set) Token: 0x06004BF1 RID: 19441 RVA: 0x0027B0FA File Offset: 0x002794FA
		public bool AllowGay { get; private set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x0027B104 File Offset: 0x00279504
		// (set) Token: 0x06004BF3 RID: 19443 RVA: 0x0027B11E File Offset: 0x0027951E
		public bool AllowFood { get; private set; }

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x0027B128 File Offset: 0x00279528
		// (set) Token: 0x06004BF5 RID: 19445 RVA: 0x0027B142 File Offset: 0x00279542
		public bool Inhabitant { get; private set; }

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x0027B14C File Offset: 0x0027954C
		// (set) Token: 0x06004BF7 RID: 19447 RVA: 0x0027B166 File Offset: 0x00279566
		public bool CertainlyBeenInCryptosleep { get; private set; }

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x0027B170 File Offset: 0x00279570
		// (set) Token: 0x06004BF9 RID: 19449 RVA: 0x0027B18A File Offset: 0x0027958A
		public bool ForceRedressWorldPawnIfFormerColonist { get; private set; }

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06004BFA RID: 19450 RVA: 0x0027B194 File Offset: 0x00279594
		// (set) Token: 0x06004BFB RID: 19451 RVA: 0x0027B1AE File Offset: 0x002795AE
		public bool WorldPawnFactionDoesntMatter { get; private set; }

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06004BFC RID: 19452 RVA: 0x0027B1B8 File Offset: 0x002795B8
		// (set) Token: 0x06004BFD RID: 19453 RVA: 0x0027B1D2 File Offset: 0x002795D2
		public Predicate<Pawn> ValidatorPreGear { get; private set; }

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06004BFE RID: 19454 RVA: 0x0027B1DC File Offset: 0x002795DC
		// (set) Token: 0x06004BFF RID: 19455 RVA: 0x0027B1F6 File Offset: 0x002795F6
		public Predicate<Pawn> ValidatorPostGear { get; private set; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06004C00 RID: 19456 RVA: 0x0027B200 File Offset: 0x00279600
		// (set) Token: 0x06004C01 RID: 19457 RVA: 0x0027B21A File Offset: 0x0027961A
		public float? MinChanceToRedressWorldPawn { get; private set; }

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06004C02 RID: 19458 RVA: 0x0027B224 File Offset: 0x00279624
		// (set) Token: 0x06004C03 RID: 19459 RVA: 0x0027B23E File Offset: 0x0027963E
		public float? FixedBiologicalAge { get; private set; }

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x0027B248 File Offset: 0x00279648
		// (set) Token: 0x06004C05 RID: 19461 RVA: 0x0027B262 File Offset: 0x00279662
		public float? FixedChronologicalAge { get; private set; }

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x0027B26C File Offset: 0x0027966C
		// (set) Token: 0x06004C07 RID: 19463 RVA: 0x0027B286 File Offset: 0x00279686
		public Gender? FixedGender { get; private set; }

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06004C08 RID: 19464 RVA: 0x0027B290 File Offset: 0x00279690
		// (set) Token: 0x06004C09 RID: 19465 RVA: 0x0027B2AA File Offset: 0x002796AA
		public float? FixedMelanin { get; private set; }

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x0027B2B4 File Offset: 0x002796B4
		// (set) Token: 0x06004C0B RID: 19467 RVA: 0x0027B2CE File Offset: 0x002796CE
		public string FixedLastName { get; private set; }

		// Token: 0x06004C0C RID: 19468 RVA: 0x0027B2D7 File Offset: 0x002796D7
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

		// Token: 0x06004C0D RID: 19469 RVA: 0x0027B310 File Offset: 0x00279710
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

		// Token: 0x06004C0E RID: 19470 RVA: 0x0027B364 File Offset: 0x00279764
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
