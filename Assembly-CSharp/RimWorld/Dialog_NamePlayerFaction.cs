using System;
using System.Runtime.CompilerServices;

namespace RimWorld
{
	public class Dialog_NamePlayerFaction : Dialog_GiveName
	{
		[CompilerGenerated]
		private static Func<string> <>f__am$cache0;

		public Dialog_NamePlayerFaction()
		{
			this.nameGenerator = (() => NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null));
			this.curName = this.nameGenerator();
			this.nameMessageKey = "NamePlayerFactionMessage";
			this.gainedNameMessageKey = "PlayerFactionGainsName";
			this.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
		}

		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		[CompilerGenerated]
		private static string <Dialog_NamePlayerFaction>m__0()
		{
			return NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null);
		}
	}
}
