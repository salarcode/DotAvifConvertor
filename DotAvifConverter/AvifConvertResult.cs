namespace DotAvifConverter;

public class AvifConvertResult
{
	public AvifConvertResult()
	{
	}

	public AvifConvertResult(bool success)
	{
		Success = success;
	}

	public AvifConvertResult(bool success, string message)
	{
		Success = success;
		Message = message;
	}

	public bool Success { get; }

	public string Message { get; } = string.Empty;
}