using RimWorld;
using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	public class BattleLogEntry_RangedImpact : LogEntry, IDamageResultLog
	{
		private Pawn initiatorPawn;

		private ThingDef initiatorThing;

		private Pawn recipientPawn;

		private ThingDef recipientThing;

		private Pawn originalTargetPawn;

		private ThingDef originalTargetThing;

		private ThingDef weaponDef;

		private ThingDef projectileDef;

		private List<BodyPartDef> damagedParts;

		private List<bool> damagedPartsDestroyed;

		private string InitiatorName
		{
			get
			{
				return (this.initiatorPawn == null) ? ((this.initiatorThing == null) ? "null" : this.initiatorThing.defName) : this.initiatorPawn.NameStringShort;
			}
		}

		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? ((this.recipientThing == null) ? "null" : this.recipientThing.defName) : this.recipientPawn.NameStringShort;
			}
		}

		public BattleLogEntry_RangedImpact()
		{
		}

		public BattleLogEntry_RangedImpact(Thing initiator, Thing recipient, Thing originalTarget, ThingDef weaponDef, ThingDef projectileDef)
		{
			if (initiator is Pawn)
			{
				this.initiatorPawn = (initiator as Pawn);
			}
			else if (initiator != null)
			{
				this.initiatorThing = initiator.def;
			}
			if (recipient is Pawn)
			{
				this.recipientPawn = (recipient as Pawn);
			}
			else if (recipient != null)
			{
				this.recipientThing = recipient.def;
			}
			if (originalTarget is Pawn)
			{
				this.originalTargetPawn = (originalTarget as Pawn);
			}
			else if (originalTarget != null)
			{
				this.originalTargetThing = originalTarget.def;
			}
			this.weaponDef = weaponDef;
			this.projectileDef = projectileDef;
		}

		public void FillTargets(List<BodyPartDef> recipientParts, List<bool> recipientPartsDestroyed)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
		}

		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn || t == this.originalTargetPawn;
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
			Rand.PushState();
			Rand.Seed = base.randSeed;
			List<Rule> list = new List<Rule>();
			if (this.recipientPawn != null || this.recipientThing != null)
			{
				list.AddRange(RulePackDefOf.Combat_RangedDamage.Rules);
			}
			else
			{
				list.AddRange(RulePackDefOf.Combat_RangedMiss.Rules);
			}
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
			if (this.originalTargetPawn != this.recipientPawn || this.originalTargetThing != this.recipientThing)
			{
				if (this.originalTargetPawn != null)
				{
					list.AddRange(GrammarUtility.RulesForPawn("originalTarget", this.originalTargetPawn));
				}
				else if (this.originalTargetThing != null)
				{
					list.AddRange(GrammarUtility.RulesForDef("originalTarget", this.originalTargetThing));
				}
				else
				{
					dictionary["originalTarget_missing"] = "True";
				}
			}
			list.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("weapon", this.weaponDef, this.projectileDef));
			list.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.damagedParts, this.damagedPartsDestroyed, dictionary));
			string result = GrammarResolver.Resolve("logentry", list, dictionary, "ranged damage");
			Rand.PopState();
			return result;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.initiatorThing, "initiatorThing");
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.recipientThing, "recipientThing");
			Scribe_References.Look<Pawn>(ref this.originalTargetPawn, "originalTargetPawn", true);
			Scribe_Defs.Look<ThingDef>(ref this.originalTargetThing, "originalTargetThing");
			Scribe_Defs.Look<ThingDef>(ref this.weaponDef, "weaponDef");
			Scribe_Defs.Look<ThingDef>(ref this.projectileDef, "projectileDef");
			Scribe_Collections.Look<BodyPartDef>(ref this.damagedParts, "damagedParts", LookMode.Def, new object[0]);
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, new object[0]);
		}

		public override string ToString()
		{
			return "BattleLogEntry_RangedImpact: " + this.InitiatorName + "->" + this.RecipientName;
		}
	}
}
