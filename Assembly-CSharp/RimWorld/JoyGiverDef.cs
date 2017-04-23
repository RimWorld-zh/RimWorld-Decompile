using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors()
		{
			JoyGiverDef.<ConfigErrors>c__Iterator8F <ConfigErrors>c__Iterator8F = new JoyGiverDef.<ConfigErrors>c__Iterator8F();
			<ConfigErrors>c__Iterator8F.<>f__this = this;
			JoyGiverDef.<ConfigErrors>c__Iterator8F expr_0E = <ConfigErrors>c__Iterator8F;
			expr_0E.$PC = -2;
			return expr_0E;
		}
	}
}
