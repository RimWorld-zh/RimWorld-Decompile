using System;
using System.Runtime.InteropServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D4F RID: 3407
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PawnGenerationRequest
	{
		// Token: 0x06004BD3 RID: 19411 RVA: 0x0027ACDC File Offset: 0x002790DC
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

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06004BD4 RID: 19412 RVA: 0x0027AE04 File Offset: 0x00279204
		// (set) Token: 0x06004BD5 RID: 19413 RVA: 0x0027AE1E File Offset: 0x0027921E
		public PawnKindDef KindDef { get; private set; }

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06004BD6 RID: 19414 RVA: 0x0027AE28 File Offset: 0x00279228
		// (set) Token: 0x06004BD7 RID: 19415 RVA: 0x0027AE42 File Offset: 0x00279242
		public PawnGenerationContext Context { get; private set; }

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x0027AE4C File Offset: 0x0027924C
		// (set) Token: 0x06004BD9 RID: 19417 RVA: 0x0027AE66 File Offset: 0x00279266
		public Faction Faction { get; private set; }

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x0027AE70 File Offset: 0x00279270
		// (set) Token: 0x06004BDB RID: 19419 RVA: 0x0027AE8A File Offset: 0x0027928A
		public int Tile { get; private set; }

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x0027AE94 File Offset: 0x00279294
		// (set) Token: 0x06004BDD RID: 19421 RVA: 0x0027AEAE File Offset: 0x002792AE
		public bool ForceGenerateNewPawn { get; private set; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x0027AEB8 File Offset: 0x002792B8
		// (set) Token: 0x06004BDF RID: 19423 RVA: 0x0027AED2 File Offset: 0x002792D2
		public bool Newborn { get; private set; }

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06004BE0 RID: 19424 RVA: 0x0027AEDC File Offset: 0x002792DC
		// (set) Token: 0x06004BE1 RID: 19425 RVA: 0x0027AEF6 File Offset: 0x002792F6
		public bool AllowDead { get; private set; }

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004BE2 RID: 19426 RVA: 0x0027AF00 File Offset: 0x00279300
		// (set) Token: 0x06004BE3 RID: 19427 RVA: 0x0027AF1A File Offset: 0x0027931A
		public bool AllowDowned { get; private set; }

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004BE4 RID: 19428 RVA: 0x0027AF24 File Offset: 0x00279324
		// (set) Token: 0x06004BE5 RID: 19429 RVA: 0x0027AF3E File Offset: 0x0027933E
		public bool CanGeneratePawnRelations { get; private set; }

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x0027AF48 File Offset: 0x00279348
		// (set) Token: 0x06004BE7 RID: 19431 RVA: 0x0027AF62 File Offset: 0x00279362
		public bool MustBeCapableOfViolence { get; private set; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x0027AF6C File Offset: 0x0027936C
		// (set) Token: 0x06004BE9 RID: 19433 RVA: 0x0027AF86 File Offset: 0x00279386
		public float ColonistRelationChanceFactor { get; private set; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06004BEA RID: 19434 RVA: 0x0027AF90 File Offset: 0x00279390
		// (set) Token: 0x06004BEB RID: 19435 RVA: 0x0027AFAA File Offset: 0x002793AA
		public bool ForceAddFreeWarmLayerIfNeeded { get; private set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06004BEC RID: 19436 RVA: 0x0027AFB4 File Offset: 0x002793B4
		// (set) Token: 0x06004BED RID: 19437 RVA: 0x0027AFCE File Offset: 0x002793CE
		public bool AllowGay { get; private set; }

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x0027AFD8 File Offset: 0x002793D8
		// (set) Token: 0x06004BEF RID: 19439 RVA: 0x0027AFF2 File Offset: 0x002793F2
		public bool AllowFood { get; private set; }

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06004BF0 RID: 19440 RVA: 0x0027AFFC File Offset: 0x002793FC
		// (set) Token: 0x06004BF1 RID: 19441 RVA: 0x0027B016 File Offset: 0x00279416
		public bool Inhabitant { get; private set; }

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x0027B020 File Offset: 0x00279420
		// (set) Token: 0x06004BF3 RID: 19443 RVA: 0x0027B03A File Offset: 0x0027943A
		public bool CertainlyBeenInCryptosleep { get; private set; }

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06004BF4 RID: 19444 RVA: 0x0027B044 File Offset: 0x00279444
		// (set) Token: 0x06004BF5 RID: 19445 RVA: 0x0027B05E File Offset: 0x0027945E
		public bool ForceRedressWorldPawnIfFormerColonist { get; private set; }

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06004BF6 RID: 19446 RVA: 0x0027B068 File Offset: 0x00279468
		// (set) Token: 0x06004BF7 RID: 19447 RVA: 0x0027B082 File Offset: 0x00279482
		public bool WorldPawnFactionDoesntMatter { get; private set; }

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06004BF8 RID: 19448 RVA: 0x0027B08C File Offset: 0x0027948C
		// (set) Token: 0x06004BF9 RID: 19449 RVA: 0x0027B0A6 File Offset: 0x002794A6
		public Predicate<Pawn> ValidatorPreGear { get; private set; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06004BFA RID: 19450 RVA: 0x0027B0B0 File Offset: 0x002794B0
		// (set) Token: 0x06004BFB RID: 19451 RVA: 0x0027B0CA File Offset: 0x002794CA
		public Predicate<Pawn> ValidatorPostGear { get; private set; }

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06004BFC RID: 19452 RVA: 0x0027B0D4 File Offset: 0x002794D4
		// (set) Token: 0x06004BFD RID: 19453 RVA: 0x0027B0EE File Offset: 0x002794EE
		public float? MinChanceToRedressWorldPawn { get; private set; }

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06004BFE RID: 19454 RVA: 0x0027B0F8 File Offset: 0x002794F8
		// (set) Token: 0x06004BFF RID: 19455 RVA: 0x0027B112 File Offset: 0x00279512
		public float? FixedBiologicalAge { get; private set; }

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06004C00 RID: 19456 RVA: 0x0027B11C File Offset: 0x0027951C
		// (set) Token: 0x06004C01 RID: 19457 RVA: 0x0027B136 File Offset: 0x00279536
		public float? FixedChronologicalAge { get; private set; }

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06004C02 RID: 19458 RVA: 0x0027B140 File Offset: 0x00279540
		// (set) Token: 0x06004C03 RID: 19459 RVA: 0x0027B15A File Offset: 0x0027955A
		public Gender? FixedGender { get; private set; }

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06004C04 RID: 19460 RVA: 0x0027B164 File Offset: 0x00279564
		// (set) Token: 0x06004C05 RID: 19461 RVA: 0x0027B17E File Offset: 0x0027957E
		public float? FixedMelanin { get; private set; }

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x06004C06 RID: 19462 RVA: 0x0027B188 File Offset: 0x00279588
		// (set) Token: 0x06004C07 RID: 19463 RVA: 0x0027B1A2 File Offset: 0x002795A2
		public string FixedLastName { get; private set; }

		// Token: 0x06004C08 RID: 19464 RVA: 0x0027B1AB File Offset: 0x002795AB
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

		// Token: 0x06004C09 RID: 19465 RVA: 0x0027B1E4 File Offset: 0x002795E4
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

		// Token: 0x06004C0A RID: 19466 RVA: 0x0027B238 File Offset: 0x00279638
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
