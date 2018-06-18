using System;

namespace RimWorld
{
	// Token: 0x02000807 RID: 2055
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		// Token: 0x06002DDE RID: 11742 RVA: 0x00182288 File Offset: 0x00180688
		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x001822F4 File Offset: 0x001806F4
		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0018230F File Offset: 0x0018070F
		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
