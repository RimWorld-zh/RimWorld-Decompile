using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		public MainTabWindow_Assign()
		{
		}

		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
