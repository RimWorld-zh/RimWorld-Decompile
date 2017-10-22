using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
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
