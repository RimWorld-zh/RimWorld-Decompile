using System.Collections.Generic;
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
			foreach (string item in base.ConfigErrors(parentDef))
			{
				yield return item;
			}
			if (parentDef.tickerType != TickerType.Rare)
			{
				yield return "CompRottable needs tickerType " + TickerType.Rare + ", has " + parentDef.tickerType;
			}
		}
	}
}
