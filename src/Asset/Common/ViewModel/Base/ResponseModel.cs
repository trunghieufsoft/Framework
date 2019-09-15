namespace Asset.Common.ViewModel
{
    public class ResponseModel
    {
        public ErrorModel Error { get; set; }

        public object Data { get; set; }

        public bool? Success { get; set; }
    }
}