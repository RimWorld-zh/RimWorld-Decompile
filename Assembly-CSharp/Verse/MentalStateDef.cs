using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	public class MentalStateDef : Def
	{
		public Type stateClass = typeof(MentalState);

		public Type workerClass = typeof(MentalStateWorker);

		public MentalStateCategory category = MentalStateCategory.Undefined;

		public bool prisonersCanDo = true;

		public bool unspawnedCanDo = false;

		public bool colonistsOnly = false;

		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		public bool blockNormalThoughts = false;

		public EffecterDef stateEffecter;

		public TaleDef tale;

		public bool allowBeatfire = false;

		public DrugCategory drugCategory = DrugCategory.Any;

		public bool ignoreDrugPolicy = false;

		public float recoveryMtbDays = 1f;

		public int minTicksBeforeRecovery = 500;

		public int maxTicksBeforeRecovery = 99999999;

		public bool recoverFromSleep = false;

		public ThoughtDef moodRecoveryThought;

		[MustTranslate]
		public string beginLetter;

		[MustTranslate]
		public string beginLetterLabel;

		public LetterDef beginLetterDef;

		public Color nameColor = Color.green;

		[MustTranslate]
		public string recoveryMessage;

		[MustTranslate]
		public string baseInspectLine;

		private MentalStateWorker workerInt = null;

		public MentalStateWorker Worker
		{
			get
			{
				if (this.workerInt == null && this.workerClass != null)
				{
					this.workerInt = (MentalStateWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public bool IsAggro
		{
			get
			{
				return this.category == MentalStateCategory.Aggro;
			}
		}

		public bool IsExtreme
		{
			get
			{
				List<MentalBreakDef> allDefsListForReading = DefDatabase<MentalBreakDef>.AllDefsListForReading;
				int num = 0;
				bool result;
				while (true)
				{
					if (num < allDefsListForReading.Count)
					{
						if (allDefsListForReading[num].intensity == MentalBreakIntensity.Extreme && allDefsListForReading[num].mentalState == this)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
			}
		}

		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.beginLetterDef == null)
			{
				this.beginLetterDef = LetterDefOf.NegativeEvent;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.beginLetter.NullOrEmpty())
				yield break;
			if (!this.beginLetterLabel.NullOrEmpty())
				yield break;
			yield return "no beginLetter or beginLetterLabel";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_010a:
			/*Error near IL_010b: Unexpected return in MoveNext()*/;
		}
	}
}
