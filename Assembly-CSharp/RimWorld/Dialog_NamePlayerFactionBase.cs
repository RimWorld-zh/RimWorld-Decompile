using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000808 RID: 2056
	public class Dialog_NamePlayerFactionBase : Dialog_GiveName
	{
		// Token: 0x0400185E RID: 6238
		private FactionBase factionBase;

		// Token: 0x06002DE8 RID: 11752 RVA: 0x00182BE4 File Offset: 0x00180FE4
		public Dialog_NamePlayerFactionBase(FactionBase factionBase)
		{
			this.factionBase = factionBase;
			if (factionBase.HasMap && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = factionBase.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, null, false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionBaseMessage";
			this.gainedNameMessageKey = "PlayerFactionBaseGainsName";
			this.invalidNameMessageKey = "PlayerFactionBaseNameIsInvalid";
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x00182C8F File Offset: 0x0018108F
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
			}
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x00182CC0 File Offset: 0x001810C0
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x00182CDB File Offset: 0x001810DB
		protected override void Named(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}
	}
}
