using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED1 RID: 3793
	internal class BlackScreenFixer : MonoBehaviour
	{
		// Token: 0x060059CE RID: 22990 RVA: 0x002E12E9 File Offset: 0x002DF6E9
		private void Start()
		{
			Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
		}
	}
}
