using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED2 RID: 3794
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x060059F0 RID: 23024 RVA: 0x002E3515 File Offset: 0x002E1915
		private void Start()
		{
			Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
		}
	}
}
