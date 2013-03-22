 
// Enums Input file 
// C:\Utils\Dropbox\Dropbox\Misc\Authorize.V1\ClassLibrary.Data\Resources\Enums.xml
using System;
namespace ClassLibrary.Data
{
	[Flags]
	public enum Operators  : byte
	{
		All = 1,
		Or = 2,
	}
	[Flags]
	public enum Permissions 
	{
		Read = (1 << 0),
		Write = (1 << 1),
		Delete = (1 << 2),
		Approve = (1 << 3),
		Deny = (1 << 4),
		Activate = (1 << 5),
	}
	[Flags]
	public enum Modules  : byte
	{
		Program = 1,
		Patient = 2,
		Application = 3,
	}
}
  
