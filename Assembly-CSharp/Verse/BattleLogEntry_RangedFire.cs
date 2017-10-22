using RimWorld;
using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	public class BattleLogEntry_RangedFire : LogEntry
	{
		private Pawn initiatorPawn;

		private ThingDef initiatorThing;

		private Pawn recipientPawn;

		private ThingDef recipientThing;

		private ThingDef weaponDef;

		private ThingDef projectileDef;

		private string InitiatorName
		{
			get
			{
				return (this.initiatorPawn == null) ? "null" : this.initiatorPawn.NameStringShort;
			}
		}

		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.NameStringShort;
			}
		}

		public BattleLogEntry_RangedFire()
		{
		}

		public BattleLogEntry_RangedFire(Thing initiator, Thing target, ThingDef weaponDef, ThingDef projectileDef)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (target is Pawn)
			{
				this.recipientPawn = (target as Pawn);
			}
			else if (target != null)
			{
				this.recipientThing = target.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
		}

		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		public override void ClickedFromPOV(Thing pov)
		{
			if (this.recipientPawn != null)
			{
				if (pov != this.initiatorPawn)
				{
					if (pov == this.recipientPawn)
					{
						CameraJumper.TryJumpAndSelect((Thing)this.initiatorPawn);
						return;
					}
					throw new NotImplementedException();
				}
				CameraJumper.TryJumpAndSelect((Thing)this.recipientPawn);
			}
		}

		public override string ToGameStringFromPOV(Thing pov)
		{
			string result;
			if (this.initiatorPawn == null && this.initiatorThing == null)
			{
				Log.ErrorOnce("BattleLogEntry_RangedFire has a null initiator.", 60465709);
				result = "[BattleLogEntry_RangedFire error: null pawn reference]";
			}
			else
			{
				Rand.PushState();
				Rand.Seed = base.randSeed;
				List<Rule> list = new List<Rule>();
				list.AddRange(RulePackDefOf.Combat_RangedFire.Rules);
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				if (this.initiatorPawn != null)
				{
					list.AddRange(GrammarUtility.RulesForPawn("initiator", this.initiatorPawn));
				}
				else if (this.initiatorThing != null)
				{
					list.AddRange(GrammarUtility.RulesForDef("initiator", this.initiatorThing));
				}
				else
				{
					dictionary["initiator_missing"] = "True";
				}
				if (this.recipientPawn != null)
				{
					list.AddRange(GrammarUtility.RulesForPawn("recipient", this.recipientPawn));
				}
				else if (this.recipientThing != null)
				{
					list.AddRange(GrammarUtility.RulesForDef("recipient", this.recipientThing));
				}
				else
				{
					dictionary["recipient_missing"] = "True";
				}
				list.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("weapon", this.weaponDef, this.projectileDef));
				string text = GrammarResolver.Resolve("logentry", list, dictionary, "ranged fire");
				Rand.PopState();
				result = text;
			}
			return result;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
		}

		public override string ToString()
		{
			return "BattleLogEntry_RangedFire: " + this.InitiatorName + "->" + this.RecipientName;
		}
	}
}
