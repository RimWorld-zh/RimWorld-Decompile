using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class Dialog_NamePlayerFactionBase : Dialog_GiveName
	{
		private FactionBase factionBase;

		[CompilerGenerated]
		private static Func<string> <>f__am$cache0;

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
			return NamePlayerFactionBaseDialogUtility.IsValidName(s);
		}

		protected override void Named(string s)
		{
			NamePlayerFactionBaseDialogUtility.Named(this.factionBase, s);
		}

		[CompilerGenerated]
		private static string <Dialog_NamePlayerFactionBase>m__0()
		{
			return NameGenerator.GenerateName(Faction.OfPlayer.def.settlementNameMaker, null, false, null, null);
		}
	}
}
