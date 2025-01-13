namespace Solara.Main.Payload;

public class BaseModel<TResponseModel, TRequestModel> : BaseModel where TResponseModel : class where TRequestModel : class
{
    public TResponseModel? Response { get; set; }
    public TRequestModel? Request { get; set; }
}

public class BaseModel<TResponseRequestModel> : BaseModel where TResponseRequestModel : class
{
    public TResponseRequestModel? ResponseRequest { get; set; }
};

public class BaseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
};