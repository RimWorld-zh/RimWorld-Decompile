using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBA RID: 3002
	public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
	{
		// Token: 0x04002C84 RID: 11396
		private Pawn initiatorPawn;

		// Token: 0x04002C85 RID: 11397
		private Pawn recipientPawn;

		// Token: 0x04002C86 RID: 11398
		private RulePackDef ruleDef;

		// Token: 0x06004122 RID: 16674 RVA: 0x00226632 File Offset: 0x00224A32
		public BattleLogEntry_DamageTaken() : base(null)
		{
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x0022663C File Offset: 0x00224A3C
		public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null) : base(null)
		{
			this.initiatorPawn = initiator;
			this.recipientPawn = recipient;
			this.ruleDef = ruleDef;
		}

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06004124 RID: 16676 RVA: 0x0022665C File Offset: 0x00224A5C
		private string RecipientName
		{
			get
			{
				return (this.recipientPawn == null) ? "null" : this.recipientPawn.LabelShort;
			}
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x00226694 File Offset: 0x00224A94
		public override bool Concerns(Thing t)
		{
			return t == this.initiatorPawn || t == this.recipientPawn;
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x002266C4 File Offset: 0x00224AC4
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

		// Token: 0x06004127 RID: 16679 RVA: 0x002266EE File Offset: 0x00224AEE
		public override void ClickedFromPOV(Thing pov)
		{
			CameraJumper.TryJumpAndSelect(this.recipientPawn);
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00226704 File Offset: 0x00224B04
		public override Texture2D IconFromPOV(Thing pov)
		{
			return LogEntry.Blood;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x00226720 File Offset: 0x00224B20
		protected override BodyDef DamagedBody()
		{
			return (this.recipientPawn == null) ? null : this.recipientPawn.RaceProps.body;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x00226758 File Offset: 0x00224B58
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

		// Token: 0x0600412B RID: 16683 RVA: 0x002267C5 File Offset: 0x00224BC5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.initiatorPawn, "initiatorPawn", true);
			Scribe_References.Look<Pawn>(ref this.recipientPawn, "recipientPawn", true);
			Scribe_Defs.Look<RulePackDef>(ref this.ruleDef, "ruleDef");
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x00226800 File Offset: 0x00224C00
		public override string ToString()
		{
			return "BattleLogEntry_DamageTaken: " + this.RecipientName;
		}
	}
}
