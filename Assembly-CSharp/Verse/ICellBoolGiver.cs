using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C00 RID: 3072
	public interface ICellBoolGiver
	{
		// Token: 0x17000A90 RID: 2704
		// (get) Token: 0x06004335 RID: 17205
		Color Color { get; }

		// Token: 0x06004336 RID: 17206
		bool GetCellBool(int index);

		// Token: 0x06004337 RID: 17207
		Color GetCellExtraColor(int index);
	}
}
