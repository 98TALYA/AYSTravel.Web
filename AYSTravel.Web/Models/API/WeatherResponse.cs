using Newtonsoft.Json;

namespace AYSTravel.Web.Models.API
{
    // ===== MÉTÉO =====
    public class WeatherResponse
    {
        public Main main { get; set; }
        public Weather[] weather { get; set; }
        public string name { get; set; }
    }

    public class Main
    {
        public double temp { get; set; }
        public int humidity { get; set; }
    }

    public class Weather
    {
        public string description { get; set; }
        public string icon { get; set; }
    }

    // ===== FOOTBALL LIVE =====
    public class FootballResponse
    {
        public List<LiveFixture> Response { get; set; }
    }

    public class LiveFixture
    {
        public FixtureDetail Fixture { get; set; }
        public LiveTeams Teams { get; set; }
        public LiveGoals Goals { get; set; }
        public LeagueDetail League { get; set; }
    }

    public class FixtureDetail
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public FixtureStatus Status { get; set; }
        public FixtureVenue Venue { get; set; }
    }

    public class FixtureStatus
    {
        public string Long { get; set; }
        public string Short { get; set; }
        public int? Elapsed { get; set; }
    }

    public class FixtureVenue
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class LiveTeams
    {
        public TeamDetail Home { get; set; }
        public TeamDetail Away { get; set; }
    }

    public class TeamDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool? Winner { get; set; }
    }

    public class LiveGoals
    {
        public int? Home { get; set; }
        public int? Away { get; set; }
    }

    public class LeagueDetail
    {
        public string Name { get; set; }
        public string Round { get; set; }
        public string Logo { get; set; }
    }

    // ===== HOTELS =====
    public class HotelResult
    {
        public string HotelId { get; set; }
        public string Name { get; set; }
        public double ReviewScore { get; set; }
        public string ReviewScoreWord { get; set; }
        public string PhotoUrl { get; set; }
        public string Url { get; set; }
        public double PrixParNuit { get; set; }
        public string Devise { get; set; }
    }

    // ===== RESTAURANT / CAFÉ =====
    public class RestaurantResult
    {
        public string FsqId { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Ville { get; set; }
        public double Note { get; set; }
        public string PhotoUrl { get; set; }
        public string Website { get; set; }
        public string Categorie { get; set; }

        public bool EstCafe => Categorie?.Contains("Café",
            StringComparison.OrdinalIgnoreCase) ?? false;
    }

    // ===== ÉQUIPE =====
    public class TeamInfo
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Logo { get; set; }
        public string Pays { get; set; }
        public string Stade { get; set; }
        public string Ville { get; set; }
        public int Capacite { get; set; }
    }

    // ===== FACE À FACE =====
    public class HeadToHeadResult
    {
        public int Victoires1 { get; set; }
        public int Victoires2 { get; set; }
        public int Nuls { get; set; }
        public List<H2HMatch> Matchs { get; set; }
    }

    public class H2HMatch
    {
        public string Date { get; set; }
        public string Equipe1 { get; set; }
        public string Equipe2 { get; set; }
        public string Logo1 { get; set; }
        public string Logo2 { get; set; }
        public string Score { get; set; }
        public string Competion { get; set; }
    }

    // ===== STATS ÉQUIPE =====
    public class TeamStats
    {
        public int MatchsJoues { get; set; }
        public int Victoires { get; set; }
        public int Defaites { get; set; }
        public int Nuls { get; set; }
        public int ButsMarques { get; set; }
        public int ButsEncaisses { get; set; }
    }

    // ===== TRANSPORT TRANSITLAND =====
    public class TransitRoute
    {
        public string Nom { get; set; }
        public string Type { get; set; }
        public string Compagnie { get; set; }
        public string Couleur { get; set; }
        public string Description { get; set; }

        public string Icone => Type switch
        {
            var t when t.Contains("Bus") => "🚌",
            var t when t.Contains("Train") => "🚆",
            var t when t.Contains("Métro") => "🚇",
            var t when t.Contains("Tram") => "🚋",
            var t when t.Contains("Ferry") => "⛴️",
            _ => "🚌"
        };
    }

    // ===== RECHERCHE ÉQUIPE (api-sports) =====
    public class TeamSearchResponse
    {
        [JsonProperty("response")]
        public List<TeamSearchItem> Response { get; set; }
    }

    public class TeamSearchItem
    {
        [JsonProperty("team")]
        public TeamSearchDetail Team { get; set; }

        [JsonProperty("venue")]
        public VenueDetail Venue { get; set; }
    }

    public class TeamSearchDetail
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }

    public class VenueDetail
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("capacity")]
        public int? Capacity { get; set; }
    }
}