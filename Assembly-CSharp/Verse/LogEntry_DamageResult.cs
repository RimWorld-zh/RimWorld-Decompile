using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BCC RID: 3020
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x04002CF8 RID: 11512
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x04002CF9 RID: 11513
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x04002CFA RID: 11514
		protected bool deflected;

		// Token: 0x060041CC RID: 16844 RVA: 0x00226871 File Offset: 0x00224C71
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x060041CD RID: 16845 RVA: 0x0022687B File Offset: 0x00224C7B
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x060041CE RID: 16846 RVA: 0x0022689C File Offset: 0x00224C9C
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x060041CF RID: 16847 RVA: 0x002268B4 File Offset: 0x00224CB4
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x00226924 File Offset: 0x00224D24
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.BodyPart, new object[0]);
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, new object[0]);
			Scribe_Values.Look<bool>(ref this.deflected, "deflected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.damagedParts != null)
				{
					for (int i = this.damagedParts.Count - 1; i >= 0; i--)
					{
						if (this.damagedParts[i] == null)
						{
							this.damagedParts.RemoveAt(i);
							if (i < this.damagedPartsDestroyed.Count)
							{
								this.damagedPartsDestroyed.RemoveAt(i);
							}
						}
					}
				}
			}
		}
	}
}
