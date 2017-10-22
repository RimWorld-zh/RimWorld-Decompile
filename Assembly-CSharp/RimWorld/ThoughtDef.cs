using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class ThoughtDef : Def
	{
		public Type thoughtClass = null;

		public Type workerClass = null;

		public List<ThoughtStage> stages = new List<ThoughtStage>();

		public int stackLimit = 1;

		public float stackedEffectMultiplier = 0.75f;

		public float durationDays = 0f;

		public bool invert = false;

		public bool validWhileDespawned = false;

		public ThoughtDef nextThought = null;

		public List<TraitDef> nullifyingTraits = null;

		public List<TaleDef> nullifyingOwnTales = null;

		public List<TraitDef> requiredTraits = null;

		public int requiredTraitsDegree = -2147483648;

		public StatDef effectMultiplyingStat = null;

		public HediffDef hediff;

		public GameConditionDef gameCondition;

		public bool nullifiedIfNotColonist;

		public ThoughtDef thoughtToMake = null;

		[NoTranslate]
		private string icon = (string)null;

		public bool showBubble = false;

		public int stackLimitPerPawn = -1;

		public float lerpOpinionToZeroAfterDurationPct = 0.7f;

		public bool socialThoughtAffectingMood = false;

		public float maxCumulatedOpinionOffset = 3.40282347E+38f;

		public TaleDef taleDef;

		[Unsaved]
		private ThoughtWorker workerInt = null;

		private Texture2D iconInt;

		public string Label
		{
			get
			{
				string result;
				if (!base.label.NullOrEmpty())
				{
					result = base.label;
				}
				else
				{
					if (this.stages.NullOrEmpty())
					{
						if (!this.stages[0].label.NullOrEmpty())
						{
							result = this.stages[0].label;
							goto IL_00b4;
						}
						if (!this.stages[0].labelSocial.NullOrEmpty())
						{
							result = this.stages[0].labelSocial;
							goto IL_00b4;
						}
					}
					Log.Error("Cannot get good label for ThoughtDef " + base.defName);
					result = base.defName;
				}
				goto IL_00b4;
				IL_00b4:
				return result;
			}
		}

		public int DurationTicks
		{
			get
			{
				return (int)(this.durationDays * 60000.0);
			}
		}

		public bool IsMemory
		{
			get
			{
				return this.durationDays > 0.0 || typeof(Thought_Memory).IsAssignableFrom(this.thoughtClass);
			}
		}

		public bool IsSituational
		{
			get
			{
				return this.Worker != null;
			}
		}

		public bool IsSocial
		{
			get
			{
				return typeof(ISocialThought).IsAssignableFrom(this.ThoughtClass);
			}
		}

		public bool RequiresSpecificTraitsDegree
		{
			get
			{
				return this.requiredTraitsDegree != -2147483648;
			}
		}

		public ThoughtWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (ThoughtWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public Type ThoughtClass
		{
			get
			{
				return (this.thoughtClass == null) ? ((!this.IsMemory) ? typeof(Thought_Situational) : typeof(Thought_Memory)) : this.thoughtClass;
			}
		}

		public Texture2D Icon
		{
			get
			{
				Texture2D result;
				if ((UnityEngine.Object)this.iconInt == (UnityEngine.Object)null)
				{
					if (this.icon == null)
					{
						result = null;
						goto IL_0044;
					}
					this.iconInt = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				result = this.iconInt;
				goto IL_0044;
				IL_0044:
				return result;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string error = enumerator.Current;
					yield return error;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.stages.NullOrEmpty())
			{
				yield return "no stages";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.workerClass != null && this.nextThought != null)
			{
				yield return "has a nextThought but also has a workerClass. nextThought only works for memories";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.IsMemory && this.workerClass != null)
			{
				yield return "has a workerClass but is a memory. workerClass only works for situational thoughts, not memories";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (this.IsMemory)
				yield break;
			if (this.workerClass != null)
				yield break;
			if (!this.IsSituational)
				yield break;
			yield return "is a situational thought but has no workerClass. Situational thoughts require workerClasses to analyze the situation";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_01ce:
			/*Error near IL_01cf: Unexpected return in MoveNext()*/;
		}

		public static ThoughtDef Named(string defName)
		{
			return DefDatabase<ThoughtDef>.GetNamed(defName, true);
		}
	}
}
