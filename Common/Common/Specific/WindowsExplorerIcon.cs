using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Common.Specific
{
	// Code is from here...
	// http://www.brad-smith.info/blog/archives/164
	public class WindowsExplorerIcon
	{

		internal const uint SHGFI_ICON = 0x100;
		internal const uint SHGFI_LARGEICON = 0x0;
		internal const uint SHGFI_SMALLICON = 0x1;
		const uint SHGFI_USEFILEATTRIBUTES = 0x10;


		#region Win32 stuff for getting icon

		// 2 size options for the icon
		public enum ShellIconSize : uint
		{
			SmallIcon = SHGFI_ICON | SHGFI_SMALLICON,
			LargeIcon = SHGFI_ICON | SHGFI_LARGEICON
		}


		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(
			string pszPath,
			uint dwFileAttributes,
			ref SHFILEINFO psfi,
			uint cbSizeFileInfo,
			ShellIconSize uFlags
		);


		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public extern static bool DestroyIcon(IntPtr handle);


		[StructLayout(LayoutKind.Sequential)]
		public struct SHFILEINFO
		{
			public IntPtr hIcon;
			public IntPtr iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			public string szTypeName;
		};

		#endregion


		// This returns the windows explorer icon for a file
		// The resolution is supposed to be slightly higher than using CommonImages.GetFileIcon
		public static ImageSource GetFileIconHighRes(string filePath, ShellIconSize size)
		{
			ArgumentUtil.IsFilePath(filePath, "filePath", "GetFileIconHighRes");

			Icon icon = null;
			SHFILEINFO shinfo = new SHFILEINFO();
			SHGetFileInfo(filePath, 0, ref shinfo, (uint) Marshal.SizeOf(shinfo), size);

			if (shinfo.hIcon.ToInt32() != 0)
			{
				icon = (Icon) Icon.FromHandle(shinfo.hIcon).Clone();
				DestroyIcon(shinfo.hIcon);
			}

			if (icon != null)
			{
				Image image = icon.ToBitmap() as Image;
				if (image != null)
					return Imaging.CreateBitmapSourceFromHIcon(icon.Handle, 
						new Int32Rect(0, 0, icon.Width, icon.Height), BitmapSizeOptions.FromEmptyOptions());
				else
					return null;
			}
			else
				return null;
		}


		// TODO : Test this
		// Returns a higher res icon for a given file extension
		public static ImageSource GetIconForExtension(string extension, ShellIconSize size)
		{
			ArgumentUtil.IsNotWhiteSpaceOrNull(extension, "extension", "GetIconForExtension");

			size |= (ShellIconSize) SHGFI_USEFILEATTRIBUTES;
			return GetFileIconHighRes(extension, size);
		}
		
	}
}
