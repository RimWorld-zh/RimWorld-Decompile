using System;

namespace RimWorld
{
	// Token: 0x02000674 RID: 1652
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x060022BF RID: 8895 RVA: 0x0012B720 File Offset: 0x00129B20
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			base.Draw();
		}
	}
}
