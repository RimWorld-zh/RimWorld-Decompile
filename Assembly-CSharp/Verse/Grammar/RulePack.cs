using System;
using System.Collections.Generic;

namespace Verse.Grammar
{
	// Token: 0x02000BE7 RID: 3047
	public class RulePack
	{
		// Token: 0x04002D80 RID: 11648
		[MustTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesStrings = new List<string>();

		// Token: 0x04002D81 RID: 11649
		[MayTranslate]
		[TranslationCanChangeCount]
		private List<string> rulesFiles = new List<string>();

		// Token: 0x04002D82 RID: 11650
		private List<Rule> rulesRaw = null;

		// Token: 0x04002D83 RID: 11651
		[Unsaved]
		private List<Rule> rulesResolved = null;

		// Token: 0x17000A75 RID: 2677
		// (get) Token: 0x06004282 RID: 17026 RVA: 0x00230E64 File Offset: 0x0022F264
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
							Rule_String rule_String = new Rule_String(this.rulesStrings[i]);
							rule_String.Init();
							this.rulesResolved.Add(rule_String);
						}
						catch (Exception ex)
						{
							Log.Error(string.Concat(new object[]
							{
								"Exception parsing grammar rule from ",
								this.rulesStrings[i],
								": ",
								ex
							}), false);
						}
					}
					for (int j = 0; j < this.rulesFiles.Count; j++)
					{
						try
						{
							string[] array = this.rulesFiles[j].Split(new string[]
							{
								"->"
							}, StringSplitOptions.None);
							Rule_File rule_File = new Rule_File();
							rule_File.keyword = array[0].Trim();
							rule_File.path = array[1].Trim();
							rule_File.Init();
							this.rulesResolved.Add(rule_File);
						}
						catch (Exception ex2)
						{
							Log.Error(string.Concat(new object[]
							{
								"Error initializing Rule_File ",
								this.rulesFiles[j],
								": ",
								ex2
							}), false);
						}
					}
					if (this.rulesRaw != null)
					{
						for (int k = 0; k < this.rulesRaw.Count; k++)
						{
							try
							{
								this.rulesRaw[k].Init();
								this.rulesResolved.Add(this.rulesRaw[k]);
							}
							catch (Exception ex3)
							{
								Log.Error(string.Concat(new object[]
								{
									"Error initializing rule ",
									this.rulesRaw[k].ToStringSafe<Rule>(),
									": ",
									ex3
								}), false);
							}
						}
					}
				}
				return this.rulesResolved;
			}
		}
	}
}
