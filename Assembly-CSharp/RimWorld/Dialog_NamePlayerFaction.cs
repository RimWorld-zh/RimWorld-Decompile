using System;

namespace RimWorld
{
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		public Dialog_NamePlayerFaction()
		{
			base.curName = NameGenerator.GenerateName(RulePackDefOf.NamerFactionPlayerRandomized, (Predicate<string>)null, false);
			base.nameMessageKey = "NamePlayerFactionMessage";
			base.gainedNameMessageKey = "PlayerFactionGainsName";
			base.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}
	}
}
