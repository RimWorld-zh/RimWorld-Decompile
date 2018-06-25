using System;

namespace Verse
{
	// Token: 0x02000DE4 RID: 3556
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06004FAE RID: 20398 RVA: 0x0029723D File Offset: 0x0029563D
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x00297248 File Offset: 0x00295648
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
