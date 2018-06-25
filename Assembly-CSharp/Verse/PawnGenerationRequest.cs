using System;
using System.Runtime.InteropServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D52 RID: 3410
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PawnGenerationRequest
	{
		// Token: 0x06004BD7 RID: 19415 RVA: 0x0027B0E8 File Offset: 0x002794E8
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
		// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x0027B210 File Offset: 0x00279610
		// (set) Token: 0x06004BD9 RID: 19417 RVA: 0x0027B22A File Offset: 0x0027962A
		public PawnKindDef KindDef { get; private set; }

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x0027B234 File Offset: 0x00279634
		// (set) Token: 0x06004BDB RID: 19419 RVA: 0x0027B24E File Offset: 0x0027964E
		public PawnGenerationContext Context { get; private set; }

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x0027B258 File Offset: 0x00279658
		// (set) Token: 0x06004BDD RID: 19421 RVA: 0x0027B272 File Offset: 0x00279672
		public Faction Faction { get; private set; }

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x0027B27C File Offset: 0x0027967C
		// (set) Token: 0x06004BDF RID: 19423 RVA: 0x0027B296 File Offset: 0x00279696
		public int Tile { get; private set; }

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004BE0 RID: 19424 RVA: 0x0027B2A0 File Offset: 0x002796A0
		// (set) Token: 0x06004BE1 RID: 19425 RVA: 0x0027B2BA File Offset: 0x002796BA
		public bool ForceGenerateNewPawn { get; private set; }

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004BE2 RID: 19426 RVA: 0x0027B2C4 File Offset: 0x002796C4
		// (set) Token: 0x06004BE3 RID: 19427 RVA: 0x0027B2DE File Offset: 0x002796DE
		public bool Newborn { get; private set; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004BE4 RID: 19428 RVA: 0x0027B2E8 File Offset: 0x002796E8
		// (set) Token: 0x06004BE5 RID: 19429 RVA: 0x0027B302 File Offset: 0x00279702
		public bool AllowDead { get; private set; }

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x0027B30C File Offset: 0x0027970C
		// (set) Token: 0x06004BE7 RID: 19431 RVA: 0x0027B326 File Offset: 0x00279726
		public bool AllowDowned { get; private set; }

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x0027B330 File Offset: 0x00279730
		// (set) Token: 0x06004BE9 RID: 19433 RVA: 0x0027B34A File Offset: 0x0027974A
		public bool CanGeneratePawnRelations { get; private set; }

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004BEA RID: 19434 RVA: 0x0027B354 File Offset: 0x00279754
		// (set) Token: 0x06004BEB RID: 19435 RVA: 0x0027B36E File Offset: 0x0027976E
		public bool MustBeCapableOfViolence { get; private set; }

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004BEC RID: 19436 RVA: 0x0027B378 File Offset: 0x00279778
		// (set) Token: 0x06004BED RID: 19437 RVA: 0x0027B392 File Offset: 0x00279792
		public float ColonistRelationChanceFactor { get; private set; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x0027B39C File Offset: 0x0027979C
		// (set) Token: 0x06004BEF RID: 19439 RVA: 0x0027B3B6 File Offset: 0x002797B6
		public bool ForceAddFreeWarmLayerIfNeeded { get; private set; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06004BF0 RID: 19440 RVA: 0x0027B3C0 File Offset: 0x002797C0
		// (set) Token: 0x06004BF1 RID: 19441 RVA: 0x0027B3DA File Offset: 0x002797DA
		public bool AllowGay { get; private set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x0027B3E4 File Offset: 0x002797E4
		// (set) Token: 0x06004BF3 RID: 19443 RVA: 0x0027B3FE File Offset: 0x002797FE
		public bool AllowFood { get; private set; }

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x0027B408 File Offset: 0x00279808
		// (set) Token: 0x06004BF5 RID: 19445 RVA: 0x0027B422 File Offset: 0x00279822
		public bool Inhabitant { get; private set; }

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x0027B42C File Offset: 0x0027982C
		// (set) Token: 0x06004BF7 RID: 19447 RVA: 0x0027B446 File Offset: 0x00279846
		public bool CertainlyBeenInCryptosleep { get; private set; }

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x0027B450 File Offset: 0x00279850
		// (set) Token: 0x06004BF9 RID: 19449 RVA: 0x0027B46A File Offset: 0x0027986A
		public bool ForceRedressWorldPawnIfFormerColonist { get; private set; }

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06004BFA RID: 19450 RVA: 0x0027B474 File Offset: 0x00279874
		// (set) Token: 0x06004BFB RID: 19451 RVA: 0x0027B48E File Offset: 0x0027988E
		public bool WorldPawnFactionDoesntMatter { get; private set; }

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06004BFC RID: 19452 RVA: 0x0027B498 File Offset: 0x00279898
		// (set) Token: 0x06004BFD RID: 19453 RVA: 0x0027B4B2 File Offset: 0x002798B2
		public Predicate<Pawn> ValidatorPreGear { get; private set; }

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06004BFE RID: 19454 RVA: 0x0027B4BC File Offset: 0x002798BC
		// (set) Token: 0x06004BFF RID: 19455 RVA: 0x0027B4D6 File Offset: 0x002798D6
		public Predicate<Pawn> ValidatorPostGear { get; private set; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06004C00 RID: 19456 RVA: 0x0027B4E0 File Offset: 0x002798E0
		// (set) Token: 0x06004C01 RID: 19457 RVA: 0x0027B4FA File Offset: 0x002798FA
		public float? MinChanceToRedressWorldPawn { get; private set; }

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06004C02 RID: 19458 RVA: 0x0027B504 File Offset: 0x00279904
		// (set) Token: 0x06004C03 RID: 19459 RVA: 0x0027B51E File Offset: 0x0027991E
		public float? FixedBiologicalAge { get; private set; }

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x0027B528 File Offset: 0x00279928
		// (set) Token: 0x06004C05 RID: 19461 RVA: 0x0027B542 File Offset: 0x00279942
		public float? FixedChronologicalAge { get; private set; }

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x0027B54C File Offset: 0x0027994C
		// (set) Token: 0x06004C07 RID: 19463 RVA: 0x0027B566 File Offset: 0x00279966
		public Gender? FixedGender { get; private set; }

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06004C08 RID: 19464 RVA: 0x0027B570 File Offset: 0x00279970
		// (set) Token: 0x06004C09 RID: 19465 RVA: 0x0027B58A File Offset: 0x0027998A
		public float? FixedMelanin { get; private set; }

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06004C0A RID: 19466 RVA: 0x0027B594 File Offset: 0x00279994
		// (set) Token: 0x06004C0B RID: 19467 RVA: 0x0027B5AE File Offset: 0x002799AE
		public string FixedLastName { get; private set; }

		// Token: 0x06004C0C RID: 19468 RVA: 0x0027B5B7 File Offset: 0x002799B7
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

		// Token: 0x06004C0D RID: 19469 RVA: 0x0027B5F0 File Offset: 0x002799F0
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

		// Token: 0x06004C0E RID: 19470 RVA: 0x0027B644 File Offset: 0x00279A44
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
