using System;

namespace RimWorld
{
	// Token: 0x02000676 RID: 1654
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x060022C3 RID: 8899 RVA: 0x0012B870 File Offset: 0x00129C70
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			base.Draw();
		}
	}
}
