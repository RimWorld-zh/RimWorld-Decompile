using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBC RID: 3004
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x04002C84 RID: 11396
		private Pawn initiatorPawn;

		// Token: 0x04002C85 RID: 11397
		private Pawn recipientPawn;

		// Token: 0x04002C86 RID: 11398
		private RulePackDef ruleDef;

		// Token: 0x06004125 RID: 16677 RVA: 0x0022670E File Offset: 0x00224B0E
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x00226718 File Offset: 0x00224B18
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06004127 RID: 16679 RVA: 0x00226738 File Offset: 0x00224B38
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00226770 File Offset: 0x00224B70
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x002267A0 File Offset: 0x00224BA0
		public override IEnumerable<Thing> GetConcerns()
		{
			if (this.initiatorPawn != null)
			{
				yield return this.initiatorPawn;
			}
			if (this.recipientPawn != null)
			{
				yield return this.recipientPawn;
			}
			yield break;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x002267CA File Offset: 0x00224BCA
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn);
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x002267E0 File Offset: 0x00224BE0
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x002267FC File Offset: 0x00224BFC
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x00226834 File Offset: 0x00224C34
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			if (this.recipientPawn == null)
			{
				Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709, false);
			}
			result.Includes.Add(this.ruleDef);
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", this.recipientPawn, result.Constants));
			return result;
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x002268A1 File Offset: 0x00224CA1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x002268DC File Offset: 0x00224CDC
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}
	}
}
