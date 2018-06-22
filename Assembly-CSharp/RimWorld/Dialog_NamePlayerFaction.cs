using System;

namespace RimWorld
{
	// Token: 0x02000803 RID: 2051
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x06002DD7 RID: 11735 RVA: 0x00182460 File Offset: 0x00180860
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x001824CC File Offset: 0x001808CC
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x001824E7 File Offset: 0x001808E7
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
