using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompProperties_Rottable : CompProperties
	{
		public float daysToRotStart = 2f;

		public bool rotDestroys = false;

		public float rotDamagePerDay = 40f;

		public float daysToDessicated = 999f;

		public float dessicatedDamagePerDay = 0f;

		public bool disableIfHatcher;

		public int TicksToRotStart
		{
			get
			{
				return Mathf.RoundToInt((float)(this.daysToRotStart * 60000.0));
			}
		}

		public int TicksToDessicated
		{
			get
			{
				return Mathf.RoundToInt((float)(this.daysToDessicated * 60000.0));
			}
		}

		public CompProperties_Rottable()
		{
			base.compClass = typeof(CompRottable);
		}

		public CompProperties_Rottable(float daysToRotStart)
		{
			this.daysToRotStart = daysToRotStart;
		}

		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			using (IEnumerator<string> enumerator = this._003CConfigErrors_003E__BaseCallProxy0(parentDef).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					string e = enumerator.Current;
					yield return e;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (parentDef.tickerType == TickerType.Normal)
				yield break;
			if (parentDef.tickerType == TickerType.Rare)
				yield break;
			yield return "CompRottable needs tickerType " + TickerType.Rare + " or " + TickerType.Normal + ", has " + parentDef.tickerType;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_014b:
			/*Error near IL_014c: Unexpected return in MoveNext()*/;
		}
	}
}
