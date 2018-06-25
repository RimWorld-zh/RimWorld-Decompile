using System;
using Verse.AI.Group;

namespace RimWorld
{
	public class LordJob_ManTurrets : LordJob
	{
		public LordJob_ManTurrets()
		{
		}

		public override StateGraph CreateGraph()
		{
			return new StateGraph
			{
				StartingToil = new LordToil_ManClosestTurrets()
			};
		}
	}
}
