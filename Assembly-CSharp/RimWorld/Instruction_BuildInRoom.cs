using System;
using Verse;

namespace RimWorld
{
	public class Instruction_BuildInRoom : Instruction_BuildAtRoom
	{
		public Instruction_BuildInRoom()
		{
		}

		protected override CellRect BuildableRect
		{
			get
			{
				return Find.TutorialState.roomRect.ContractedBy(1);
			}
		}
	}
}
