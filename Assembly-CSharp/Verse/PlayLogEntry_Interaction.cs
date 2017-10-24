using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	public class PlayLogEntry_Interaction : LogEntry
	{
		private InteractionDef intDef;

		private Pawn initiator;

		private Pawn recipient;

		private List<RulePackDef> extraSentencePacks = null;

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

		public PlayLogEntry_Interaction()
		{
		}

		public PlayLogEntry_Interaction(InteractionDef intDef, Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipient = recipient;
			this.extraSentencePacks = extraSentencePacks;
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
			return this.intDef.Symbol;
		}

		public override string ToGameStringFromPOV(Thing pov)
		{
			string result;
			if (this.initiator == null || this.recipient == null)
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422);
				result = "[" + this.intDef.label + " error: null pawn reference]";
			}
			else
			{
				Rand.PushState();
				Rand.Seed = base.randSeed;
				GrammarRequest request = default(GrammarRequest);
				string text;
				if (pov == this.initiator)
				{
					request.Rules.AddRange(this.intDef.logRulesInitiator.Rules);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("me", this.initiator.Name, this.initiator.kindDef, this.initiator.gender, this.initiator.Faction));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("other", this.recipient.Name, this.recipient.kindDef, this.recipient.gender, this.recipient.Faction));
					text = GrammarResolver.Resolve("logentry", request, "interaction from initiator");
				}
				else if (pov == this.recipient)
				{
					if (this.intDef.logRulesRecipient != null)
					{
						request.Rules.AddRange(this.intDef.logRulesRecipient.Rules);
					}
					else
					{
						request.Rules.AddRange(this.intDef.logRulesInitiator.Rules);
					}
					request.Rules.AddRange(GrammarUtility.RulesForPawn("me", this.recipient.Name, this.recipient.kindDef, this.recipient.gender, this.recipient.Faction));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("other", this.initiator.Name, this.initiator.kindDef, this.initiator.gender, this.initiator.Faction));
					text = GrammarResolver.Resolve("logentry", request, "interaction from recipient");
				}
				else
				{
					Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251);
					text = this.ToString();
				}
				if (this.extraSentencePacks != null)
				{
					for (int i = 0; i < this.extraSentencePacks.Count; i++)
					{
						request.Clear();
						request.Includes.Add(this.extraSentencePacks[i]);
						request.Rules.AddRange(GrammarUtility.RulesForPawn("initiator", this.initiator.Name, this.initiator.kindDef, this.initiator.gender, this.initiator.Faction));
						request.Rules.AddRange(GrammarUtility.RulesForPawn("recipient", this.recipient.Name, this.recipient.kindDef, this.recipient.gender, this.recipient.Faction));
						text = text + " " + GrammarResolver.Resolve(this.extraSentencePacks[i].RulesPlusIncludes[0].keyword, request, "extraSentencePack");
					}
				}
				Rand.PopState();
				result = text;
			}
			return result;
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<InteractionDef>(ref this.intDef, "intDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipient, "recipient", true);
			Scribe_Collections.Look<RulePackDef>(ref this.extraSentencePacks, "extras", LookMode.Undefined, new object[0]);
		}

		public override string ToString()
		{
			return this.intDef.label + ": " + this.InitiatorName + "->" + this.RecipientName;
		}
	}
}
