using System;

namespace RimWorld
{
	// Token: 0x02000805 RID: 2053
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x06002DDA RID: 11738 RVA: 0x00182814 File Offset: 0x00180C14
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x00182880 File Offset: 0x00180C80
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x0018289B File Offset: 0x00180C9B
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
