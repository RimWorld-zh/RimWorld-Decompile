using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200080A RID: 2058
	public class Dialog_NamePlayerFactionBase : Dialog_GiveName
	{
		// Token: 0x06002DEC RID: 11756 RVA: 0x00182658 File Offset: 0x00180A58
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

		// Token: 0x06002DED RID: 11757 RVA: 0x00182703 File Offset: 0x00180B03
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
			}
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x00182734 File Offset: 0x00180B34
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x0018274F File Offset: 0x00180B4F
		protected override void Named(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}

		// Token: 0x0400185C RID: 6236
		private FactionBase factionBase;
	}
}
