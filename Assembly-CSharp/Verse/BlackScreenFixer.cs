using System;
using UnityEngine;

namespace Verse
{
	internal class BlackScreenFixer : MonoBehaviour
	{
		public BlackScreenFixer()
		{
		}

		private void Start()
		{
			Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreen);
		}
	}
}
