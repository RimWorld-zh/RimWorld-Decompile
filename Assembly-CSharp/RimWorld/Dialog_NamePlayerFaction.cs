using System;

namespace RimWorld
{
	// Token: 0x02000807 RID: 2055
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x06002DDC RID: 11740 RVA: 0x001821F4 File Offset: 0x001805F4
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x00182260 File Offset: 0x00180660
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x0018227B File Offset: 0x0018067B
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
