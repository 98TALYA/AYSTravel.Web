namespace AYSTravel.Web.ViewModels
{
    public class MatchViewModel
    {
        public int Id { get; set; }

        public string Equipe1 { get; set; }
        public string Equipe2 { get; set; }

        public string Stade { get; set; }
        public DateTime Date { get; set; }

        public string VilleNom { get; set; }
    }
}