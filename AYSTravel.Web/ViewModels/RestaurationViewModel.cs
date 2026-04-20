namespace AYSTravel.Web.ViewModels
{
    public class RestaurationViewModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Type { get; set; } // Restaurant ou Café
        public string Adresse { get; set; }
        public string Budget { get; set; } // Economique, Moyen, Elevé
        public string ImageUrl { get; set; }
        public int VilleId { get; set; }
    }
}
