using RimWorld.Planet;
using System;
using Verse;

namespace RimWorld
{
	public class Dialog_NamePlayerFactionAndBase : Dialog_GiveName
	{
		private FactionBase factionBase;

		public Dialog_NamePlayerFactionAndBase(FactionBase factionBase)
		{
			this.factionBase = factionBase;
			if (factionBase.HasMap && factionBase.Map.mapPawns.FreeColonistsSpawnedCount != 0)
			{
				base.suggestingPawn = factionBase.Map.mapPawns.FreeColonistsSpawned.RandomElement();
			}
			base.curName = NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMakerPlayer, (Predicate<string>)null, false, (string)null);
			base.nameMessageKey = "NamePlayerFactionMessage";
			base.invalidNameMessageKey = "PlayerFactionNameIsInvalid";
			base.useSecondName = true;
			base.curSecondName = NameGenerator.GenerateName(Faction.OfPlayer.def.baseNameMakerPlayer, (Predicate<string>)null, false, (string)null);
			base.secondNameMessageKey = "NamePlayerFactionBaseMessage_NameFactionContinuation";
			base.invalidSecondNameMessageKey = "PlayerFactionBaseNameIsInvalid";
			base.gainedNameMessageKey = "PlayerFactionAndBaseGainsName";
		}

		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.VisibleMap = this.factionBase.Map;
			}
		}

		protected override bool IsValidName(string s)
		{
			return NamePlayerFactionDialogUtility.IsValidName(s);
		}

		protected override bool IsValidSecondName(string s)
		{
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		protected override void Named(string s)
		{
			NamePlayerFactionDialogUtility.Named(s);
		}

		protected override void NamedSecond(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}
	}
}
