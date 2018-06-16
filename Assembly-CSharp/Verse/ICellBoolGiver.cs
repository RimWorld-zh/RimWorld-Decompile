using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C02 RID: 3074
	public interface ICellBoolGiver
	{
		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x0600432B RID: 17195
		Color Color { get; }

		// Token: 0x0600432C RID: 17196
		bool GetCellBool(int index);

		// Token: 0x0600432D RID: 17197
		Color GetCellExtraColor(int index);
	}
}
