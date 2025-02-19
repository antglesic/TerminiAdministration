namespace TerminiDomain.Core
{
	[Serializable]
	public abstract class ResponseBase<T> : IResponse where T : IRequest
	{
		public Guid ResponseToken { get; private set; } = Guid.NewGuid();
		public bool Success { get; set; }
		public string Message { get; set; }
		public List<ResponseStatus> Statuses { get; set; }
		public T Request { get; set; }
		public object ErrorResult => new
		{
			Message = Message,
			ErrorId = ResponseToken
		};

		protected ResponseBase()
		{
			Statuses = new List<ResponseStatus>();
		}
	}
}
