namespace Prosjekt.Models.ModelView
{
    //Viewmodel som brukes til å vise informasjon om en feil.
    public class ErrorViewModel
    {
        //Unik ID for forespørselen som førte til feilen.
        public string? RequestId { get; set; }

        //Sjekker om RequestID finnes og bestemmer om den skal vises.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}