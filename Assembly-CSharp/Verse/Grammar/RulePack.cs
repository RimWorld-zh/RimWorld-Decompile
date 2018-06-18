using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE8 RID: 3048
	public class RulePack
	{
		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x0600427D RID: 17021 RVA: 0x00230330 File Offset: 0x0022E730
		public List<Rule> Rules
		{
			get
			{
				if (this.rulesResolved == null)
				{
					this.rulesResolved = new List<Rule>();
					for (int i = 0; i < this.rulesStrings.Count; i++)
					{
						try
						{
							Rule_String item = new Rule_String(this.rulesStrings[i]);
							this.rulesResolved.Add(item);
						}
						catch (Exception ex)
						{
							Log.Error("Exception parsing grammar rule from " + this.rulesStrings[i] + ": " + ex.ToString(), false);
						}
					}
					if (this.rulesRaw != null)
					{
						for (int j = 0; j < this.rulesRaw.Count; j++)
						{
							this.rulesRaw[j].Init();
							this.rulesResolved.Add(this.rulesRaw[j]);
						}
					}
				}
				return this.rulesResolved;
			}
		}

		// Token: 0x04002D74 RID: 11636
		[MustTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesStrings = new List<string>();

		// Token: 0x04002D75 RID: 11637
		private List<Rule> rulesRaw = null;

		// Token: 0x04002D76 RID: 11638
		[Unsaved]
		private List<Rule> rulesResolved = null;
	}
}
