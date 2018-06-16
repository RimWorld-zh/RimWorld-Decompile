using System;

namespace RimWorld
{
	// Token: 0x02000678 RID: 1656
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x060022C5 RID: 8901 RVA: 0x0012B560 File Offset: 0x00129960
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			base.Draw();
		}
	}
}
