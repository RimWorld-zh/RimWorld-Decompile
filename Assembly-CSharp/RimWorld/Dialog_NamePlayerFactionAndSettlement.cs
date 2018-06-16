using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000809 RID: 2057
	public class Dialog_NamePlayerFactionAndSettlement : Dialog_GiveName
	{
		// Token: 0x06002DE2 RID: 11746 RVA: 0x001823F0 File Offset: 0x001807F0
		public Dialog_NamePlayerFactionAndSettlement(FactionBase factionBase)
		{
			this.factionBase = factionBase;
			if (factionBase.HasMap && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = factionBase.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
			this.useSecondName = true;
			this.secondNameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, null, false, null));
			this.curSecondName = this.secondNameGenerator();
			this.secondNameMessageKey = "NamePlayerFactionBaseMessage_NameFactionContinuation";
			this.invalidSecondNameMessageKey = "PlayerFactionBaseNameIsInvalid";
			this.gainedNameMessageKey = "PlayerFactionAndBaseGainsName";
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x001824EC File Offset: 0x001808EC
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x0018251C File Offset: 0x0018091C
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x00182538 File Offset: 0x00180938
		protected override bool IsValidSecondName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x00182553 File Offset: 0x00180953
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x0018255C File Offset: 0x0018095C
		protected override void NamedSecond(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}

		// Token: 0x04001859 RID: 6233
		private FactionBase factionBase;
	}
}
