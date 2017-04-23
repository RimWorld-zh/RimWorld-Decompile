using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompProperties_Rottable : CompProperties
	{
		public float daysToRotStart = 2f;

		public bool rotDestroys;

		public float rotDamagePerDay = 40f;

		public float daysToDessicated = 999f;

		public float dessicatedDamagePerDay;

		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt(this.daysToRotStart * 60000f);
			}
		}

		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt(this.daysToDessicated * 60000f);
			}
		}

		public CompProperties_Rottable()
		{
			this.compClass = typeof(CompRottable);
		}

		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		[DebuggerHidden]
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Rottable.<ConfigErrors>c__Iterator7C <ConfigErrors>c__Iterator7C = new CompProperties_Rottable.<ConfigErrors>c__Iterator7C();
			<ConfigErrors>c__Iterator7C.parentDef = parentDef;
			<ConfigErrors>c__Iterator7C.<$>parentDef = parentDef;
			<ConfigErrors>c__Iterator7C.<>f__this = this;
			CompProperties_Rottable.<ConfigErrors>c__Iterator7C expr_1C = <ConfigErrors>c__Iterator7C;
			expr_1C.$PC = -2;
			return expr_1C;
		}
	}
}
