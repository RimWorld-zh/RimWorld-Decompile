using System;

namespace Verse
{
	// Token: 0x02000DE1 RID: 3553
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06004FAA RID: 20394 RVA: 0x00296E31 File Offset: 0x00295231
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x00296E3C File Offset: 0x0029523C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Terrain(path=",
				this.path,
				", shader=",
				base.Shader,
				", color=",
				this.color,
				")"
			});
		}
	}
}
