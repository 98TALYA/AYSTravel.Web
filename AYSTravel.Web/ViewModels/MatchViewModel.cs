using System.ComponentModel.DataAnnotations;

namespace AYSTravel.Web.ViewModels
{
    public class MatchViewModel
    {
        // ===== DONNÉES BASE (existantes) =====
        public int Id { get; set; }
        public string Equipe1 { get; set; }
        public string Equipe2 { get; set; }
        public string Stade { get; set; }

        [Url]
        public string StadeImageUrl { get; set; }
        public string StadeNom { get; set; }
        public DateTime Date { get; set; }
        public string VilleNom { get; set; }

        // ===== LOGOS ET DRAPEAUX =====
        public string LogoEquipe1 { get; set; }
        public string LogoEquipe2 { get; set; }
        public string CodePays1 { get; set; }
        public string CodePays2 { get; set; }

        // ===== SCORE LIVE =====
        public int? ScoreEquipe1 { get; set; }
        public int? ScoreEquipe2 { get; set; }
        public string Statut { get; set; }       // "NS", "1H", "HT", "2H", "FT"
        public int? MinuteJouee { get; set; }    // minutes écoulées

        // ===== INFOS COMPÉTITION =====
        public string Round { get; set; }
        public string CompetitionNom { get; set; }
        public string CompetitionLogo { get; set; }

        // ===== PROPRIÉTÉS CALCULÉES =====
        public bool EstLive =>
            Statut == "1H" || Statut == "2H" || Statut == "HT" || Statut == "ET";

        public bool EstAVenir =>
            string.IsNullOrEmpty(Statut) || Statut == "NS";

        public bool EstTermine =>
            Statut == "FT" || Statut == "AET" || Statut == "PEN";

        public string ScoreAffichage =>
            EstAVenir
                ? "VS"
                : $"{ScoreEquipe1 ?? 0} - {ScoreEquipe2 ?? 0}";

        public long SecondesAvantMatch =>
            EstAVenir ? (long)(Date - DateTime.UtcNow).TotalSeconds : 0;
    }
}
