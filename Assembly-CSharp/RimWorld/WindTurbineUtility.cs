using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorld
{
	public static class WindTurbineUtility
	{
		[DebuggerHidden]
		public static IEnumerable<IntVec3> CalculateWindCells(IntVec3 center, Rot4 rot, IntVec2 size)
		{
			WindTurbineUtility.<CalculateWindCells>c__IteratorB5 <CalculateWindCells>c__IteratorB = new WindTurbineUtility.<CalculateWindCells>c__IteratorB5();
			<CalculateWindCells>c__IteratorB.rot = rot;
			<CalculateWindCells>c__IteratorB.center = center;
			<CalculateWindCells>c__IteratorB.<$>rot = rot;
			<CalculateWindCells>c__IteratorB.<$>center = center;
			WindTurbineUtility.<CalculateWindCells>c__IteratorB5 expr_23 = <CalculateWindCells>c__IteratorB;
			expr_23.$PC = -2;
			return expr_23;
		}
	}
}
