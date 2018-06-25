using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000807 RID: 2055
	public class Dialog_NamePlayerFactionAndSettlement : Dialog_GiveName
	{
		// Token: 0x0400185B RID: 6235
		private FactionBase factionBase;

		// Token: 0x06002DE0 RID: 11744 RVA: 0x00182A10 File Offset: 0x00180E10
		public Dialog_NamePlayerFactionAndSettlement(FactionBase factionBase)
		{
			this.factionBase = factionBase;
			if (factionBase.HasMap && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				this.suggestingPawn = factionBase.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>();
			}
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
			this.useSecondName = true;
			this.secondNameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, null, false, null, null));
			this.curSecondName = this.secondNameGenerator();
			this.secondNameMessageKey = "NamePlayerFactionBaseMessage_NameFactionContinuation";
			this.invalidSecondNameMessageKey = "PlayerFactionBaseNameIsInvalid";
			this.gainedNameMessageKey = "PlayerFactionAndBaseGainsName";
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x00182B0C File Offset: 0x00180F0C
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x00182B3C File Offset: 0x00180F3C
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x00182B58 File Offset: 0x00180F58
		protected override bool IsValidSecondName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x00182B73 File Offset: 0x00180F73
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x00182B7C File Offset: 0x00180F7C
		protected override void NamedSecond(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}
	}
}
