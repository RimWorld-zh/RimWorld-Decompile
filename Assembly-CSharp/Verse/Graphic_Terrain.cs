using System;

namespace Verse
{
	// Token: 0x02000DE5 RID: 3557
	public class Graphic_Terrain : Graphic_Single
	{
		// Token: 0x06004F97 RID: 20375 RVA: 0x00295875 File Offset: 0x00293C75
		public override void Init(GraphicRequest req)
		{
			base.Init(req);
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x00295880 File Offset: 0x00293C80
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
