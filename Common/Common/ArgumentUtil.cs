using System;
using System.IO;
using System.Linq.Expressions;

namespace Common
{
	// http://stackoverflow.com/a/9801735   // to get parameter names
	// http://weblogs.asp.net/fredriknormen/how-to-validate-a-method-s-arguments    // to validate parameters

	// Used to validate method arguments
	public class ArgumentUtil
	{
		public static readonly string regexLetters = @"^[a-zA-Z]+$";
		public static readonly string regexLettersAndSpace = @"^[a-zA-Z ]+$";
		public static readonly string regexLettersAndNumbers = @"^[a-zA-Z0-9]+$";
		public static readonly string regexLettersAndNumbersAndSpace = @"^[a-zA-Z0-9 ]+$";


		#region Get method info

		// Returns the name of a Csharp method parameter.  
		// Call like this : ArgumentUtil.GetParameterName<T>(() => myParameter)		
		public static string GetParameterName<T>(Expression<Func<T>> memberExpression)
		{
			MemberExpression expression = memberExpression.Body as MemberExpression;
			if (expression != null)
				return expression.Member.Name;
			else
				throw new ArgumentException("Error getting string from paramenter name", "memberExpression");
		}

		// Returns the name of a Csharp method
		// Call like this : ArgumentUtil.GetMethodName<ClassName>(x => MethodName(null, ""))  any values can go inside the ()
		// http://stackoverflow.com/a/1524327
		public static string GetMethodName<T>(Expression<Action<T>> action)
		{
			MethodCallExpression methodCall = action.Body as MethodCallExpression;
			if (methodCall != null)
				return methodCall.Method.Name;
			else
				throw new ArgumentException("Error getting method name", GetParameterName<Expression<Action<T>>>(() => action));
		}

		#endregion


		// String - Null or whitespace		
		public static void IsNotWhiteSpaceOrNull(string value, string argumentName = "", string methodName = "", string message = "Unexpected empty String")
		{
			if (string.IsNullOrWhiteSpace(value) == true)
				throw new ExtendedArgumentException(value, argumentName, methodName, message);
		}

		// String - Is a file path
		public static void IsFilePath(string value, string argumentName = "", string methodName = "", string message = "Error in File Path")
		{
			IsNotWhiteSpaceOrNull(value, argumentName, message);
						
			if (FileFolderUtil.IsPathADirectory(value) == true || File.Exists(value) == false)
				throw new ExtendedArgumentException(value, argumentName, methodName, message);
		}

		// String - Is a folder path
		public static void IsFolderPath(string value, string argumentName = "", string methodName = "", string message = "Error in Folder Path")
		{
			IsNotWhiteSpaceOrNull(value, argumentName, message);

			if (FileFolderUtil.IsPathADirectory(value) == false)
				throw new ExtendedArgumentException(value, argumentName, methodName, message);
		}




		// Long - Value is greater than a limit value.  If value <= 3 and limitValue = 3 it will throw an exception.  Value must be greater than the limit
		public static void IsValueGreaterThan(long value, long limitValue, string argumentName = "", string methodName = "", string message = "Integer was out of range")
		{
			if (value <= limitValue)
				throw new ExtendedArgumentException(value.ToString() + " < limit of " + limitValue.ToString(), argumentName, methodName, message);
		}




		// Is Not Null
		public static void IsNotNull<T>(T value, string argumentName = "", string methodName = "", string message = "Value was null")
		{
			if (value == null)
				throw new ExtendedArgumentException(typeof(T).ToString() + " was null", argumentName, methodName, message);
		}
	}


	
	// Custom argument exception
	public class ExtendedArgumentException : ArgumentException
	{
		public string message { get; set; }
		public string value { get; set; }
		public string argumentName { get; set; }
		public string methodName { get; set; }
		public string stackTrace { get; set; }

		public ExtendedArgumentException(string valueAsString, string argumentName, string methodName, string message)
			: base(message + "\nValue : " + valueAsString)
		{
			this.message = message;
			this.value = valueAsString;
			this.argumentName = argumentName;
			this.methodName = methodName;			
		}

		public override string ToString()
		{
			return
				"An Error has Occurred....\n\n" + 
				"Message : " + message + "\n" +
				"Value : " + value + "\n\n" +
				"Parameter : " + argumentName + "\n" +
				"Method : " + methodName + "\n\n" +
				"StackTrace (the first one is where it failed) : " + 
					(this.stackTrace != null ? this.stackTrace.Replace(" at ", "\n").Replace(" in ", "\n") : @"N/A"); // stacktrace or N/A
		}
	}
}


// Examples...

// Get parameters and method names automatically

//    ArgumentUtil.IsNotNull(value, ArgumentUtil.GetParameterName<List<string>>(() => value), ArgumentUtil.GetMethodName<DataUtil>(x => GetDataTable(null)),"Message");

//    ArgumentUtil.IsValueGreaterThan(value, -1, ArgumentUtil.GetParameterName<long>(() => value), "Message");

//    ArgumentUtil.IsFilePath(filePath, ArgumentUtil.GetParameterName<string>(() => filePath), ArgumentUtil.GetMethodName<CsvUtil>(x => ReadHeadersFromCsv(null, ""))); 

//    ArgumentUtil.IsFolderPath(folderPath, ArgumentUtil.GetParameterName<string>(() => folderPath), ArgumentUtil.GetMethodName<FileFolderUtil>(x => GetAllFilePaths(null, null, false)));

//    ArgumentUtil.IsNotWhiteSpaceOrNull(newName, ArgumentUtil.GetParameterName<string>(() => newName), ArgumentUtil.GetMethodName<FileFolderUtil>(x => RenameFolder(null, null)));


// Define parameters and method names as strings

//    ArgumentUtil.IsNotNull(value, "filePath", "GetFileIconHighRes", "Message");

//    ArgumentUtil.IsValueGreaterThan(value, -1, "filePath", "GetFileIconHighRes", "Message");

//    ArgumentUtil.IsFilePath(filePath, "filePath", "GetFileIconHighRes", "Message"); 

//    ArgumentUtil.IsFolderPath(folderPath, "filePath", "GetFileIconHighRes", "Message");

//    ArgumentUtil.IsNotWhiteSpaceOrNull(newName, "filePath", "GetFileIconHighRes", "Message");