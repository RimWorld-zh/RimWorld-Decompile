using System;

namespace RimWorld
{
	// Token: 0x02000678 RID: 1656
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x060022C7 RID: 8903 RVA: 0x0012B5D8 File Offset: 0x001299D8
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			base.Draw();
		}
	}
}
