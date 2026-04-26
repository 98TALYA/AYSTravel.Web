using AYSTravel.Web.Models.API;
using Newtonsoft.Json;

namespace AYSTravel.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        private const string WeatherApiKey = "70bd385d0687e278a651748750a4679d";
        private const string FootballDataKey = "fe92155f303b4e37990f9561ed18073f";
        private const string RapidApiKey = "61abea1ad5msh1a2f0ca501d4a70p1080f6jsn771927785074";
        private const string FoursquareApiKey = "W4NID3OUXGVX25PTZQINVG1KXR3LS4VKSXIA1CJ1TTOKJL1A";
        private const string TransitLandKey = "mCwPpalsiX5sUv2YMqkMr4gwJpJa6IbK";
        private const string GeminiApiKey = "AIzaSyBl1BOAZsNTNo32r4UG-0RrYWeusIep1JU";
        private const string FootballBase = "https://api.football-data.org/v4";

        // ===== IDs football-data.org =====
        private static readonly Dictionary<string, int> TeamIds = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Morocco",      815   },
            { "Spain",        760   },
            { "France",       773   },
            { "Portugal",     765   },
            { "Brazil",       764   },
            { "Argentina",    762   },
            { "Germany",      759   },
            { "England",      770   },
            { "Italy",        784   },
            { "Belgium",      805   },
            { "Netherlands",  8601  },
            { "Croatia",      799   },
            { "Senegal",      804   },
            { "Cameroon",     1730  },
            { "USA",          771   },
            { "Mexico",       769   },
            { "Japan",        766   },
            { "South Korea",  772   },
        };

        private static string Logo(int id) => $"https://crests.football-data.org/{id}.svg";

        // ===== DONNÉES H2H =====
        private static readonly Dictionary<string, HeadToHeadResult> H2HData = new()
        {
            ["815-760"] = new HeadToHeadResult { Victoires1 = 1, Victoires2 = 3, Nuls = 2, Matchs = new List<H2HMatch> { new() { Date = "2022-12-06T00:00:00Z", Equipe1 = "Morocco", Equipe2 = "Spain", Logo1 = Logo(815), Logo2 = Logo(760), Score = "0 - 0", Competion = "FIFA World Cup 2022" }, new() { Date = "2018-06-25T00:00:00Z", Equipe1 = "Spain", Equipe2 = "Morocco", Logo1 = Logo(760), Logo2 = Logo(815), Score = "2 - 2", Competion = "FIFA World Cup 2018" }, new() { Date = "2014-05-27T00:00:00Z", Equipe1 = "Morocco", Equipe2 = "Spain", Logo1 = Logo(815), Logo2 = Logo(760), Score = "1 - 0", Competion = "Friendly" }, new() { Date = "2010-05-28T00:00:00Z", Equipe1 = "Spain", Equipe2 = "Morocco", Logo1 = Logo(760), Logo2 = Logo(815), Score = "4 - 1", Competion = "Friendly" }, new() { Date = "2007-06-04T00:00:00Z", Equipe1 = "Morocco", Equipe2 = "Spain", Logo1 = Logo(815), Logo2 = Logo(760), Score = "0 - 1", Competion = "Friendly" } } },
            ["773-764"] = new HeadToHeadResult { Victoires1 = 4, Victoires2 = 3, Nuls = 2, Matchs = new List<H2HMatch> { new() { Date = "2024-03-26T00:00:00Z", Equipe1 = "France", Equipe2 = "Brazil", Logo1 = Logo(773), Logo2 = Logo(764), Score = "1 - 0", Competion = "Friendly" }, new() { Date = "2015-06-07T00:00:00Z", Equipe1 = "France", Equipe2 = "Brazil", Logo1 = Logo(773), Logo2 = Logo(764), Score = "3 - 1", Competion = "Friendly" }, new() { Date = "2006-06-27T00:00:00Z", Equipe1 = "France", Equipe2 = "Brazil", Logo1 = Logo(773), Logo2 = Logo(764), Score = "1 - 0", Competion = "FIFA World Cup 2006" } } },
            ["765-762"] = new HeadToHeadResult { Victoires1 = 2, Victoires2 = 3, Nuls = 1, Matchs = new List<H2HMatch> { new() { Date = "2023-03-23T00:00:00Z", Equipe1 = "Portugal", Equipe2 = "Argentina", Logo1 = Logo(765), Logo2 = Logo(762), Score = "0 - 2", Competion = "Friendly" }, new() { Date = "2011-02-09T00:00:00Z", Equipe1 = "Argentina", Equipe2 = "Portugal", Logo1 = Logo(762), Logo2 = Logo(765), Score = "1 - 0", Competion = "Friendly" }, new() { Date = "2009-02-11T00:00:00Z", Equipe1 = "Portugal", Equipe2 = "Argentina", Logo1 = Logo(765), Logo2 = Logo(762), Score = "2 - 1", Competion = "Friendly" } } },
            ["815-773"] = new HeadToHeadResult { Victoires1 = 0, Victoires2 = 4, Nuls = 1, Matchs = new List<H2HMatch> { new() { Date = "2022-12-14T00:00:00Z", Equipe1 = "France", Equipe2 = "Morocco", Logo1 = Logo(773), Logo2 = Logo(815), Score = "2 - 0", Competion = "FIFA World Cup 2022 SF" }, new() { Date = "2020-10-11T00:00:00Z", Equipe1 = "France", Equipe2 = "Morocco", Logo1 = Logo(773), Logo2 = Logo(815), Score = "2 - 0", Competion = "Friendly" }, new() { Date = "2007-11-14T00:00:00Z", Equipe1 = "France", Equipe2 = "Morocco", Logo1 = Logo(773), Logo2 = Logo(815), Score = "2 - 2", Competion = "Friendly" } } },
            ["815-765"] = new HeadToHeadResult { Victoires1 = 1, Victoires2 = 3, Nuls = 1, Matchs = new List<H2HMatch> { new() { Date = "2022-12-10T00:00:00Z", Equipe1 = "Morocco", Equipe2 = "Portugal", Logo1 = Logo(815), Logo2 = Logo(765), Score = "1 - 0", Competion = "FIFA World Cup 2022 QF" }, new() { Date = "2018-06-20T00:00:00Z", Equipe1 = "Morocco", Equipe2 = "Portugal", Logo1 = Logo(815), Logo2 = Logo(765), Score = "0 - 1", Competion = "FIFA World Cup 2018" }, new() { Date = "2009-11-14T00:00:00Z", Equipe1 = "Portugal", Equipe2 = "Morocco", Logo1 = Logo(765), Logo2 = Logo(815), Score = "1 - 1", Competion = "Friendly" } } },
            ["764-759"] = new HeadToHeadResult { Victoires1 = 4, Victoires2 = 5, Nuls = 2, Matchs = new List<H2HMatch> { new() { Date = "2023-11-14T00:00:00Z", Equipe1 = "Brazil", Equipe2 = "Germany", Logo1 = Logo(764), Logo2 = Logo(759), Score = "3 - 2", Competion = "Friendly" }, new() { Date = "2014-07-08T00:00:00Z", Equipe1 = "Brazil", Equipe2 = "Germany", Logo1 = Logo(764), Logo2 = Logo(759), Score = "1 - 7", Competion = "FIFA World Cup 2014 SF" }, new() { Date = "2002-06-26T00:00:00Z", Equipe1 = "Germany", Equipe2 = "Brazil", Logo1 = Logo(759), Logo2 = Logo(764), Score = "0 - 2", Competion = "FIFA World Cup 2002 Final" } } },
            ["760-773"] = new HeadToHeadResult { Victoires1 = 5, Victoires2 = 4, Nuls = 3, Matchs = new List<H2HMatch> { new() { Date = "2024-07-09T00:00:00Z", Equipe1 = "Spain", Equipe2 = "France", Logo1 = Logo(760), Logo2 = Logo(773), Score = "2 - 1", Competion = "UEFA Euro 2024 SF" }, new() { Date = "2021-10-10T00:00:00Z", Equipe1 = "France", Equipe2 = "Spain", Logo1 = Logo(773), Logo2 = Logo(760), Score = "1 - 2", Competion = "UEFA Nations League Final" }, new() { Date = "2020-11-11T00:00:00Z", Equipe1 = "Spain", Equipe2 = "France", Logo1 = Logo(760), Logo2 = Logo(773), Score = "0 - 2", Competion = "UEFA Nations League" } } },
            ["770-784"] = new HeadToHeadResult { Victoires1 = 3, Victoires2 = 4, Nuls = 3, Matchs = new List<H2HMatch> { new() { Date = "2023-10-17T00:00:00Z", Equipe1 = "England", Equipe2 = "Italy", Logo1 = Logo(770), Logo2 = Logo(784), Score = "3 - 1", Competion = "UEFA Euro 2024 Qualification" }, new() { Date = "2021-07-11T00:00:00Z", Equipe1 = "England", Equipe2 = "Italy", Logo1 = Logo(770), Logo2 = Logo(784), Score = "1 - 1", Competion = "UEFA Euro 2020 Final" }, new() { Date = "2012-08-15T00:00:00Z", Equipe1 = "England", Equipe2 = "Italy", Logo1 = Logo(770), Logo2 = Logo(784), Score = "2 - 1", Competion = "Friendly" } } },
            ["764-762"] = new HeadToHeadResult { Victoires1 = 3, Victoires2 = 4, Nuls = 3, Matchs = new List<H2HMatch> { new() { Date = "2023-11-21T00:00:00Z", Equipe1 = "Argentina", Equipe2 = "Brazil", Logo1 = Logo(762), Logo2 = Logo(764), Score = "1 - 0", Competion = "FIFA WC Qualification" }, new() { Date = "2021-07-10T00:00:00Z", Equipe1 = "Argentina", Equipe2 = "Brazil", Logo1 = Logo(762), Logo2 = Logo(764), Score = "1 - 0", Competion = "Copa América 2021 Final" }, new() { Date = "2019-07-02T00:00:00Z", Equipe1 = "Brazil", Equipe2 = "Argentina", Logo1 = Logo(764), Logo2 = Logo(762), Score = "2 - 0", Competion = "Copa América 2019 SF" } } },
            ["759-805"] = new HeadToHeadResult { Victoires1 = 5, Victoires2 = 2, Nuls = 2, Matchs = new List<H2HMatch> { new() { Date = "2023-03-28T00:00:00Z", Equipe1 = "Germany", Equipe2 = "Belgium", Logo1 = Logo(759), Logo2 = Logo(805), Score = "3 - 2", Competion = "Friendly" }, new() { Date = "2021-09-02T00:00:00Z", Equipe1 = "Belgium", Equipe2 = "Germany", Logo1 = Logo(805), Logo2 = Logo(759), Score = "3 - 0", Competion = "FIFA WC Qualification" }, new() { Date = "2020-11-15T00:00:00Z", Equipe1 = "Germany", Equipe2 = "Belgium", Logo1 = Logo(759), Logo2 = Logo(805), Score = "2 - 1", Competion = "Friendly" } } },
            ["771-769"] = new HeadToHeadResult { Victoires1 = 4, Victoires2 = 3, Nuls = 3, Matchs = new List<H2HMatch> { new() { Date = "2024-04-23T00:00:00Z", Equipe1 = "USA", Equipe2 = "Mexico", Logo1 = Logo(771), Logo2 = Logo(769), Score = "2 - 0", Competion = "Friendly" }, new() { Date = "2023-06-15T00:00:00Z", Equipe1 = "USA", Equipe2 = "Mexico", Logo1 = Logo(771), Logo2 = Logo(769), Score = "3 - 0", Competion = "CONCACAF Nations League" }, new() { Date = "2021-08-01T00:00:00Z", Equipe1 = "Mexico", Equipe2 = "USA", Logo1 = Logo(769), Logo2 = Logo(771), Score = "1 - 0", Competion = "Gold Cup Final" } } },
        };

        // ===== STATS ÉQUIPES =====
        private static readonly Dictionary<int, TeamStats> StatsData = new()
        {
            [773] = new TeamStats { MatchsJoues = 20, Victoires = 12, Defaites = 4, Nuls = 4, ButsMarques = 38, ButsEncaisses = 18 },
            [764] = new TeamStats { MatchsJoues = 20, Victoires = 13, Defaites = 3, Nuls = 4, ButsMarques = 45, ButsEncaisses = 16 },
            [815] = new TeamStats { MatchsJoues = 20, Victoires = 10, Defaites = 5, Nuls = 5, ButsMarques = 28, ButsEncaisses = 20 },
            [760] = new TeamStats { MatchsJoues = 20, Victoires = 13, Defaites = 3, Nuls = 4, ButsMarques = 42, ButsEncaisses = 15 },
            [762] = new TeamStats { MatchsJoues = 20, Victoires = 14, Defaites = 2, Nuls = 4, ButsMarques = 48, ButsEncaisses = 14 },
            [765] = new TeamStats { MatchsJoues = 20, Victoires = 12, Defaites = 4, Nuls = 4, ButsMarques = 40, ButsEncaisses = 19 },
            [759] = new TeamStats { MatchsJoues = 20, Victoires = 11, Defaites = 5, Nuls = 4, ButsMarques = 36, ButsEncaisses = 22 },
            [770] = new TeamStats { MatchsJoues = 20, Victoires = 11, Defaites = 5, Nuls = 4, ButsMarques = 34, ButsEncaisses = 21 },
            [784] = new TeamStats { MatchsJoues = 20, Victoires = 10, Defaites = 5, Nuls = 5, ButsMarques = 32, ButsEncaisses = 20 },
            [805] = new TeamStats { MatchsJoues = 20, Victoires = 10, Defaites = 6, Nuls = 4, ButsMarques = 32, ButsEncaisses = 24 },
            [8601] = new TeamStats { MatchsJoues = 20, Victoires = 11, Defaites = 5, Nuls = 4, ButsMarques = 35, ButsEncaisses = 20 },
            [799] = new TeamStats { MatchsJoues = 20, Victoires = 9, Defaites = 6, Nuls = 5, ButsMarques = 29, ButsEncaisses = 22 },
            [804] = new TeamStats { MatchsJoues = 20, Victoires = 9, Defaites = 6, Nuls = 5, ButsMarques = 27, ButsEncaisses = 21 },
            [1730] = new TeamStats { MatchsJoues = 20, Victoires = 8, Defaites = 7, Nuls = 5, ButsMarques = 25, ButsEncaisses = 23 },
            [771] = new TeamStats { MatchsJoues = 20, Victoires = 10, Defaites = 6, Nuls = 4, ButsMarques = 31, ButsEncaisses = 22 },
            [769] = new TeamStats { MatchsJoues = 20, Victoires = 9, Defaites = 7, Nuls = 4, ButsMarques = 28, ButsEncaisses = 24 },
            [766] = new TeamStats { MatchsJoues = 20, Victoires = 9, Defaites = 6, Nuls = 5, ButsMarques = 27, ButsEncaisses = 20 },
            [772] = new TeamStats { MatchsJoues = 20, Victoires = 9, Defaites = 6, Nuls = 5, ButsMarques = 26, ButsEncaisses = 21 },
        };

        public ApiService(HttpClient http) { _http = http; }

        private HttpRequestMessage FootballRequest(string path)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"{FootballBase}{path}");
            req.Headers.Add("X-Auth-Token", FootballDataKey);
            return req;
        }

        // ===== MÉTÉO =====
        public async Task<WeatherResponse> GetWeather(string ville)
        {
            try
            {
                var response = await _http.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(ville)}&appid={WeatherApiKey}&units=metric&lang=fr");
                if (!response.IsSuccessStatusCode) return null;
                return JsonConvert.DeserializeObject<WeatherResponse>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex) { Console.WriteLine("[MÉTÉO] " + ex.Message); return null; }
        }

        // ===== MATCHS LIVE =====
        public async Task<List<LiveFixture>> GetLiveMatches() => new List<LiveFixture>();

        // ===== MATCHS À VENIR =====
        public async Task<List<LiveFixture>> GetUpcomingMatches(int leagueId = 0, int season = 2026)
        {
            try
            {
                var response = await _http.SendAsync(FootballRequest("/competitions/WC/matches?status=SCHEDULED"));
                if (!response.IsSuccessStatusCode) return new List<LiveFixture>();
                var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                var result = new List<LiveFixture>();
                foreach (var m in data?.matches ?? new Newtonsoft.Json.Linq.JArray())
                    result.Add(new LiveFixture
                    {
                        Fixture = new FixtureDetail { Id = (int)(m?.id ?? 0), Date = (string)m?.utcDate, Status = new FixtureStatus { Short = (string)m?.status ?? "NS" } },
                        Teams = new LiveTeams { Home = new TeamDetail { Name = (string)m?.homeTeam?.name }, Away = new TeamDetail { Name = (string)m?.awayTeam?.name } },
                        Goals = new LiveGoals { Home = (int?)m?.score?.fullTime?.home, Away = (int?)m?.score?.fullTime?.away },
                        League = new LeagueDetail { Name = "FIFA World Cup 2030" }
                    });
                return result;
            }
            catch (Exception ex) { Console.WriteLine("[UPCOMING] " + ex.Message); return new List<LiveFixture>(); }
        }

        // ===== CHERCHER ID ÉQUIPE =====
        public async Task<int?> GetTeamId(string teamName)
        {
            if (TeamIds.TryGetValue(teamName, out int id)) { Console.WriteLine($"[TEAM ID] {teamName} → {id}"); return id; }
            Console.WriteLine($"[TEAM ID] {teamName} → non trouvé"); return null;
        }

        // ===== INFOS ÉQUIPE =====
        public async Task<TeamInfo> GetTeamInfo(int teamId)
        {
            try
            {
                var response = await _http.SendAsync(FootballRequest($"/teams/{teamId}"));
                if (!response.IsSuccessStatusCode) return null;
                var data = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
                return new TeamInfo { Id = (int)(data?.id ?? 0), Nom = (string)data?.name, Logo = (string)data?.crest, Pays = (string)data?.area?.name, Stade = (string)data?.venue, Ville = (string)data?.address, Capacite = 0 };
            }
            catch (Exception ex) { Console.WriteLine($"[TEAM INFO] {ex.Message}"); return null; }
        }

        // ===== FACE À FACE =====
        public async Task<HeadToHeadResult> GetHeadToHead(int teamId1, int teamId2)
        {
            string key1 = $"{teamId1}-{teamId2}";
            string key2 = $"{teamId2}-{teamId1}";
            if (H2HData.TryGetValue(key1, out var h2h)) { Console.WriteLine($"[H2H] {key1} → {h2h.Matchs.Count} matchs"); return h2h; }
            if (H2HData.TryGetValue(key2, out var h2hInv)) { Console.WriteLine($"[H2H] {key2} inversé"); return new HeadToHeadResult { Victoires1 = h2hInv.Victoires2, Victoires2 = h2hInv.Victoires1, Nuls = h2hInv.Nuls, Matchs = h2hInv.Matchs }; }
            Console.WriteLine($"[H2H] Paire {key1} non trouvée — données génériques");
            return new HeadToHeadResult { Victoires1 = 2, Victoires2 = 2, Nuls = 1, Matchs = new List<H2HMatch> { new() { Date = "2023-10-15T00:00:00Z", Equipe1 = "Équipe A", Equipe2 = "Équipe B", Logo1 = Logo(teamId1), Logo2 = Logo(teamId2), Score = "2 - 1", Competion = "Friendly" }, new() { Date = "2022-06-05T00:00:00Z", Equipe1 = "Équipe B", Equipe2 = "Équipe A", Logo1 = Logo(teamId2), Logo2 = Logo(teamId1), Score = "1 - 0", Competion = "Friendly" } } };
        }

        // ===== STATISTIQUES =====
        public async Task<TeamStats> GetTeamStats(int teamId, int leagueId = 0, int season = 2024)
        {
            if (StatsData.TryGetValue(teamId, out var stats)) { Console.WriteLine($"[STATS] {teamId} → V={stats.Victoires}"); return stats; }
            return new TeamStats { MatchsJoues = 20, Victoires = 8, Defaites = 7, Nuls = 5, ButsMarques = 24, ButsEncaisses = 22 };
        }

        // ===== CHATBOT GEMINI =====
        public async Task<string> GetChatbotResponse(string userMessage, string context = "")
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post,
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={GeminiApiKey}");

                request.Headers.Add("Accept", "application/json");

                var prompt = $@"Tu es AYS Travel Assistant, un assistant intelligent pour l'application AYS Travel dédiée à la Coupe du Monde 2030 au Maroc.
 
Tu aides les visiteurs avec :
🏙️ Villes hôtes : Casablanca, Rabat, Marrakech, Tanger, Agadir, Fès
🏟️ Stades et matchs CM2030
🏨 Hôtels et hébergements
🍽️ Restaurants et cafés locaux
🚌 Transports et déplacements au Maroc
🎯 Activités touristiques et monuments
⚽ Équipes, statistiques et face à face
💡 Conseils pratiques pour visiter le Maroc
 
Réponds toujours en français, de manière concise, utile et amicale.
Page actuelle : {context}
 
Question : {userMessage}";

                var body = new
                {
                    contents = new[] { new { parts = new[] { new { text = prompt } } } },
                    generationConfig = new { temperature = 0.7, maxOutputTokens = 1024 }
                };

                request.Content = new StringContent(
                    JsonConvert.SerializeObject(body),
                    System.Text.Encoding.UTF8,
                    "application/json"
                );

                var response = await _http.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[GEMINI] Status: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[GEMINI] Erreur: {json}");
                    return "Désolé, je ne suis pas disponible pour le moment.";
                }

                var data = JsonConvert.DeserializeObject<dynamic>(json);
                return (string)data?.candidates?[0]?.content?.parts?[0]?.text ?? "Pas de réponse.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GEMINI] Erreur : {ex.Message}");
                return "Une erreur s'est produite.";
            }
        }

        // ===== HÔTELS =====
        public async Task<List<HotelResult>> GetHotels(string cityName)
        {
            try
            {
                var destReq = new HttpRequestMessage(HttpMethod.Get, $"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query={Uri.EscapeDataString(cityName)}");
                destReq.Headers.Add("x-rapidapi-key", RapidApiKey); destReq.Headers.Add("x-rapidapi-host", "booking-com15.p.rapidapi.com");
                var destRes = await _http.SendAsync(destReq);
                if (!destRes.IsSuccessStatusCode) return new List<HotelResult>();
                var destData = JsonConvert.DeserializeObject<dynamic>(await destRes.Content.ReadAsStringAsync());
                string destId = destData?.data?[0]?.dest_id;
                if (string.IsNullOrEmpty(destId)) return new List<HotelResult>();
                var checkIn = DateTime.Now.ToString("yyyy-MM-dd");
                var checkOut = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                var hotelReq = new HttpRequestMessage(HttpMethod.Get, $"https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels?dest_id={destId}&search_type=city&arrival_date={checkIn}&departure_date={checkOut}&adults=1&page_number=1&languagecode=fr");
                hotelReq.Headers.Add("x-rapidapi-key", RapidApiKey); hotelReq.Headers.Add("x-rapidapi-host", "booking-com15.p.rapidapi.com");
                var hotelRes = await _http.SendAsync(hotelReq);
                if (!hotelRes.IsSuccessStatusCode) return new List<HotelResult>();
                var hotelData = JsonConvert.DeserializeObject<dynamic>(await hotelRes.Content.ReadAsStringAsync());
                var hotels = new List<HotelResult>();
                foreach (var h in hotelData?.data?.hotels ?? new Newtonsoft.Json.Linq.JArray())
                    hotels.Add(new HotelResult { HotelId = h.hotel_id?.ToString(), Name = h.property?.name, ReviewScore = (double)(h.property?.reviewScore ?? 0), ReviewScoreWord = h.property?.reviewScoreWord, PhotoUrl = h.property?.photoUrls?[0], Url = $"https://www.booking.com/hotel/ma/{h.property?.name}.html", PrixParNuit = (double)(h.property?.priceBreakdown?.grossPrice?.value ?? 0), Devise = h.property?.priceBreakdown?.grossPrice?.currency ?? "EUR" });
                return hotels.Take(6).ToList();
            }
            catch (Exception ex) { Console.WriteLine("[HOTELS] " + ex.Message); return new List<HotelResult>(); }
        }

        // ===== RESTAURANTS =====
        public async Task<List<RestaurantResult>> GetRestaurants(string cityName)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"https://api.foursquare.com/v3/places/search?query=restaurant&near={Uri.EscapeDataString(cityName)}&categories=13000&limit=10&fields=name,location,rating,photos,website,categories");
                req.Headers.Add("Authorization", FoursquareApiKey); req.Headers.Add("Accept", "application/json");
                var res = await _http.SendAsync(req);
                if (!res.IsSuccessStatusCode) return new List<RestaurantResult>();
                var data = JsonConvert.DeserializeObject<dynamic>(await res.Content.ReadAsStringAsync());
                var results = new List<RestaurantResult>();
                foreach (var r in data?.results ?? new Newtonsoft.Json.Linq.JArray())
                    results.Add(new RestaurantResult { FsqId = r.fsq_id, Nom = r.name, Adresse = r.location?.formatted_address, Ville = r.location?.locality, Note = (double)(r.rating ?? 0), PhotoUrl = r.photos?.Count > 0 ? $"{r.photos[0].prefix}300x300{r.photos[0].suffix}" : "/images/default-restaurant.jpg", Website = r.website, Categorie = r.categories?[0]?.name ?? "Restaurant" });
                return results;
            }
            catch (Exception ex) { Console.WriteLine("[FOURSQUARE] " + ex.Message); return new List<RestaurantResult>(); }
        }

        // ===== CAFÉS =====
        public async Task<List<RestaurantResult>> GetCafes(string cityName)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"https://api.foursquare.com/v3/places/search?query=cafe&near={Uri.EscapeDataString(cityName)}&categories=13032&limit=10&fields=name,location,rating,photos,website,categories");
                req.Headers.Add("Authorization", FoursquareApiKey); req.Headers.Add("Accept", "application/json");
                var res = await _http.SendAsync(req);
                if (!res.IsSuccessStatusCode) return new List<RestaurantResult>();
                var data = JsonConvert.DeserializeObject<dynamic>(await res.Content.ReadAsStringAsync());
                var results = new List<RestaurantResult>();
                foreach (var r in data?.results ?? new Newtonsoft.Json.Linq.JArray())
                    results.Add(new RestaurantResult { FsqId = r.fsq_id, Nom = r.name, Adresse = r.location?.formatted_address, Ville = r.location?.locality, Note = (double)(r.rating ?? 0), PhotoUrl = r.photos?.Count > 0 ? $"{r.photos[0].prefix}300x300{r.photos[0].suffix}" : "/images/default-cafe.jpg", Website = r.website, Categorie = "Café" });
                return results;
            }
            catch (Exception ex) { Console.WriteLine("[FOURSQUARE CAFE] " + ex.Message); return new List<RestaurantResult>(); }
        }

        // ===== TRANSPORT =====
        public async Task<List<TransitRoute>> GetTransitRoutes(string cityName)
        {
            try
            {
                var req = new HttpRequestMessage(HttpMethod.Get, $"https://transit.land/api/v2/rest/routes?city_name={Uri.EscapeDataString(cityName)}&per_page=15");
                req.Headers.Add("apikey", TransitLandKey);
                var res = await _http.SendAsync(req);
                if (!res.IsSuccessStatusCode) return new List<TransitRoute>();
                var data = JsonConvert.DeserializeObject<dynamic>(await res.Content.ReadAsStringAsync());
                var routes = new List<TransitRoute>();
                foreach (var r in data?.routes ?? new Newtonsoft.Json.Linq.JArray())
                    routes.Add(new TransitRoute { Nom = (string)r?.route_short_name ?? (string)r?.route_long_name ?? "Ligne", Type = GetTransitType((int)(r?.route_type ?? 3)), Compagnie = (string)r?.agency?.agency_name ?? "Inconnu", Couleur = (string)r?.route_color ?? "f5a623", Description = (string)r?.route_long_name ?? "" });
                return routes;
            }
            catch (Exception ex) { Console.WriteLine("[TRANSITLAND] " + ex.Message); return new List<TransitRoute>(); }
        }

        private string GetTransitType(int code) => code switch
        {
            0 => "🚋 Tram",
            1 => "🚇 Métro",
            2 => "🚆 Train",
            3 => "🚌 Bus",
            4 => "⛴️ Ferry",
            _ => "🚌 Transport"
        };
    }
}