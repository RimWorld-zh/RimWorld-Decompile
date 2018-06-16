using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080A RID: 2058
	public class Dialog_NamePlayerFactionBase : Dialog_GiveName
	{
		// Token: 0x06002DEA RID: 11754 RVA: 0x001825C4 File Offset: 0x001809C4
		public Dialog_NamePlayerFactionBase(FactionBase factionBase)
		{
			this.factionBase = factionBase;
			if (factionBase.HasMap && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = factionBase.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, null, false, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionBaseMessage";
			this.gainedNameMessageKey = "PlayerFactionBaseGainsName";
			this.invalidNameMessageKey = "PlayerFactionBaseNameIsInvalid";
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x0018266F File Offset: 0x00180A6F
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x001826A0 File Offset: 0x00180AA0
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x001826BB File Offset: 0x00180ABB
		protected override void Named(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}

		// Token: 0x0400185C RID: 6236
		private FactionBase factionBase;
	}
}
