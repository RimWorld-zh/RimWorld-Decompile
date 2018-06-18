using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zlib;

namespace Verse
{
	// Token: 0x02000ED5 RID: 3797
	public static class CompressUtility
	{
		// Token: 0x060059DC RID: 23004 RVA: 0x002E1ACC File Offset: 0x002DFECC
		public static byte[] Compress(byte[] input)
		{
			MemoryStream memoryStream = new MemoryStream();
			DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Compress);
			deflateStream.Write(input, 0, input.Length);
			deflateStream.Close();
			return memoryStream.ToArray();
		}

		// Token: 0x060059DD RID: 23005 RVA: 0x002E1B08 File Offset: 0x002DFF08
		public static byte[] Decompress(byte[] input)
		{
			MemoryStream stream = new MemoryStream(input);
			DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress);
			List<byte[]> list = null;
			byte[] array;
			int num;
			for (;;)
			{
				array = new byte[65536];
				num = deflateStream.Read(array, 0, array.Length);
				if (num < array.Length && list == null)
				{
					break;
				}
				if (num < array.Length)
				{
					goto Block_3;
				}
				if (list == null)
				{
					list = new List<byte[]>();
				}
				list.Add(array);
			}
			byte[] array2 = new byte[num];
			Array.Copy(array, array2, num);
			return array2;
			Block_3:
			byte[] array3 = new byte[num + list.Count * array.Length];
			for (int i = 0; i < list.Count; i++)
			{
				Array.ConstrainedCopy(list[i], 0, array3, i * array.Length, array.Length);
			}
			Array.ConstrainedCopy(array, 0, array3, list.Count * array.Length, num);
			return array3;
		}
	}
}
