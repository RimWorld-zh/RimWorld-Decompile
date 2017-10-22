using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class StatDef : Def
	{
		public StatCategoryDef category = null;

		public Type workerClass = typeof(StatWorker);

		public float hideAtValue = -2.14748365E+09f;

		public bool showNonAbstract = true;

		public bool showIfUndefined = true;

		public bool showOnPawns = true;

		public bool showOnHumanlikes = true;

		public bool showOnAnimals = true;

		public bool showOnMechanoids = true;

		public bool showOnNonWorkTables = true;

		public bool neverDisabled = false;

		public int displayPriorityInCategory = 0;

		public ToStringNumberSense toStringNumberSense = ToStringNumberSense.Absolute;

		public ToStringStyle toStringStyle = ToStringStyle.Integer;

		public ToStringStyle? toStringStyleUnfinalized;

		public string formatString = (string)null;

		public float defaultBaseValue = 1f;

		public List<SkillNeed> skillNeedOffsets = null;

		public float noSkillOffset = 0f;

		public List<PawnCapacityOffset> capacityOffsets = null;

		public List<StatDef> statFactors = null;

		public bool applyFactorsIfNegative = true;

		public List<SkillNeed> skillNeedFactors = null;

		public float noSkillFactor = 1f;

		public List<PawnCapacityFactor> capacityFactors = null;

		public SimpleCurve postProcessCurve = null;

		public float minValue = -9999999f;

		public float maxValue = 9999999f;

		public bool roundValue = false;

		public float roundToFiveOver = 3.40282347E+38f;

		public bool minifiedThingInherits;

		public bool scenarioRandomizable = false;

		public List<StatPart> parts = null;

		private StatWorker workerInt = null;

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

		public ToStringStyle ToStringStyleUnfinalized
		{
			get
			{
				ToStringStyle? nullable = this.toStringStyleUnfinalized;
				return (!nullable.HasValue) ? this.toStringStyle : this.toStringStyleUnfinalized.Value;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string err2 = enumerator.Current;
					yield return err2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.capacityFactors != null)
			{
				foreach (PawnCapacityFactor capacityFactor in this.capacityFactors)
				{
					if (capacityFactor.weight > 1.0)
					{
						yield return base.defName + " has activity factor with weight > 1";
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
			}
			if (this.parts != null)
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					using (IEnumerator<string> enumerator3 = this.parts[i].ConfigErrors().GetEnumerator())
					{
						if (enumerator3.MoveNext())
						{
							string err = enumerator3.Current;
							yield return base.defName + " has error in StatPart " + this.parts[i].ToString() + ": " + err;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
			}
			yield break;
			IL_02b9:
			/*Error near IL_02ba: Unexpected return in MoveNext()*/;
		}

		public string ValueToString(float val, ToStringNumberSense numberSense = ToStringNumberSense.Absolute)
		{
			return this.Worker.ValueToString(val, true, numberSense);
		}

		public static StatDef Named(string defName)
		{
			return DefDatabase<StatDef>.GetNamed(defName, true);
		}
	}
}
