using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED1 RID: 3793
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x060059F0 RID: 23024 RVA: 0x002E32F5 File Offset: 0x002E16F5
		private void Start()
		{
			Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
		}
	}
}
