namespace AYSTravel.Web.ViewModels
{
    public class HotelViewModel
    {
        // ===== DONNÉES BASE (existantes) =====
        public int Id { get; set; }
        public string Nom { get; set; }
        public string ImageUrl { get; set; }
        public double Note { get; set; }
        public string BookingUrl { get; set; }
        public string Description { get; set; }
        public string VilleNom { get; set; }

        // ===== DONNÉES API BOOKING =====
        public string ApiPhotoUrl { get; set; }       // image depuis Booking API
        public double ApiPrixParNuit { get; set; }    // prix réel
        public string ApiDevise { get; set; }         // EUR, MAD...
        public string ApiNote { get; set; }           // "Superbe", "Très bien"...
        public double ApiNoteScore { get; set; }      // 8.5 / 10

        // ===== PROPRIÉTÉS CALCULÉES =====

        // Utilise l'image API si disponible, sinon celle de la DB
        public string PhotoFinale =>
            !string.IsNullOrEmpty(ApiPhotoUrl) ? ApiPhotoUrl : ImageUrl;

        // Utilise la note API si disponible, sinon celle de la DB
        public double NoteFinale =>
            ApiNoteScore > 0 ? ApiNoteScore : Note;

        // Affiche le prix si disponible
        public bool HasPrix => ApiPrixParNuit > 0;
    }
}