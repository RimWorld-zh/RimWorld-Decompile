using System;

namespace RimWorld
{
	// Token: 0x02000805 RID: 2053
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x06002DDB RID: 11739 RVA: 0x001825B0 File Offset: 0x001809B0
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x0018261C File Offset: 0x00180A1C
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x00182637 File Offset: 0x00180A37
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
