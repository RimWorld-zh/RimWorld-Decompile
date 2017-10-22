using System;
using UnityEngine;

namespace Verse
{
	public static class SimpleColorExtension
	{
		public static Color ToUnityColor(this SimpleColor color)
		{
			Color result;
			switch (color)
			{
			case SimpleColor.White:
			{
				result = Color.white;
				break;
			}
			case SimpleColor.Red:
			{
				result = Color.red;
				break;
			}
			case SimpleColor.Green:
			{
				result = Color.green;
				break;
			}
			case SimpleColor.Blue:
			{
				result = Color.blue;
				break;
			}
			case SimpleColor.Magenta:
			{
				result = Color.magenta;
				break;
			}
			case SimpleColor.Yellow:
			{
				result = Color.yellow;
				break;
			}
			case SimpleColor.Cyan:
			{
				result = Color.cyan;
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}
	}
}
