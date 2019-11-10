using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Prometei.Api
{
	internal static class Utils
	{
		static Utils()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}

		public static string ByteArrayTo1251String(byte[] bytes)
		{
			return Encoding.GetEncoding(1251).GetString(bytes);
		}
	}
}
