using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class WorkGiverDef : Def
	{
		public Type giverClass = null;

		public WorkTypeDef workType = null;

		public WorkTags workTags = WorkTags.None;

		public int priorityInType = 0;

		[MustTranslate]
		public string verb;

		[MustTranslate]
		public string gerund;

		public bool scanThings = true;

		public bool scanCells = false;

		public bool emergency = false;

		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		public bool directOrderable = true;

		public bool prioritizeSustains = false;

		public bool canBeDoneByNonColonists = false;

		public JobTag tagToGive = JobTag.MiscWork;

		public List<ThingDef> fixedBillGiverDefs = null;

		public bool billGiversAllHumanlikes = false;

		public bool billGiversAllHumanlikesCorpses = false;

		public bool billGiversAllMechanoids = false;

		public bool billGiversAllMechanoidsCorpses = false;

		public bool billGiversAllAnimals = false;

		public bool billGiversAllAnimalsCorpses = false;

		public bool tendToHumanlikesOnly = false;

		public bool tendToAnimalsOnly = false;

		public bool feedHumanlikesOnly = false;

		public bool feedAnimalsOnly = false;

		[Unsaved]
		private WorkGiver workerInt = null;

		public WorkGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (WorkGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
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
			if (this.verb.NullOrEmpty())
			{
				yield return base.defName + " lacks a verb.";
				/*Error: Unable to find new state assignment for yield return*/;
			}
			if (!this.gerund.NullOrEmpty())
				yield break;
			yield return base.defName + " lacks a gerund.";
			/*Error: Unable to find new state assignment for yield return*/;
			IL_014d:
			/*Error near IL_014e: Unexpected return in MoveNext()*/;
		}
	}
}
