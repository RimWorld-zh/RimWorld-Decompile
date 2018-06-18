using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED0 RID: 3792
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x060059CC RID: 22988 RVA: 0x002E13C1 File Offset: 0x002DF7C1
		private void Start()
		{
			Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
		}
	}
}
