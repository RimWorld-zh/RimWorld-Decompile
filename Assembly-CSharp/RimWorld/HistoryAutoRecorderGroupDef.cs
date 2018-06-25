using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class HistoryAutoRecorderGroupDef : Def
	{
		public bool useFixedScale = false;

		public Vector2 fixedScale = default(Vector2);

		public bool integersOnly = false;

		public List<HistoryAutoRecorderDef> historyAutoRecorderDefs = new List<HistoryAutoRecorderDef>();

		public HistoryAutoRecorderGroupDef()
		{
		}

		public static HistoryAutoRecorderGroupDef Named(string defName)
		{
			return DefDatabase<HistoryAutoRecorderGroupDef>.GetNamed(defName, true);
		}
	}
}
