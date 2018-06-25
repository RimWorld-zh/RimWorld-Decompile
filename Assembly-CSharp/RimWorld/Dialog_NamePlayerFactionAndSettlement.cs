using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000807 RID: 2055
	public class Dialog_NamePlayerFactionAndSettlement : Dialog_GiveName
	{
		// Token: 0x04001857 RID: 6231
		private FactionBase factionBase;

		// Token: 0x06002DE1 RID: 11745 RVA: 0x001827AC File Offset: 0x00180BAC
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

		// Token: 0x06002DE2 RID: 11746 RVA: 0x001828A8 File Offset: 0x00180CA8
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
			}
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x001828D8 File Offset: 0x00180CD8
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x001828F4 File Offset: 0x00180CF4
		protected override bool IsValidSecondName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x0018290F File Offset: 0x00180D0F
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x00182918 File Offset: 0x00180D18
		protected override void NamedSecond(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}
	}
}
