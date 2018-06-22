using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ECF RID: 3791
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x060059ED RID: 23021 RVA: 0x002E31D5 File Offset: 0x002E15D5
		private void Start()
		{
			Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
		}
	}
}
