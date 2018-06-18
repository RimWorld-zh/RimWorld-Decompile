using System;
using System.Runtime.InteropServices;
using RimWorld;

namespace Verse
{
	// Token: 0x02000D52 RID: 3410
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct PawnGenerationRequest
	{
		// Token: 0x06004BBF RID: 19391 RVA: 0x00279744 File Offset: 0x00277B44
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

		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06004BC0 RID: 19392 RVA: 0x0027986C File Offset: 0x00277C6C
		// (set) Token: 0x06004BC1 RID: 19393 RVA: 0x00279886 File Offset: 0x00277C86
		public PawnKindDef KindDef { get; private set; }

		// Token: 0x17000C3D RID: 3133
		// (get) Token: 0x06004BC2 RID: 19394 RVA: 0x00279890 File Offset: 0x00277C90
		// (set) Token: 0x06004BC3 RID: 19395 RVA: 0x002798AA File Offset: 0x00277CAA
		public PawnGenerationContext Context { get; private set; }

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06004BC4 RID: 19396 RVA: 0x002798B4 File Offset: 0x00277CB4
		// (set) Token: 0x06004BC5 RID: 19397 RVA: 0x002798CE File Offset: 0x00277CCE
		public Faction Faction { get; private set; }

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x06004BC6 RID: 19398 RVA: 0x002798D8 File Offset: 0x00277CD8
		// (set) Token: 0x06004BC7 RID: 19399 RVA: 0x002798F2 File Offset: 0x00277CF2
		public int Tile { get; private set; }

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x06004BC8 RID: 19400 RVA: 0x002798FC File Offset: 0x00277CFC
		// (set) Token: 0x06004BC9 RID: 19401 RVA: 0x00279916 File Offset: 0x00277D16
		public bool ForceGenerateNewPawn { get; private set; }

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x06004BCA RID: 19402 RVA: 0x00279920 File Offset: 0x00277D20
		// (set) Token: 0x06004BCB RID: 19403 RVA: 0x0027993A File Offset: 0x00277D3A
		public bool Newborn { get; private set; }

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x06004BCC RID: 19404 RVA: 0x00279944 File Offset: 0x00277D44
		// (set) Token: 0x06004BCD RID: 19405 RVA: 0x0027995E File Offset: 0x00277D5E
		public bool AllowDead { get; private set; }

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x06004BCE RID: 19406 RVA: 0x00279968 File Offset: 0x00277D68
		// (set) Token: 0x06004BCF RID: 19407 RVA: 0x00279982 File Offset: 0x00277D82
		public bool AllowDowned { get; private set; }

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06004BD0 RID: 19408 RVA: 0x0027998C File Offset: 0x00277D8C
		// (set) Token: 0x06004BD1 RID: 19409 RVA: 0x002799A6 File Offset: 0x00277DA6
		public bool CanGeneratePawnRelations { get; private set; }

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x002799B0 File Offset: 0x00277DB0
		// (set) Token: 0x06004BD3 RID: 19411 RVA: 0x002799CA File Offset: 0x00277DCA
		public bool MustBeCapableOfViolence { get; private set; }

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06004BD4 RID: 19412 RVA: 0x002799D4 File Offset: 0x00277DD4
		// (set) Token: 0x06004BD5 RID: 19413 RVA: 0x002799EE File Offset: 0x00277DEE
		public float ColonistRelationChanceFactor { get; private set; }

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06004BD6 RID: 19414 RVA: 0x002799F8 File Offset: 0x00277DF8
		// (set) Token: 0x06004BD7 RID: 19415 RVA: 0x00279A12 File Offset: 0x00277E12
		public bool ForceAddFreeWarmLayerIfNeeded { get; private set; }

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06004BD8 RID: 19416 RVA: 0x00279A1C File Offset: 0x00277E1C
		// (set) Token: 0x06004BD9 RID: 19417 RVA: 0x00279A36 File Offset: 0x00277E36
		public bool AllowGay { get; private set; }

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06004BDA RID: 19418 RVA: 0x00279A40 File Offset: 0x00277E40
		// (set) Token: 0x06004BDB RID: 19419 RVA: 0x00279A5A File Offset: 0x00277E5A
		public bool AllowFood { get; private set; }

		// Token: 0x17000C4A RID: 3146
		// (get) Token: 0x06004BDC RID: 19420 RVA: 0x00279A64 File Offset: 0x00277E64
		// (set) Token: 0x06004BDD RID: 19421 RVA: 0x00279A7E File Offset: 0x00277E7E
		public bool Inhabitant { get; private set; }

		// Token: 0x17000C4B RID: 3147
		// (get) Token: 0x06004BDE RID: 19422 RVA: 0x00279A88 File Offset: 0x00277E88
		// (set) Token: 0x06004BDF RID: 19423 RVA: 0x00279AA2 File Offset: 0x00277EA2
		public bool CertainlyBeenInCryptosleep { get; private set; }

		// Token: 0x17000C4C RID: 3148
		// (get) Token: 0x06004BE0 RID: 19424 RVA: 0x00279AAC File Offset: 0x00277EAC
		// (set) Token: 0x06004BE1 RID: 19425 RVA: 0x00279AC6 File Offset: 0x00277EC6
		public bool ForceRedressWorldPawnIfFormerColonist { get; private set; }

		// Token: 0x17000C4D RID: 3149
		// (get) Token: 0x06004BE2 RID: 19426 RVA: 0x00279AD0 File Offset: 0x00277ED0
		// (set) Token: 0x06004BE3 RID: 19427 RVA: 0x00279AEA File Offset: 0x00277EEA
		public bool WorldPawnFactionDoesntMatter { get; private set; }

		// Token: 0x17000C4E RID: 3150
		// (get) Token: 0x06004BE4 RID: 19428 RVA: 0x00279AF4 File Offset: 0x00277EF4
		// (set) Token: 0x06004BE5 RID: 19429 RVA: 0x00279B0E File Offset: 0x00277F0E
		public Predicate<Pawn> ValidatorPreGear { get; private set; }

		// Token: 0x17000C4F RID: 3151
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x00279B18 File Offset: 0x00277F18
		// (set) Token: 0x06004BE7 RID: 19431 RVA: 0x00279B32 File Offset: 0x00277F32
		public Predicate<Pawn> ValidatorPostGear { get; private set; }

		// Token: 0x17000C50 RID: 3152
		// (get) Token: 0x06004BE8 RID: 19432 RVA: 0x00279B3C File Offset: 0x00277F3C
		// (set) Token: 0x06004BE9 RID: 19433 RVA: 0x00279B56 File Offset: 0x00277F56
		public float? MinChanceToRedressWorldPawn { get; private set; }

		// Token: 0x17000C51 RID: 3153
		// (get) Token: 0x06004BEA RID: 19434 RVA: 0x00279B60 File Offset: 0x00277F60
		// (set) Token: 0x06004BEB RID: 19435 RVA: 0x00279B7A File Offset: 0x00277F7A
		public float? FixedBiologicalAge { get; private set; }

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x06004BEC RID: 19436 RVA: 0x00279B84 File Offset: 0x00277F84
		// (set) Token: 0x06004BED RID: 19437 RVA: 0x00279B9E File Offset: 0x00277F9E
		public float? FixedChronologicalAge { get; private set; }

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06004BEE RID: 19438 RVA: 0x00279BA8 File Offset: 0x00277FA8
		// (set) Token: 0x06004BEF RID: 19439 RVA: 0x00279BC2 File Offset: 0x00277FC2
		public Gender? FixedGender { get; private set; }

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06004BF0 RID: 19440 RVA: 0x00279BCC File Offset: 0x00277FCC
		// (set) Token: 0x06004BF1 RID: 19441 RVA: 0x00279BE6 File Offset: 0x00277FE6
		public float? FixedMelanin { get; private set; }

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06004BF2 RID: 19442 RVA: 0x00279BF0 File Offset: 0x00277FF0
		// (set) Token: 0x06004BF3 RID: 19443 RVA: 0x00279C0A File Offset: 0x0027800A
		public string FixedLastName { get; private set; }

		// Token: 0x06004BF4 RID: 19444 RVA: 0x00279C13 File Offset: 0x00278013
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

		// Token: 0x06004BF5 RID: 19445 RVA: 0x00279C4C File Offset: 0x0027804C
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

		// Token: 0x06004BF6 RID: 19446 RVA: 0x00279CA0 File Offset: 0x002780A0
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
