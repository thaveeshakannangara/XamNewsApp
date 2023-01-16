using Plugin.FirebaseCrashlytics;
using System;
using System.Text.RegularExpressions;

namespace NewsApp.Helpers
{
	public static class Validators
	{
		public static bool IsEmail(string email)
		{
			try
			{
				string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
										+ @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
										+ @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
				return Regex.Match(email, validEmailPattern).Success;
			}
			catch (Exception ex)
			{
				CrossFirebaseCrashlytics.Current.RecordException(ex);
				return false;
			}
		}
	}
}