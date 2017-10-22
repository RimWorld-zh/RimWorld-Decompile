using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StatDef : Def
	{
		public StatCategoryDef category;

		public Type workerClass = typeof(StatWorker);

		public float hideAtValue = -2.14748365E+09f;

		public bool showNonAbstract = true;

		public bool showIfUndefined = true;

		public bool showOnPawns = true;

		public bool showOnHumanlikes = true;

		public bool showOnAnimals = true;

		public bool showOnMechanoids = true;

		public int displayPriorityInCategory;

		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		public ToStringStyle toStringStyle;

		public string formatString;

		public float defaultBaseValue = 1f;

		public float minValue;

		public float maxValue = 9999999f;

		public bool roundValue;

		public float roundToFiveOver = 3.40282347E+38f;

		public List<StatDef> statFactors;

		public bool applyFactorsIfNegative = true;

		public float noSkillFactor = 1f;

		public List<SkillNeed> skillNeedFactors;

		public List<PawnCapacityFactor> capacityFactors;

		public SimpleCurve postProcessCurve;

		public List<StatPart> parts;

		private StatWorker workerInt;

		public StatWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					if (this.parts != null)
					{
						for (int i = 0; i < this.parts.Count; i++)
						{
							this.parts[i].parentStat = this;
						}
					}
					this.workerInt = (StatWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.InitSetStat(this);
				}
				return this.workerInt;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string item in base.ConfigErrors())
			{
				yield return item;
			}
			if (this.capacityFactors != null)
			{
				List<PawnCapacityFactor>.Enumerator enumerator2 = this.capacityFactors.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						PawnCapacityFactor afac = enumerator2.Current;
						if (afac.weight > 1.0)
						{
							yield return base.defName + " has activity factor with weight > 1";
						}
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
			}
			if (this.parts != null)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					foreach (string item2 in this.parts[i].ConfigErrors())
					{
						yield return base.defName + " has error in StatPart " + this.parts[i].ToString() + ": " + item2;
					}
				}
			}
		}

		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			string text = val.ToStringByStyle(this.toStringStyle, numberSense);
			if (!this.formatString.NullOrEmpty())
			{
				text = string.Format(this.formatString, text);
			}
			return text;
		}

		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}
	}
}
