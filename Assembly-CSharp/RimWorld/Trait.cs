using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Trait : IExposable
	{
		public TraitDef def;

		private int degree;

		private bool scenForced;

		public int Degree
		{
			get
			{
				return this.degree;
			}
		}

		public TraitDegreeData CurrentData
		{
			get
			{
				return this.def.DataAtDegree(this.degree);
			}
		}

		public string Label
		{
			get
			{
				return this.CurrentData.label;
			}
		}

		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		public bool ScenForced
		{
			get
			{
				return this.scenForced;
			}
		}

		public Trait()
		{
		}

		public Trait(TraitDef def, int degree = 0, bool forced = false)
		{
			this.def = def;
			this.degree = degree;
			this.scenForced = forced;
		}

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
			Dictionary<SkillDef, int>.Enumerator enumerator = this.CurrentData.skillGains.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<SkillDef, int> current = enumerator.Current;
					if (current.Value != 0)
					{
						string value = "    " + current.Key.skillLabel + ":   " + current.Value.ToString("+##;-##");
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
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			if (this.GetPermaThoughts().Any())
			{
				stringBuilder.AppendLine();
				foreach (ThoughtDef permaThought in this.GetPermaThoughts())
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("    " + "PermanentMoodEffect".Translate() + " " + permaThought.stages[0].baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset));
				}
			}
			if (currentData.statOffsets != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				for (int i = 0; i < currentData.statOffsets.Count; i++)
				{
					StatModifier statModifier = currentData.statOffsets[i];
					string toStringAsOffset = statModifier.ToStringAsOffset;
					string value2 = "    " + statModifier.stat.LabelCap + " " + toStringAsOffset;
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

		public override string ToString()
		{
			return "Trait(" + this.def.ToString() + "-" + this.degree + ")";
		}

		private IEnumerable<ThoughtDef> GetPermaThoughts()
		{
			TraitDegreeData degree = this.CurrentData;
			List<ThoughtDef> allThoughts = DefDatabase<ThoughtDef>.AllDefsListForReading;
			for (int i = 0; i < allThoughts.Count; i++)
			{
				if (allThoughts[i].IsSituational && allThoughts[i].Worker is ThoughtWorker_AlwaysActive && allThoughts[i].requiredTraits != null && allThoughts[i].requiredTraits.Contains(this.def) && (!allThoughts[i].RequiresSpecificTraitsDegree || allThoughts[i].requiredTraitsDegree == degree.degree))
				{
					yield return allThoughts[i];
				}
			}
		}

		private bool AllowsWorkType(WorkTypeDef workDef)
		{
			return (this.def.disabledWorkTags & workDef.workTags) == WorkTags.None;
		}

		public IEnumerable<WorkTypeDef> GetDisabledWorkTypes()
		{
			for (int j = 0; j < this.def.disabledWorkTypes.Count; j++)
			{
				yield return this.def.disabledWorkTypes[j];
			}
			List<WorkTypeDef> workTypeDefList = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < workTypeDefList.Count; i++)
			{
				WorkTypeDef w = workTypeDefList[i];
				if (!this.AllowsWorkType(w))
				{
					yield return w;
				}
			}
		}
	}
}
