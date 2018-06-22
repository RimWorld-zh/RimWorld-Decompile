using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BFE RID: 3070
	public interface ICellBoolGiver
	{
		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06004332 RID: 17202
		Color Color { get; }

		// Token: 0x06004333 RID: 17203
		bool GetCellBool(int index);

		// Token: 0x06004334 RID: 17204
		Color GetCellExtraColor(int index);
	}
}
