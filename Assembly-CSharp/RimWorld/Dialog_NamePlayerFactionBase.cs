using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000808 RID: 2056
	public class Dialog_NamePlayerFactionBase : Dialog_GiveName
	{
		// Token: 0x0400185A RID: 6234
		private FactionBase factionBase;

		// Token: 0x06002DE9 RID: 11753 RVA: 0x00182980 File Offset: 0x00180D80
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

		// Token: 0x06002DEA RID: 11754 RVA: 0x00182A2B File Offset: 0x00180E2B
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
			}
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x00182A5C File Offset: 0x00180E5C
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x00182A77 File Offset: 0x00180E77
		protected override void Named(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}
	}
}
