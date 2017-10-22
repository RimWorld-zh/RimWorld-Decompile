using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class JoyGiverDef : Def
	{
		public Type giverClass = null;

		public float baseChance = 0f;

		public List<ThingDef> thingDefs = null;

		public JobDef jobDef;

		public bool desireSit = true;

		public float pctPawnsEverDo = 1f;

		public bool unroofedOnly = false;

		public JoyKindDef joyKind;

		public List<PawnCapacityDef> requiredCapacities = new List<PawnCapacityDef>();

		public bool canDoWhileInBed;

		private JoyGiver workerInt = null;

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
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0().GetEnumerator())
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
			IL_015d:
			/*Error near IL_015e: Unexpected return in MoveNext()*/;
		}
	}
}
