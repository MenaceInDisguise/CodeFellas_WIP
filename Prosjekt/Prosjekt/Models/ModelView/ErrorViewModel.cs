namespace Prosjekt.Models.ModelView
{
    //Viewmodel used to show information about an error.
    public class ErrorViewModel
    {
        //Unique ID for the request that caused the error.
        public string? RequestId { get; set; }

        //Checks whether RequestID exists and decides whether it should be shown.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}