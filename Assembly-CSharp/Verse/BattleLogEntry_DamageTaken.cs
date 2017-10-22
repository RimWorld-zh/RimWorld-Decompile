using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	public class BattleLogEntry_DamageTaken : LogEntry, IDamageResultLog
	{
		private Pawn recipientPawn;

		private RulePackDef ruleDef;

		private List<BodyPartDef> damagedParts;

		private List<bool> damagedPartsDestroyed;

		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.NameStringShort;
			}
		}

		public BattleLogEntry_DamageTaken()
		{
		}

		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef)
		{
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		public void FillTargets(List<BodyPartDef> recipientParts, List<bool> recipientPartsDestroyed)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
		}

		public override bool Concerns(Thing t)
		{
			return t == this.recipientPawn;
		}

		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect((Thing)this.recipientPawn);
		}

		public override string ToGameStringFromPOV(Thing pov)
		{
			string result;
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709);
				result = "[BattleLogEntry_DamageTaken error: null pawn reference]";
			}
			else
			{
				Rand.PushState();
				Rand.Seed = base.randSeed;
				List<Rule> list = new List<Rule>();
				list.AddRange(this.ruleDef.Rules);
				list.AddRange(GrammarUtility.RulesForPawn("recipient", this.recipientPawn));
				Dictionary<string, string> constants = new Dictionary<string, string>();
				list.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.damagedParts, this.damagedPartsDestroyed, constants));
				string text = GrammarResolver.Resolve("logentry", list, constants, "damage taken");
				Rand.PopState();
				result = text;
			}
			return result;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
			Scribe_Collections.Look<BodyPartDef>(ref this.damagedParts, "damagedParts", LookMode.Def, new object[0]);
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, new object[0]);
		}

		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}
	}
}
