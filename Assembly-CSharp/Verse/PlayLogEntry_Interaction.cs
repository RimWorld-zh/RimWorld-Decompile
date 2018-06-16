using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BD1 RID: 3025
	[HasDebugOutput]
	public class PlayLogEntry_Interaction : LogEntry
	{
		// Token: 0x060041D7 RID: 16855 RVA: 0x0022AD88 File Offset: 0x00229188
		public PlayLogEntry_Interaction() : base(null)
		{
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x0022AD99 File Offset: 0x00229199
		public PlayLogEntry_Interaction(InteractionDef intDef, Pawn initiator, Pawn recipient, List<RulePackDef> extraSentencePacks) : base(null)
		{
			this.intDef = intDef;
			this.initiator = initiator;
			this.recipient = recipient;
			this.extraSentencePacks = extraSentencePacks;
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x060041D9 RID: 16857 RVA: 0x0022ADC8 File Offset: 0x002291C8
		private string InitiatorName
		{
			get
			{
				return (this.initiator == null) ? "null" : this.initiator.LabelShort;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x060041DA RID: 16858 RVA: 0x0022AE00 File Offset: 0x00229200
		private string RecipientName
		{
			get
			{
				return (this.recipient == null) ? "null" : this.recipient.LabelShort;
			}
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x0022AE38 File Offset: 0x00229238
		public override bool Concerns(Thing t)
		{
			return t == this.initiator || t == this.recipient;
		}

		// Token: 0x060041DC RID: 16860 RVA: 0x0022AE68 File Offset: 0x00229268
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiator != null)
			{
				yield return this.initiator;
			}
			if (this.recipient != null)
			{
				yield return this.recipient;
			}
			yield break;
		}

		// Token: 0x060041DD RID: 16861 RVA: 0x0022AE94 File Offset: 0x00229294
		public override void ClickedFromPOV(Thing pov)
		{
			if (pov == this.initiator)
			{
				CameraJumper.TryJumpAndSelect(this.recipient);
			}
			else
			{
				if (pov != this.recipient)
				{
					throw new NotImplementedException();
				}
				CameraJumper.TryJumpAndSelect(this.initiator);
			}
		}

		// Token: 0x060041DE RID: 16862 RVA: 0x0022AEEC File Offset: 0x002292EC
		public override Texture2D IconFromPOV(Thing pov)
		{
			return this.intDef.Symbol;
		}

		// Token: 0x060041DF RID: 16863 RVA: 0x0022AF0C File Offset: 0x0022930C
		public override string GetTipString()
		{
			return this.intDef.LabelCap + "\n" + base.GetTipString();
		}

		// Token: 0x060041E0 RID: 16864 RVA: 0x0022AF3C File Offset: 0x0022933C
		protected override string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			string result;
			if (this.initiator == null || this.recipient == null)
			{
				Log.ErrorOnce("PlayLogEntry_Interaction has a null pawn reference.", 34422, false);
				result = "[" + this.intDef.label + " error: null pawn reference]";
			}
			else
			{
				Rand.PushState();
				Rand.Seed = this.logID;
				GrammarRequest request = base.GenerateGrammarRequest();
				string text;
				if (pov == this.initiator)
				{
					request.IncludesBare.Add(this.intDef.logRulesInitiator);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("ME", this.initiator, request.Constants));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("OTHER", this.recipient, request.Constants));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants));
					text = GrammarResolver.Resolve("r_logentry", request, "interaction from initiator", forceLog);
				}
				else if (pov == this.recipient)
				{
					if (this.intDef.logRulesRecipient != null)
					{
						request.IncludesBare.Add(this.intDef.logRulesRecipient);
					}
					else
					{
						request.IncludesBare.Add(this.intDef.logRulesInitiator);
					}
					request.Rules.AddRange(GrammarUtility.RulesForPawn("ME", this.recipient, request.Constants));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("OTHER", this.initiator, request.Constants));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants));
					text = GrammarResolver.Resolve("r_logentry", request, "interaction from recipient", forceLog);
				}
				else
				{
					Log.ErrorOnce("Cannot display PlayLogEntry_Interaction from POV who isn't initiator or recipient.", 51251, false);
					text = this.ToString();
				}
				if (this.extraSentencePacks != null)
				{
					for (int i = 0; i < this.extraSentencePacks.Count; i++)
					{
						request.Clear();
						request.Includes.Add(this.extraSentencePacks[i]);
						request.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", this.initiator, request.Constants));
						request.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipient, request.Constants));
						text = text + " " + GrammarResolver.Resolve(this.extraSentencePacks[i].RulesPlusIncludes[0].keyword, request, "extraSentencePack", forceLog);
					}
				}
				Rand.PopState();
				result = text;
			}
			return result;
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x0022B250 File Offset: 0x00229650
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<InteractionDef>(ref this.intDef, "intDef");
			Scribe_References.Look<Pawn>(ref this.initiator, "initiator", true);
			Scribe_References.Look<Pawn>(ref this.recipient, "recipient", true);
			Scribe_Collections.Look<RulePackDef>(ref this.extraSentencePacks, "extras", LookMode.Undefined, new object[0]);
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x0022B2B0 File Offset: 0x002296B0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				this.intDef.label,
				": ",
				this.InitiatorName,
				"->",
				this.RecipientName
			});
		}

		// Token: 0x04002CF2 RID: 11506
		private InteractionDef intDef;

		// Token: 0x04002CF3 RID: 11507
		private Pawn initiator;

		// Token: 0x04002CF4 RID: 11508
		private Pawn recipient;

		// Token: 0x04002CF5 RID: 11509
		private List<RulePackDef> extraSentencePacks = null;
	}
}
