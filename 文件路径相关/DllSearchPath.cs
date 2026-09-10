using System;
using System.Runtime.InteropServices;

string baseDir = AppDomain.CurrentDomain.BaseDirectory;

/// <summary>
/// 声明对 Windows 系统库 kernel32.dll 中 SetDllDirectory 函数的调用。
/// 作用：把指定目录加入"原生 DLL 搜索路径"，之后 DllImport 加载 DLL 时会去该目录查找。
/// </summary>
/// <param name="lpPathName">
/// 要加入搜索路径的目录（绝对路径）。
/// 传 null 表示恢复系统默认的 DLL 搜索顺序。
/// </param>
/// <returns>成功返回 true，失败返回 false（可用 Marshal.GetLastWin32Error() 获取错误码）。</returns>
[DllImport(
    "kernel32.dll",              // 目标非托管 DLL 名称（Windows 系统库）
    CharSet = CharSet.Unicode,   // 使用 Unicode 字符集，对应 Windows 的 W 版 API（SetDllDirectoryW）
    SetLastError = true          // 保留最后一次 Win32 错误码，便于失败时排查
)]
static extern bool SetDllDirectory(string lpPathName); //只能设置一个dll搜索目录，重复调用会覆盖
string nativeDllDir = Path.Combine(baseDir, "temp");
if (Directory.Exists(nativeDllDir)) SetDllDirectory(nativeDllDir);


DllSearchPath.AddSearchDirectories(
	Path.Combine(baseDir, "dll", "win"),
	Path.Combine(baseDir, "dll", "common"),
	Path.Combine(baseDir, "runtimes", "win-x64", "native")
);



//添加多个目录
public static class DllSearchPath
{
	// ---------- P/Invoke 声明 ----------
	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
	private static extern IntPtr AddDllDirectory(string newDirectory);

	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool SetDefaultDllDirectories(uint directoryFlags);

	// 搜索标志
	private const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;
	private const uint LOAD_LIBRARY_SEARCH_USER_DIRS    = 0x00000400;

	/// <summary>
	/// 批量添加多个 DLL 搜索目录（可叠加，不覆盖）。
	/// </summary>
	public static void AddSearchDirectories(params string[] dirs)
	{
		// 必须先设置默认搜索标志，AddDllDirectory 才生效
		SetDefaultDllDirectories(LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_USER_DIRS);

		foreach (var dir in dirs)
		{
			if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
				continue;

			IntPtr handle = AddDllDirectory(dir);
			if (handle == IntPtr.Zero)
			{
				int err = Marshal.GetLastWin32Error();
				Console.WriteLine($"AddDllDirectory 失败: {dir}，错误码: {err}");
			}
		}
	}
}