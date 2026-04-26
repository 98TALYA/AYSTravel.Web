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

        // ===== AJOUTS =====
        public string Description { get; set; }
        public string VilleNom { get; set; }

        // ===== FOURSQUARE API =====
        public string ApiPhotoUrl { get; set; }
        public double ApiNote { get; set; }
        public string ApiAdresse { get; set; }
        public string ApiWebsite { get; set; }

        // Utilise photo API si disponible sinon DB
        public string PhotoFinale =>
            !string.IsNullOrEmpty(ApiPhotoUrl) ? ApiPhotoUrl : ImageUrl;
    }
 }