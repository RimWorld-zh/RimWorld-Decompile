using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Verse
{
	// Token: 0x02000CBF RID: 3263
	public class ModAssemblyHandler
	{
		// Token: 0x040030CB RID: 12491
		private ModContentPack mod;

		// Token: 0x040030CC RID: 12492
		public List<Assembly> loadedAssemblies = new List<Assembly>();

		// Token: 0x040030CD RID: 12493
		private static bool globalResolverIsSet = false;

		// Token: 0x0600480C RID: 18444 RVA: 0x0025EC79 File Offset: 0x0025D079
		public ModAssemblyHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x0025EC94 File Offset: 0x0025D094
		public void ReloadAll()
		{
			if (!ModAssemblyHandler.globalResolverIsSet)
			{
				ResolveEventHandler @object = (object obj, ResolveEventArgs args) => Assembly.GetExecutingAssembly();
				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.AssemblyResolve += @object.Invoke;
				ModAssemblyHandler.globalResolverIsSet = true;
			}
			string path = Path.Combine(this.mod.RootDir, "Assemblies");
			string path2 = Path.Combine(GenFilePaths.CoreModsFolderPath, path);
			DirectoryInfo directoryInfo = new DirectoryInfo(path2);
			if (directoryInfo.Exists)
			{
				foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.*", SearchOption.AllDirectories))
				{
					if (!(fileInfo.Extension.ToLower() != ".dll"))
					{
						Assembly assembly = null;
						try
						{
							byte[] rawAssembly = File.ReadAllBytes(fileInfo.FullName);
							string fileName = Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.FullName)) + ".pdb";
							FileInfo fileInfo2 = new FileInfo(fileName);
							if (fileInfo2.Exists)
							{
								byte[] rawSymbolStore = File.ReadAllBytes(fileInfo2.FullName);
								assembly = AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore);
							}
							else
							{
								assembly = AppDomain.CurrentDomain.Load(rawAssembly);
							}
						}
						catch (Exception ex)
						{
							Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString(), false);
							break;
						}
						if (assembly != null)
						{
							if (this.AssemblyIsUsable(assembly))
							{
								this.loadedAssemblies.Add(assembly);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x0025EE64 File Offset: 0x0025D264
		private bool AssemblyIsUsable(Assembly asm)
		{
			try
			{
				asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"ReflectionTypeLoadException getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex
				}));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Loader exceptions:");
				if (ex.LoaderExceptions != null)
				{
					foreach (Exception ex2 in ex.LoaderExceptions)
					{
						stringBuilder.AppendLine("   => " + ex2.ToString());
					}
				}
				Log.Error(stringBuilder.ToString(), false);
				return false;
			}
			catch (Exception ex3)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex3
				}), false);
				return false;
			}
			return true;
		}
	}
}
