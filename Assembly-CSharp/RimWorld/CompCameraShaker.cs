using Verse;

namespace RimWorld
{
	public class CompCameraShaker : ThingComp
	{
		public CompProperties_CameraShaker Props
		{
			get
			{
				return (CompProperties_CameraShaker)base.props;
			}
		}

		public override void CompTick()
		{
			base.CompTick();
			if (base.parent.Spawned && base.parent.Map == Find.VisibleMap)
			{
				Find.CameraDriver.shaker.SetMinShake(this.Props.mag);
			}
		}
	}
}
