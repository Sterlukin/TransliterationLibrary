namespace TransliterationLibrary
{
	public class OperationResult
	{
		public bool IsSuccess { get; set; }
		public string Message { get; set; }

		public static OperationResult Fail(string message = null)
		{
			return new OperationResult() { IsSuccess = false, Message = message };
		}

		public static OperationResult Success()
		{
			return new OperationResult() { IsSuccess = true, Message = string.Empty };
		}
	}

	public class OperationResult<TData> : OperationResult where TData : class
	{
		public TData Data { get; set; }

		public static new OperationResult<TData> Fail(string message = null)
		{
			return new OperationResult<TData> { IsSuccess = false, Message = message, Data = null };
		}

		public static OperationResult<TData> Success(TData data)
		{
			return new OperationResult<TData> { IsSuccess = true, Message = string.Empty, Data = data };
		}
	}
}
