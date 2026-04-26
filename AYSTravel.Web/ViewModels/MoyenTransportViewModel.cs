namespace AYSTravel.Web.ViewModels
{
    public class MoyenTransportViewModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Tarif { get; set; }
        public string HeureDepart { get; set; }
        public string HeureArrivee { get; set; }
        public string VilleNom { get; set; }
    }
}