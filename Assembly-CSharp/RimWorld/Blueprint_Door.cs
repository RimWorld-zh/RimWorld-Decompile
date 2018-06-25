using System;

namespace RimWorld
{
	// Token: 0x02000676 RID: 1654
	public class Blueprint_Door : Blueprint_Build
	{
		// Token: 0x060022C2 RID: 8898 RVA: 0x0012BAD8 File Offset: 0x00129ED8
		public override void Draw()
		{
			base.Rotation = Building_Door.DoorRotationAt(base.Position, base.Map);
			base.Draw();
		}
	}
}
