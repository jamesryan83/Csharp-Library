using Microsoft.Office.Interop.Excel;

namespace CommonExcel
{
	public class ExcelVBA
	{

		// Runs a macro in Excel
		public static bool RunExcelMacro(string excelMethodName, Application excelApp = null)
		{
			try
			{
				if (excelApp == null)
				{
					excelApp = ExcelUtil.GetRunningExcelApp();
					if (excelApp == null)
						return false;
				}

				excelApp.Run(excelMethodName);
				return true;
			}
			catch
			{
				return false;
			}
		}


		// Create a VBA code module 
		public static bool CreateVBACodeModule(Application excelApp, string moduleName)
		{
			try
			{
				// Create the vba module CsharpTempModule
				Microsoft.Vbe.Interop.VBComponent newStandardModule = 
					excelApp.ActiveWorkbook.VBProject.VBComponents.Add
				(
					Microsoft.Vbe.Interop.vbext_ComponentType.vbext_ct_StdModule
				);

				Microsoft.Vbe.Interop.CodeModule codeModule = newStandardModule.CodeModule;
				newStandardModule.Name = moduleName;

				return true;
			}
			catch
			{
				return false;
			}
		}


		// Delete a VBA code module 
		public static bool DeleteVBACodeModule(Application excelApp, string moduleName)
		{
			try
			{
				Microsoft.Vbe.Interop.VBComponents components = excelApp.ActiveWorkbook.VBProject.VBComponents.Parent.VBComponents;
				for (int i = 1; i <= components.Count; i++)
					if (components.Item(i).Name == moduleName)
						excelApp.ActiveWorkbook.VBProject.VBComponents.Parent.VBComponents.Remove(components.Item(i));

				return true;
			}
			catch
			{
				return false;
			}
		}		


		// TODO : fix up
		// Adds some code to a module
		public static bool AddTempCodeToVBAModule(Application excelApp)
		{
			return false;
			//try
			//{
			//	Microsoft.Vbe.Interop.CodeModule codeModule = excelApp.ActiveWorkbook.VBProject.VBComponents.Item()

			//	 add vba code to module
			//	string macroName = "CsharpTempSub";
			//	string vbaNewLine = "\r\n";
			//	string clientProgramWs = "ThisWorkbook.ActiveSheet";

			//	string macro = string.Format("'This sub was generated automatically...{0}" +
			//	"Public Sub CsharpTempSub(){0}" +					
			//		"Dim i as Integer{0}" +
			//		"For i = 1 To 10{0}" +						
			//		"Next{0}" +
			//	"End Sub", vbaNewLine);

			//	codeModule.InsertLines(codeModule.CountOfLines + 1, macro);

			//	return true;
			//}
			//catch
			//{
			//	return false;
			//}
		}
	}
}
