using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200053D RID: 1341
	public class Trait : IExposable
	{
		// Token: 0x060018EA RID: 6378 RVA: 0x000D891E File Offset: 0x000D6D1E
		public Trait()
		{
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x000D8927 File Offset: 0x000D6D27
		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x060018EC RID: 6380 RVA: 0x000D8948 File Offset: 0x000D6D48
		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x060018ED RID: 6381 RVA: 0x000D8964 File Offset: 0x000D6D64
		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x060018EE RID: 6382 RVA: 0x000D898C File Offset: 0x000D6D8C
		public string Label
		{
			get
			{
				return this.CurrentData.label;
			}
		}

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x060018EF RID: 6383 RVA: 0x000D89AC File Offset: 0x000D6DAC
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x060018F0 RID: 6384 RVA: 0x000D89CC File Offset: 0x000D6DCC
		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x000D89E8 File Offset: 0x000D6DE8
		public void ExposeData()
		{
			Scribe_Defs.Look<TraitDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.degree, "degree", 0, false);
			Scribe_Values.Look<bool>(ref this.scenForced, "scenForced", false, false);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.def == null)
			{
				this.def = DefDatabase<TraitDef>.GetRandom();
				this.degree = PawnGenerator.RandomTraitDegree(this.def);
			}
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x000D8A60 File Offset: 0x000D6E60
		public float OffsetOfStat(StatDef stat)
		{
			float num = 0f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statOffsets != null)
			{
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					if (currentData.statOffsets[i].stat == stat)
					{
						num += currentData.statOffsets[i].value;
					}
				}
			}
			return num;
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x000D8ADC File Offset: 0x000D6EDC
		public float MultiplierOfStat(StatDef stat)
		{
			float num = 1f;
			TraitDegreeData currentData = this.CurrentData;
			if (currentData.statFactors != null)
			{
				for (int i = 0; i < currentData.statFactors.Count; i++)
				{
					if (currentData.statFactors[i].stat == stat)
					{
						num *= currentData.statFactors[i].value;
					}
				}
			}
			return num;
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x000D8B58 File Offset: 0x000D6F58
		public string TipString(Pawn pawn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			TraitDegreeData currentData = this.CurrentData;
			stringBuilder.Append(currentData.description.AdjustedFor(pawn));
			int count = this.CurrentData.skillGains.Count;
			if (count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
			}
			int num = 0;
			foreach (KeyValuePair<SkillDef, int> keyValuePair in this.CurrentData.skillGains)
			{
				if (keyValuePair.Value != 0)
				{
					string value = "    " + keyValuePair.Key.skillLabel.CapitalizeFirst() + ":   " + keyValuePair.Value.ToString("+##;-##");
					if (num < count - 1)
					{
						stringBuilder.AppendLine(value);
					}
					else
					{
						stringBuilder.Append(value);
					}
					num++;
				}
			}
			if (this.GetPermaThoughts().Any<ThoughtDef>())
			{
				stringBuilder.AppendLine();
				foreach (ThoughtDef thoughtDef in this.GetPermaThoughts())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("    " + "PermanentMoodEffect".Translate() + " " + thoughtDef.stages[0].baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset));
				}
			}
			if (currentData.statOffsets != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					StatModifier statModifier = currentData.statOffsets[i];
					string valueToStringAsOffset = statModifier.ValueToStringAsOffset;
					string value2 = "    " + statModifier.stat.LabelCap + " " + valueToStringAsOffset;
					if (i < currentData.statOffsets.Count - 1)
					{
						stringBuilder.AppendLine(value2);
					}
					else
					{
						stringBuilder.Append(value2);
					}
				}
			}
			if (currentData.statFactors != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				for (int j = 0; j < currentData.statFactors.Count; j++)
				{
					StatModifier statModifier2 = currentData.statFactors[j];
					string toStringAsFactor = statModifier2.ToStringAsFactor;
					string value3 = "    " + statModifier2.stat.LabelCap + " " + toStringAsFactor;
					if (j < currentData.statFactors.Count - 1)
					{
						stringBuilder.AppendLine(value3);
					}
					else
					{
						stringBuilder.Append(value3);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x000D8E60 File Offset: 0x000D7260
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Trait(",
				this.def.ToString(),
				"-",
				this.degree,
				")"
			});
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x000D8EB4 File Offset: 0x000D72B4
		private IEnumerable<ThoughtDef> GetPermaThoughts()
		{
			TraitDegreeData degree = this.CurrentData;
			List<ThoughtDef> allThoughts = DefDatabase<ThoughtDef>.AllDefsListForReading;
			for (int i = 0; i < allThoughts.Count; i++)
			{
				if (allThoughts[i].IsSituational)
				{
					if (allThoughts[i].Worker is ThoughtWorker_AlwaysActive)
					{
						if (allThoughts[i].requiredTraits != null && allThoughts[i].requiredTraits.Contains(this.def))
						{
							if (!allThoughts[i].RequiresSpecificTraitsDegree || allThoughts[i].requiredTraitsDegree == degree.degree)
							{
								yield return allThoughts[i];
							}
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060018F7 RID: 6391 RVA: 0x000D8EE0 File Offset: 0x000D72E0
		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x000D8F0C File Offset: 0x000D730C
		public IEnumerable<WorkTypeDef> GetDisabledWorkTypes()
		{
			for (int i = 0; i < this.def.disabledWorkTypes.Count; i++)
			{
				yield return this.def.disabledWorkTypes[i];
			}
			List<WorkTypeDef> workTypeDefList = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int j = 0; j < workTypeDefList.Count; j++)
			{
				WorkTypeDef w = workTypeDefList[j];
				if (!this.AllowsWorkType(w))
				{
					yield return w;
				}
			}
			yield break;
		}

		// Token: 0x04000EAB RID: 3755
		public TraitDef def;

		// Token: 0x04000EAC RID: 3756
		private int degree;

		// Token: 0x04000EAD RID: 3757
		private bool scenForced;
	}
}
