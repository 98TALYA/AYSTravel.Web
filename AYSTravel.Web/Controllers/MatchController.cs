using AYSTravel.Logic;
using AYSTravel.Web.Services;
using AYSTravel.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AYSTravel.Web.Controllers
{
    public class MatchController : Controller
    {
        private readonly MatchManager _manager;
        private readonly ApiService _apiService;

        public MatchController(MatchManager manager, ApiService apiService)
        {
            _manager = manager;
            _apiService = apiService;
        }

        // ===== TRADUCTION NOM ÉQUIPE FR → EN =====
        private string TraduireNomEquipe(string nom)
        {
            var traductions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Maroc",       "Morocco"      },
                { "Espagne",     "Spain"        },
                { "Portugal",    "Portugal"     },
                { "France",      "France"       },
                { "Brésil",      "Brazil"       },
                { "Argentine",   "Argentina"    },
                { "Allemagne",   "Germany"      },
                { "Italie",      "Italy"        },
                { "Angleterre",  "England"      },
                { "Belgique",    "Belgium"      },
                { "Pays-Bas",    "Netherlands"  },
                { "Croatie",     "Croatia"      },
                { "Sénégal",     "Senegal"      },
                { "Tunisie",     "Tunisia"      },
                { "Algérie",     "Algeria"      },
                { "Égypte",      "Egypt"        },
                { "Cameroun",    "Cameroon"     },
                { "Ghana",       "Ghana"        },
                { "Nigeria",     "Nigeria"      },
                { "États-Unis",  "USA"          },
                { "Mexique",     "Mexico"       },
                { "Japon",       "Japan"        },
                { "Corée",       "South Korea"  },
                { "Australie",   "Australia"    },
                { "Suisse",      "Switzerland"  },
                { "Suède",       "Sweden"       },
                { "Danemark",    "Denmark"      },
                { "Pologne",     "Poland"       },
                { "Serbie",      "Serbia"       },
                { "Ukraine",     "Ukraine"      },
                { "Turquie",     "Turkey"       },
                { "Colombie",    "Colombia"     },
                { "Uruguay",     "Uruguay"      },
                { "Chili",       "Chile"        },
                { "Pérou",       "Peru"         },
                { "Équateur",    "Ecuador"      },
                { "Qatar",       "Qatar"        },
            };

            return traductions.TryGetValue(nom.Trim(), out var traduction) ? traduction : nom;
        }
        public async Task<IActionResult> TestApi()
        {
            using var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Add("X-Auth-Token", "fe92155f303b4e37990f9561ed18073f");

            var sb = new System.Text.StringBuilder();

            // Test France (773)
            var r1 = await client.GetStringAsync("https://api.football-data.org/v4/teams/773");
            var d1 = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(r1);
            sb.AppendLine($"France → {d1?.name} | crest={d1?.crest}");

            // Test Brazil (764)
            var r2 = await client.GetStringAsync("https://api.football-data.org/v4/teams/764");
            var d2 = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(r2);
            sb.AppendLine($"Brazil → {d2?.name} | crest={d2?.crest}");

            // Test H2H France vs Brazil matchs
            var r3 = await client.GetStringAsync("https://api.football-data.org/v4/teams/773/matches?status=FINISHED&limit=30");
            var d3 = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(r3);
            sb.AppendLine($"Matchs France (30 derniers) : {d3?.resultSet?.count}");
            foreach (var m in d3?.matches ?? new Newtonsoft.Json.Linq.JArray())
            {
                sb.AppendLine($"  {m?.utcDate} | {m?.homeTeam?.name} vs {m?.awayTeam?.name}");
            }

            return Content(sb.ToString());
        }
        // ===== LISTE TOUS LES MATCHS =====
        public async Task<IActionResult> Index(int? villeId)
        {
            var query = _manager.GetAll().ToList();

            Console.WriteLine($"[DEBUG] Total matchs : {query.Count}");

            if (villeId.HasValue)
                query = query.Where(m => m.Stade?.VilleId == villeId.Value).ToList();

            var matchs = query.Select(m => new MatchViewModel
            {
                Id = m.Id,
                Equipe1 = m.Equipe1,
                Equipe2 = m.Equipe2,
                Date = m.Date,
                StadeImageUrl = m.Stade?.ImageUrl,
                StadeNom = m.Stade?.Nom ?? "Stade inconnu",
                VilleNom = m.Stade?.Ville?.Nom ?? "Ville inconnue",
                Statut = "NS"
            }).ToList();

            try
            {
                var liveMatches = await _apiService.GetLiveMatches();
                var upcomingMatches = await _apiService.GetUpcomingMatches();
                ViewBag.LiveMatches = liveMatches;
                ViewBag.UpcomingMatches = upcomingMatches;
                ViewBag.HasLive = liveMatches?.Any() ?? false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MATCH INDEX] Erreur API : {ex.Message}");
                ViewBag.LiveMatches = null;
                ViewBag.UpcomingMatches = null;
                ViewBag.HasLive = false;
            }

            return View(matchs);
        }

        // ===== DÉTAILS D'UN MATCH =====
        public async Task<IActionResult> Details(int id)
        {
            var matchEntity = _manager.GetById(id);
            if (matchEntity == null) return NotFound();

            var match = new MatchViewModel
            {
                Id = matchEntity.Id,
                Equipe1 = matchEntity.Equipe1,
                Equipe2 = matchEntity.Equipe2,
                Date = matchEntity.Date,
                StadeImageUrl = matchEntity.Stade?.ImageUrl,
                StadeNom = matchEntity.Stade?.Nom ?? "Stade inconnu",
                VilleNom = matchEntity.Stade?.Ville?.Nom ?? "Ville inconnue",
                Statut = "NS"
            };

            // Valeurs par défaut ViewBag
            ViewBag.TeamInfo1 = null;
            ViewBag.TeamInfo2 = null;
            ViewBag.HeadToHead = null;
            ViewBag.StatsEquipe1 = null;
            ViewBag.StatsEquipe2 = null;

            try
            {
                var nom1 = TraduireNomEquipe(matchEntity.Equipe1);
                var nom2 = TraduireNomEquipe(matchEntity.Equipe2);

                Console.WriteLine($"[API] ===== DÉBUT DETAILS pour '{nom1}' vs '{nom2}' =====");

                // ── IDs équipes (en parallèle) ──────────────────────────────
                var t1Task = _apiService.GetTeamId(nom1);
                var t2Task = _apiService.GetTeamId(nom2);
                await Task.WhenAll(t1Task, t2Task);

                var teamId1 = t1Task.Result;
                var teamId2 = t2Task.Result;

                Console.WriteLine($"[API] teamId1={teamId1?.ToString() ?? "NULL"} | teamId2={teamId2?.ToString() ?? "NULL"}");

                if (teamId1.HasValue && teamId2.HasValue)
                {
                    // ── Infos équipes, H2H, Stats (en parallèle) ────────────
                    var infoTask1 = _apiService.GetTeamInfo(teamId1.Value);
                    var infoTask2 = _apiService.GetTeamInfo(teamId2.Value);
                    var h2hTask = _apiService.GetHeadToHead(teamId1.Value, teamId2.Value);
                    var stats1Task = _apiService.GetTeamStats(teamId1.Value);
                    var stats2Task = _apiService.GetTeamStats(teamId2.Value);

                    await Task.WhenAll(infoTask1, infoTask2, h2hTask, stats1Task, stats2Task);

                    ViewBag.TeamInfo1 = infoTask1.Result;
                    ViewBag.TeamInfo2 = infoTask2.Result;
                    ViewBag.HeadToHead = h2hTask.Result;
                    ViewBag.StatsEquipe1 = stats1Task.Result;
                    ViewBag.StatsEquipe2 = stats2Task.Result;

                    Console.WriteLine($"[API] TeamInfo1 = {(ViewBag.TeamInfo1 != null ? ViewBag.TeamInfo1.Nom : "NULL")}");
                    Console.WriteLine($"[API] TeamInfo2 = {(ViewBag.TeamInfo2 != null ? ViewBag.TeamInfo2.Nom : "NULL")}");
                    Console.WriteLine($"[API] H2H matchs = {(ViewBag.HeadToHead?.Matchs != null ? ViewBag.HeadToHead.Matchs.Count.ToString() : "NULL")}");
                    Console.WriteLine($"[API] Stats1 joués={ViewBag.StatsEquipe1?.MatchsJoues} V={ViewBag.StatsEquipe1?.Victoires} B={ViewBag.StatsEquipe1?.ButsMarques}");
                    Console.WriteLine($"[API] Stats2 joués={ViewBag.StatsEquipe2?.MatchsJoues} V={ViewBag.StatsEquipe2?.Victoires} B={ViewBag.StatsEquipe2?.ButsMarques}");
                }
                else
                {
                    Console.WriteLine($"[API] ⚠️ ID(s) non trouvé(s) — H2H et Stats ignorés");
                }

                // ── Match live ──────────────────────────────────────────────
                var liveMatches = await _apiService.GetLiveMatches();
                Console.WriteLine($"[API] Matchs live trouvés : {liveMatches?.Count ?? 0}");

                var liveMatch = liveMatches?.FirstOrDefault(l =>
                    l.Teams?.Home?.Name?.Contains(nom1, StringComparison.OrdinalIgnoreCase) == true ||
                    l.Teams?.Away?.Name?.Contains(nom1, StringComparison.OrdinalIgnoreCase) == true ||
                    l.Teams?.Home?.Name?.Contains(nom2, StringComparison.OrdinalIgnoreCase) == true ||
                    l.Teams?.Away?.Name?.Contains(nom2, StringComparison.OrdinalIgnoreCase) == true);

                if (liveMatch != null)
                {
                    Console.WriteLine($"[API] Match LIVE trouvé : {liveMatch.Teams?.Home?.Name} vs {liveMatch.Teams?.Away?.Name}");
                    match.ScoreEquipe1 = liveMatch.Goals?.Home;
                    match.ScoreEquipe2 = liveMatch.Goals?.Away;
                    match.Statut = liveMatch.Fixture?.Status?.Short ?? "NS";
                    match.MinuteJouee = liveMatch.Fixture?.Status?.Elapsed;
                    match.LogoEquipe1 = liveMatch.Teams?.Home?.Logo;
                    match.LogoEquipe2 = liveMatch.Teams?.Away?.Logo;
                    match.Round = liveMatch.League?.Round;
                    match.CompetitionNom = liveMatch.League?.Name;
                    match.CompetitionLogo = liveMatch.League?.Logo;
                }

                Console.WriteLine($"[API] ===== FIN DETAILS =====");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MATCH DETAILS] ❌ Erreur : {ex.Message}");
                Console.WriteLine($"[MATCH DETAILS] Stack : {ex.StackTrace}");
            }

            return View(match);
        }
    }
}