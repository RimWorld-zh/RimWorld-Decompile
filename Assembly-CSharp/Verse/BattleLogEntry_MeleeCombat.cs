using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	public class BattleLogEntry_MeleeCombat : LogEntry, IDamageResultLog
	{
		private RulePackDef outcomeRuleDef;

		private RulePackDef maneuverRuleDef;

		private Pawn initiator;

		private Pawn recipient;

		private ImplementOwnerTypeDef implementType;

		private ThingDef ownerEquipmentDef;

		private HediffDef ownerHediffDef;

		private string toolLabel;

		private List<BodyPartDef> damagedParts;

		private List<bool> damagedPartsDestroyed;

		private string InitiatorName
		{
			get
			{
				return (this.initiator == null) ? "null" : this.initiator.NameStringShort;
			}
		}

		private string RecipientName
		{
			get
			{
				return (this.recipient == null) ? "null" : this.recipient.NameStringShort;
			}
		}

		public BattleLogEntry_MeleeCombat()
		{
		}

		public BattleLogEntry_MeleeCombat(RulePackDef outcomeRuleDef, RulePackDef maneuverRuleDef, Pawn initiator, Pawn recipient, ImplementOwnerTypeDef implementType, string toolLabel, ThingDef ownerEquipmentDef = null, HediffDef ownerHediffDef = null)
		{
			this.outcomeRuleDef = outcomeRuleDef;
			this.maneuverRuleDef = maneuverRuleDef;
			this.initiator = initiator;
			this.recipient = recipient;
			this.implementType = implementType;
			this.ownerEquipmentDef = ownerEquipmentDef;
			this.ownerHediffDef = ownerHediffDef;
			this.toolLabel = toolLabel;
			if (ownerEquipmentDef != null && ownerHediffDef != null)
			{
				Log.ErrorOnce(string.Format("Combat log owned by both equipment {0} and hediff {1}, may produce unexpected results", ownerEquipmentDef.label, ownerHediffDef.label), 96474669);
			}
		}

		public void FillTargets(List<BodyPartDef> damagedParts, List<bool> damagedPartsDestroyed)
		{
			this.damagedParts = damagedParts;
			this.damagedPartsDestroyed = damagedPartsDestroyed;
		}

		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipient;
		}

		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect((Thing)this.recipient);
				return;
			}
			if (pov == this.recipient)
			{
				CameraJumper.TryJumpAndSelect((Thing)this.initiator);
				return;
			}
			throw new NotImplementedException();
		}

		public override Texture2D IconFromPOV(Thing pov)
		{
			return (!this.damagedParts.NullOrEmpty()) ? ((pov == null || pov == this.recipient) ? LogEntry.Blood : ((pov != this.initiator) ? null : LogEntry.BloodTarget)) : null;
		}

		public override string ToGameStringFromPOV(Thing pov)
		{
			string result;
			if (this.initiator == null || this.recipient == null)
			{
				Log.ErrorOnce("BattleLogEntry_MeleeCombat has a null pawn reference.", 34422);
				result = "[" + this.outcomeRuleDef.label + " error: null pawn reference]";
			}
			else
			{
				Rand.PushState();
				Rand.Seed = base.randSeed;
				GrammarRequest request = default(GrammarRequest);
				request.Rules.AddRange(GrammarUtility.RulesForPawn("initiator", this.initiator));
				request.Rules.AddRange(GrammarUtility.RulesForPawn("recipient", this.recipient));
				request.Includes.Add(this.outcomeRuleDef);
				request.Includes.Add(this.maneuverRuleDef);
				if (!this.toolLabel.NullOrEmpty())
				{
					request.Rules.Add(new Rule_String("tool_label", this.toolLabel));
				}
				if (this.implementType != null && !this.implementType.implementOwnerRuleName.NullOrEmpty())
				{
					if (this.ownerEquipmentDef != null)
					{
						request.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerEquipmentDef));
					}
					else if (this.ownerHediffDef != null)
					{
						request.Rules.AddRange(GrammarUtility.RulesForDef(this.implementType.implementOwnerRuleName, this.ownerHediffDef));
					}
				}
				if (this.implementType != null && !this.implementType.implementOwnerTypeValue.NullOrEmpty())
				{
					request.Constants["implementOwnerType"] = this.implementType.implementOwnerTypeValue;
				}
				request.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.damagedParts, this.damagedPartsDestroyed, request.Constants));
				string text = GrammarResolver.Resolve("logentry", request, "combat interaction");
				Rand.PopState();
				result = text;
			}
			return result;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RulePackDef>(ref this.outcomeRuleDef, "outcomeRuleDef");
			Scribe_Defs.Look<RulePackDef>(ref this.maneuverRuleDef, "maneuverRuleDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipient, "recipient", true);
			Scribe_Defs.Look<ImplementOwnerTypeDef>(ref this.implementType, "implementType");
			Scribe_Defs.Look<ThingDef>(ref this.ownerEquipmentDef, "ownerDef");
			Scribe_Collections.Look<BodyPartDef>(ref this.damagedParts, "damagedParts", LookMode.Def, new object[0]);
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, new object[0]);
		}

		public override string ToString()
		{
			return this.outcomeRuleDef.defName + ": " + this.InitiatorName + "->" + this.RecipientName;
		}
	}
}
