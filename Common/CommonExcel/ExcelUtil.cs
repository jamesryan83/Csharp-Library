using Common;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace CommonExcel
{
	public interface IRowData { }

	public class ExcelUtil
	{
		public static readonly long LAST_ROW = 1048576;
		public static readonly int LAST_COLUMN = 16384;


		#region Open/Close Excel

		// Returns the currently running Excel application
		// this is required to do anything with excel from c#
		public static Application GetRunningExcelApp()
		{
			Application excelApp;

			try { excelApp = (Application) Marshal.GetActiveObject("Excel.Application"); }
			catch { return null; }

			// Check there are workbooks in the excel file
			if (excelApp.Workbooks.Count == 0)
			{				
				CloseExcel(excelApp);
				return null;
			}

			return excelApp;
		}


		public static Workbook OpenExcelWorkbook(string workbookPath)
		{
			try
			{
				Application excel = new Application();
				Workbook wb = excel.Workbooks.Open(workbookPath);
				return wb;
			}
			catch
			{
				return null;
			}
		}



		// Close some of the excel stuff
		public static void CloseExcel(Application excelApp, Workbook workbook = null)
		{						
			if (excelApp != null)
			{
				if (workbook != null)
					Marshal.ReleaseComObject(workbook);
				else
					for (int i = 1; i <= excelApp.Workbooks.Count; i++)
						Marshal.ReleaseComObject(excelApp.Workbooks[i]);

				excelApp.Quit();
				Marshal.ReleaseComObject(excelApp);
				GC.Collect();
				GC.WaitForPendingFinalizers();
			}		
		}


		// Force close any instances of excel that are open
		public static void ForceCloseExcel()
		{
			SystemUtil.KillSystemProcess("Excel");
		}

		#endregion


		#region Excel Info
		
		// Returns true if excel is running
		public static bool IsExcelRunning()
		{
			return SystemUtil.IsSystemProcessRunning("excel");
		}
				
		#endregion
		

		#region Workbook Stuff

		// Returns a list of workbook names in the open Excel instance
		public static List<string> GetListOfWorkbookNames(Application excelApp = null)
		{
			if (excelApp == null)
			{
				excelApp = GetRunningExcelApp();
				if (excelApp == null)
					return null;
			}

			List<string> workbookNames = new List<string>();
			foreach (Workbook w in excelApp.Workbooks)
				workbookNames.Add(w.Name);

			return workbookNames;
		}

		#endregion


		#region Get Last Row/Column Number

		// Returns the row number of the last cell with a value
		public static long GetLastUsedRow(Worksheet ws, long columnNumber)
		{
			if (ws == null || columnNumber < 1 || columnNumber > LAST_COLUMN)
				return -1;

			if (ws.Cells[LAST_ROW, columnNumber].Value != null)
				return LAST_ROW;
			else
			{
				long temp = ws.Cells[ws.Rows.Count, columnNumber].End(XlDirection.xlUp).Row;

				if (temp == 1 && ws.Cells[1, columnNumber].Value != null)
					return temp;
				else if (temp == 1) // if first row and cell is empty, return -1
					return -1;

				return temp;
			}
		}


		// Returns the last used row of a range of columns
		public static long GetLastUsedRowOfMultipleColumns(Range selectedRange)
		{
			if (selectedRange == null)
				return -1;

			List<long> lastRowList = new List<long>();
			for (int i = 0; i < selectedRange.Columns.Count; i++)
				lastRowList.Add(GetLastUsedRow(selectedRange.Worksheet, selectedRange.Column + i));

			return lastRowList.Max();
		}


		// Returns the row number of the first cell with a value in a range
		public static long GetFirstUsedRow(Worksheet ws, long columnNumber)
		{
			if (ws == null || columnNumber < 1 || columnNumber > LAST_COLUMN)
				return -1;

			if (ws.Cells[1, columnNumber].Value != null)
				return 1;
			else
			{
				long temp = ws.Cells[1, columnNumber].End(XlDirection.xlDown).Row;

				if (temp == LAST_ROW && ws.Cells[LAST_ROW, columnNumber].Value != null)
					return temp;
				else if (temp == LAST_ROW) // if last row and cell is empty, return -1
					return -1;

				return temp;
			}
		}


		// Returns the row number of the first cell with a value in a range
		public static long GetFirstUsedRowOfMultipleColumns(Range selectedRange)
		{
			if (selectedRange == null)
				return -1;

			List<long> firstRowList = new List<long>();

			for (int i = 0; i < selectedRange.Columns.Count; i++)
				firstRowList.Add(GetFirstUsedRow(selectedRange.Worksheet, selectedRange.Column + i));

			// can't use firstRowList.Min() because it returns -1 all the time
			long tempMin = long.MaxValue;
			foreach (long l in firstRowList)
				if (l != -1 && l < tempMin)
					tempMin = l;

			if (tempMin == long.MaxValue)
				return -1;
			else
				return tempMin;
		}


		// Returns the column number of the last cell with a value
		public static int GetFirstUsedColumn(Worksheet ws, long rowNumber)
		{
			if (ws == null || rowNumber < 1 || rowNumber > LAST_ROW)
				return -1;

			if (ws.Cells[rowNumber, 1].Value != null)
				return 1;
			else
			{
				int temp = ws.Cells[rowNumber, 1].End(XlDirection.xlToRight).Column;

				if (temp == LAST_COLUMN && ws.Cells[rowNumber, LAST_COLUMN].Value != null)
					return temp;
				else if (temp == LAST_COLUMN) // if last column cell is empty, return -1
					return -1;
				
				return temp;
			}
		}


		// Returns the column number of the last cell with a value
		public static int GetLastUsedColumn(Worksheet ws, long rowNumber)
		{
			if (ws == null || rowNumber < 1 || rowNumber > LAST_ROW)
				return -1;

			if (ws.Cells[rowNumber, LAST_COLUMN].Value != null)
				return LAST_COLUMN;
			else
			{
				int temp = ws.Cells[rowNumber, LAST_COLUMN].End(XlDirection.xlToLeft).Column;

				if (temp == 1 && ws.Cells[rowNumber, 1].Value != null)
					return temp;
				else if (temp == 1) // if last column cell is empty, return -1
					return -1;

				return temp;
			}
		}

		#endregion


		#region Worksheet Stuff

		// Creates a new Worksheet
		public static Worksheet CreateWorksheet(Workbook wb, string sheetName)
		{
			if (wb == null || sheetName == null || sheetName.Length == 0)
				return null;

			foreach (Worksheet wks in wb.Sheets)
				if (wks.Name == sheetName)
					return wks;

			Worksheet ws = wb.Sheets.Add();
			ws.Name = sheetName;
			ws.Move(wb.Sheets[wb.Sheets.Count]);
			return ws;
		}


		// Get a worksheet by its name
		public static Worksheet GetWorksheetByName(Application excelApp, string name)
		{
			if (excelApp == null || name == null || name.Length == 0)
				return null;

			if (CheckIfWorksheetExists(excelApp, name) == false)
				return null;

			return excelApp.Worksheets[name];
		}


		// Check if worksheet exists or not		
		public static bool CheckIfWorksheetExists(Application excelApp, string name)
		{
			if (excelApp == null || name == null || name.Length == 0)
				return false;

			foreach (Worksheet sheet in excelApp.Worksheets)
				if (sheet.Name == name)
					return true;

			return false;
		}


		// Make a whole row bold
		public static bool MakeWholeRowBold(Worksheet ws, int rowNumber)
		{
			if (ws == null || rowNumber < 1 || rowNumber > LAST_ROW)
				return false;

			ws.Cells[rowNumber, 1].EntireRow.Font.Bold = true;
			return true;
		}


		// Puts the standard black border around all cells in the range
		public static bool BorderAroundAllCells(Range range)
		{
			if (range == null)
				return false;

			range.Borders.LineStyle = XlLineStyle.xlContinuous;
			return true;
		}


		// Sets a range to have the Currency Format
		public static bool SetRangeToCurrencyFormat(Range range)
		{
			if (range == null)
				return false;
			
			range.NumberFormat = "$#,###.00";
			return true;
		}


		// Sets a column to have the Currency Format
		public static bool SetColumnToCurrencyFormat(Worksheet ws, int columnNumber)
		{
			if (ws == null || columnNumber < 1 || columnNumber > 16300)
				return false;

			Range range = ws.Range[ws.Cells[1, columnNumber], ws.Cells[ws.Rows.Count - 1, columnNumber]];
			range.NumberFormat = "$#,###.00";
			return true;
		}


		// Merge and center a range			
		public static bool MergeAndCentre(Range range)
		{
			if (range == null)
				return false;

			range.Merge();
			range.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter;
			return true;
		}


		// Returns the Address of a selected Range in Excel.  Lock Row/Column adds the $ sign
		public static string GetSelectedRangeAddressAsString(Application excelApp = null, bool lockRow = false, bool lockColumn = false)
		{
			if (excelApp == null)
			{
				excelApp = GetRunningExcelApp();
				if (excelApp == null)
					return null;
			}

			Range r = excelApp.Selection;						
			return r.Address[lockRow, lockColumn];			
		}


		// Returns the Address of a selected Range in Excel.  Lock Row/Column adds the $ sign
		public static string GetRangeAddressAsString(Range range, bool lockRow = false, bool lockColumn = false)
		{
			if (range == null)
				return null;

			return range.Address[lockRow, lockColumn];
		}


		// Returns the date from an Excel cell value
		public static DateTime? GetDateFromCellValue(object cellValue)
		{
			if (cellValue == null)
				return null;

			string tempString = cellValue.ToString();

			// Try to parse date as is
			DateTime temp;
			if (DateTime.TryParse(tempString, out temp) == false)
			{
				// Date might be in its number format, so try convert from that
				try { return DateTime.FromOADate(Convert.ToDouble(cellValue)); }
				catch { return null; }
			}
			else
				return temp;
		}


		// Sets the print area so the printRange fits on the screen
		public static void SetPrintArea(Range printRange)
		{
			if (printRange == null)
				return;

			Worksheet ws = printRange.Worksheet;
			ws.PageSetup.FitToPagesTall = 1;
			ws.PageSetup.FitToPagesWide = 1;
			ws.PageSetup.Zoom = false;
			ws.PageSetup.PrintArea = printRange.Address;
		}

		#endregion


		#region Excel Data

		// Get the Selected Range as an object[,]
		// Range must have at least 2 rows or 2 columns
		// returned object array is base 1
		public static object[,] GetSelectedRangeAs2DObjectArray(Application excelApp = null)
		{
			if (excelApp == null)
			{
				excelApp = GetRunningExcelApp();
				if (excelApp == null)
					return null;
			}

			Range r = excelApp.Selection;
			return r.Value as object[,];
		}


		// Get the Selected Range as an object[,]
		// Range must have at least 2 rows or 2 columns
		// returned object array is base 1
		public static object[,] GetRangeAs2DObjectArray(Range range)
		{
			if (range == null)
				return null;
						
			return range.Value as object[,];
		}


		// Puts an object[,] into a Worksheet Range
		// Output range needs to be same size as data range
		public static bool Put2DObjectArrayIntoRange(object[,] data, Worksheet ws, long startRow, long startCol, long finishRow, long finishCol, bool autoFit = false)
		{
			if (data == null || ws == null)
				return false;

			if (startRow < 1 || startCol < 1 || finishRow > LAST_ROW || finishCol > LAST_COLUMN)
				return false;

			ws.Range[ws.Cells[startRow, startCol], ws.Cells[finishRow, finishCol]] = data;

			if (autoFit == true)
				ws.Columns.AutoFit();

			return true;
		}


		// Replaces all null values in a 2D object[,] with ""
		public static object[,] RemoveNullsFrom2DObjectArray(object[,] objectArray, bool isBase1 = false)
		{
			if (isBase1 == false)
				for (int i = 0; i < objectArray.GetLength(0); i++)
					for (int j = 0; j < objectArray.GetLength(1); j++)
						if (objectArray[i, j] == null)
							objectArray[i, j] = "";

			if (isBase1 == true)
				for (int i = 1; i <= objectArray.GetLength(0); i++)
					for (int j = 1; j <= objectArray.GetLength(1); j++)
						if (objectArray[i, j] == null)
							objectArray[i, j] = "";

			return objectArray;
		}


		// Removes blank rows from an Excel range that has been copied into an object[,]
		// when anyCellMissingMeansBlankRow = true, rows with any blank values will be ignored
		// otherwise the whole row will have to be blank
		// The returned object[,] will be base 0
		// No way to easily remove or copy rows from object[,], so need to loop through everything
		public static object[,] RemoveBlankRowsFrom2DObjectArray(object[,] currentRange, bool isBase1 = false, bool anyCellMissingMeansBlankRow = false)
		{
			if (currentRange == null)
				return null;

			object[,] newRange = null;
			List<int> nonBlankRows = new List<int>();

			// Find all the rows with no null values
			if (isBase1 == true)
			{
				for (int i = 1; i <= currentRange.GetLength(0); i++)
				{
					int nullCount = 0;
					for (int j = 1; j <= currentRange.GetLength(1); j++)
						if (currentRange[i, j] == null)
							nullCount++;

					int emptyCount = 0;
					for (int j = 1; j <= currentRange.GetLength(1); j++)
						if (currentRange[i, j] != null && currentRange[i, j].ToString() == "")
							emptyCount++;

					if (anyCellMissingMeansBlankRow == true && (nullCount > 0 || emptyCount > 0))
						continue;

					if (nullCount != currentRange.GetLength(1) && emptyCount != currentRange.GetLength(1) && (nullCount + emptyCount) != currentRange.GetLength(1))
						nonBlankRows.Add(i);
				}
			}
			else
			{
				// same logic as above, but base 0 instead of 1
				for (int i = 0; i < currentRange.GetLength(0); i++)
				{
					int nullCount = 0;
					for (int j = 0; j < currentRange.GetLength(1); j++)
						if (currentRange[i, j] == null)
							nullCount++;

					int emptyCount = 0;
					for (int j = 0; j < currentRange.GetLength(1); j++)
						if (currentRange[i, j] != null && currentRange[i, j].ToString() == "")
							emptyCount++;

					if (anyCellMissingMeansBlankRow == true && (nullCount > 0 || emptyCount > 0))
						continue;

					if (nullCount != currentRange.GetLength(1) && emptyCount != currentRange.GetLength(1) && (nullCount + emptyCount) != currentRange.GetLength(1))
						nonBlankRows.Add(i);
				}
			}

			if (nonBlankRows.Count == 0)
				return null;

			// Put non-blank rows into new object[,]
			newRange = new object[nonBlankRows.Count, currentRange.GetLength(1)];
			for (int i = 0; i < nonBlankRows.Count; i++)
				for (int j = 1; j <= currentRange.GetLength(1); j++)
					newRange[i, j - 1] = currentRange[nonBlankRows[i], j];

			return newRange;
		}

		#endregion
		

		#region Excel to Csv
				
		// Save the currently selected range to csv
		// this ignores blank rows 
		// the selected range must be bigger than 1 row and 1 column
		public static List<string> SaveSelectedRangeToCsv(Application excelApp = null, Range range = null, string delimiter = ",", bool anyCellMissingMeansBlankRow = false)
		{
			if (excelApp == null)
			{
				excelApp = GetRunningExcelApp();
				if (excelApp == null)
					return null;
			}

			if (delimiter == null || delimiter == "")
				delimiter = ",";


			if (range == null)
				range = excelApp.Selection;


			// Can't have 1 row and 1 column, it won't save into the object[,]
			if (range.Rows.Count == 1 && range.Columns.Count == 1)
				return null;

			object[,] dataRows = range.Value as object[,];
			dataRows = ExcelUtil.RemoveBlankRowsFrom2DObjectArray(dataRows, true, anyCellMissingMeansBlankRow);
					

			// Make csv strings
			List<string> outputData = new List<string>();
			for (int i = 0; i < dataRows.GetLength(0); i++)
			{
				string tempString = "";
				for (int j = 0; j < dataRows.GetLength(1); j++)
				{
					if (j == dataRows.GetLength(1) - 1)
						tempString += dataRows[i, j] == null ? "" : dataRows[i, j].ToString();
					else
						tempString += (dataRows[i, j] == null ? "" : dataRows[i, j].ToString()) + delimiter;
				}

				outputData.Add(tempString);
			}

			return outputData;
		}


		// TODO : test
		// Save a list of data to a csv file
		public static void SaveDataToCsvFile(List<IRowData> rowData, string headers, string filePath)
		{
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				writer.WriteLine(headers);
				foreach (object d in rowData)
					writer.WriteLine(d.ToString());
			}
		}

		#endregion


		// Returns the column letter for the given column index (starting at 1)
		// http://stackoverflow.com/a/182924
		public static string GetColumnLetterFromColumnNumber(int columnNumber)
		{
			if (columnNumber < 1 || columnNumber > LAST_COLUMN)
				return null;

			int dividend = columnNumber;
			string columnName = String.Empty;
			int modulo;

			while (dividend > 0)
			{
				modulo = (dividend - 1) % 26;
				columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
				dividend = (int) ((dividend - modulo) / 26);
			}

			return columnName;
		}


		// Convert a hex color (ie. #123456) to an Excel color which is just an int
		public static int? ConvertHexColorStringToExcelColor(string colorStringInHex)
		{
			if (colorStringInHex == null)
				return null;

			if (colorStringInHex.StartsWith("#") == false)
				colorStringInHex = "#" + colorStringInHex;

			try
			{
				return ColorTranslator.ToOle(ColorTranslator.FromHtml(colorStringInHex));
			}
			catch
			{
				return null;
			}
			
		}
	}
}
