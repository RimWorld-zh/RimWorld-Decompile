using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C01 RID: 3073
	public interface ICellBoolGiver
	{
		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06004329 RID: 17193
		Color Color { get; }

		// Token: 0x0600432A RID: 17194
		bool GetCellBool(int index);

		// Token: 0x0600432B RID: 17195
		Color GetCellExtraColor(int index);
	}
}
