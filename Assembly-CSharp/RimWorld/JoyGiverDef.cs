using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class JoyGiverDef : Def
	{
		public Type giverClass;

		public float baseChance;

		public List<ThingDef> thingDefs;

		public JobDef jobDef;

		public bool desireSit = true;

		public float pctPawnsEverDo = 1f;

		public bool unroofedOnly;

		public JoyKindDef joyKind;

		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		public bool canDoWhileInBed;

		private JoyGiver workerInt;

		public JoyGiver Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (JoyGiver)Activator.CreateInstance(this.giverClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		public override IEnumerable<string> ConfigErrors()
		{
			using (IEnumerator<string> enumerator = base.ConfigErrors().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (this.jobDef == null)
				yield break;
			if (this.jobDef.joyKind == this.joyKind)
				yield break;
			yield return "jobDef " + this.jobDef + " has joyKind " + this.jobDef.joyKind + " which does not match our joyKind " + this.joyKind;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_0159:
			/*Error near IL_015a: Unexpected return in MoveNext()*/;
		}
	}
}
