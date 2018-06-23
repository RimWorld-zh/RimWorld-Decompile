using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000806 RID: 2054
	public class Dialog_NamePlayerFactionBase : Dialog_GiveName
	{
		// Token: 0x0400185A RID: 6234
		private FactionBase factionBase;

		// Token: 0x06002DE5 RID: 11749 RVA: 0x00182830 File Offset: 0x00180C30
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

		// Token: 0x06002DE6 RID: 11750 RVA: 0x001828DB File Offset: 0x00180CDB
		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
			}
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x0018290C File Offset: 0x00180D0C
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x00182927 File Offset: 0x00180D27
		protected override void Named(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}
	}
}
