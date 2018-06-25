using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class Dialog_NamePlayerFactionAndSettlement : Dialog_GiveName
	{
		private FactionBase factionBase;

		[CompilerGenerated]
		private static Func<string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<string> <>f__am$cache1;

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

		public override void PostOpen()
		{
			base.PostOpen();
			if (this.factionBase.Map != null)
			{
				Current.Game.CurrentMap = this.factionBase.Map;
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

		[CompilerGenerated]
		private static string <Dialog_NamePlayerFactionAndSettlement>m__0()
		{
			return NameGenerator.GenerateName(Faction.OfPlayer.def.factionNameMaker, null, false, null, null);
		}

		[CompilerGenerated]
		private static string <Dialog_NamePlayerFactionAndSettlement>m__1()
		{
			return NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, null, false, null, null);
		}
	}
}
