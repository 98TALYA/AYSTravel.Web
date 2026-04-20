using AYSTravel.Web.ViewModels;
using System.ComponentModel.DataAnnotations;

public class VilleViewModel
{
    public int Id { get; set; }
    public string Nom { get; set; }
    public string Description { get; set; }
    public string Ville { get; set; }

    public string ImageUrl { get; set; }
    
    public int Population { get; set; }
    public int NombreMonuments { get; set; }
    public int NombreActivites { get; set; }
    public int NombreRestaurations { get; set; }
    public int NombreMatchs { get; set; }
    public int NombreTransports { get; set; }

    // Collections complètes
    public List<MonumentViewModel> Monuments { get; set; }
    public List<ActiviteViewModel> Activites { get; set; }
    public List<MoyenTransportViewModel> MoyensTransports { get; set; }
    public List<MatchViewModel> Matchs { get; set; }
    
    public List<VilleViewModel> VillesSimilaires { get; set; }
    public List<HotelViewModel> Hotels { get; set; } = new List<HotelViewModel>();
}