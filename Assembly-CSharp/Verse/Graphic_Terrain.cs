using System;

namespace Verse
{
	// Token: 0x02000DE4 RID: 3556
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06004F95 RID: 20373 RVA: 0x00295855 File Offset: 0x00293C55
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x00295860 File Offset: 0x00293C60
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
