using System;
using Verse;

namespace RimWorld
{
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		public Instruction_ExpandAreaHome()
		{
		}

		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
