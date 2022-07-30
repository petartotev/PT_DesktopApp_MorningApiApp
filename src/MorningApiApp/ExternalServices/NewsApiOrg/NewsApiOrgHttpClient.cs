using System;
using System.Collections.Generic;
using MorningApiApp.ExternalServices.NewsApiOrg.Enums;

namespace MorningApiApp.ExternalServices.NewsApiOrg
{
    internal class NewsApiOrgHttpClient : WpfHttpClient
    {
        private readonly Dictionary<string, string> _newsSourcesCollection = new Dictionary<string,string>();

        public NewsApiOrgHttpClient()
        {
            GenerateNewsSources();
        }

        internal string ComposeUrlByInput(string endpoint, DateTime? from, DateTime? to, string sortBy, string source, string input = null, string category = null, string country = null)
        {
            string url = NewsApiOrgConstants.UrlBase;

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentNullException("Endpoint cannot be null.");
            }
            else if ((NewsEndpointEnum)Enum.Parse(typeof(NewsEndpointEnum), endpoint) == NewsEndpointEnum.Everything)
            {
                url += NewsApiOrgConstants.QueryEverything;
                url += string.Format(NewsApiOrgConstants.Keyword, !string.IsNullOrWhiteSpace(input) ? input.ToString() : "Raspberry");

                if (!string.IsNullOrWhiteSpace(source))
                {
                    NewsSourceEnum sourceEnum = (NewsSourceEnum)Enum.Parse(typeof(NewsSourceEnum), source.ToString());
                    url += string.Format(NewsApiOrgConstants.SourcesAnd, _newsSourcesCollection[sourceEnum.ToString()]);
                }
            }
            else if ((NewsEndpointEnum)Enum.Parse(typeof(NewsEndpointEnum), endpoint.ToString()) == NewsEndpointEnum.TopHeadlines)
            {
                url += NewsApiOrgConstants.QueryTopHeadlines;

                if (!string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(country))
                {
                    NewsCategoryEnum categoryEnum = (NewsCategoryEnum)Enum.Parse(typeof(NewsCategoryEnum), category.ToString());
                    url += string.Format(NewsApiOrgConstants.Category, categoryEnum.ToString());

                    NewsCountryEnum countryEnum = (NewsCountryEnum)Enum.Parse(typeof(NewsCountryEnum), country.ToString());
                    url += string.Format(NewsApiOrgConstants.CountryAnd, countryEnum.ToString());
                }
                else if (string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(country))
                {
                    NewsCountryEnum countryEnum = (NewsCountryEnum)Enum.Parse(typeof(NewsCountryEnum), country.ToString());
                    url += string.Format(NewsApiOrgConstants.Country, countryEnum.ToString());
                }
                else if (!string.IsNullOrWhiteSpace(category) && string.IsNullOrWhiteSpace(country))
                {
                    NewsCategoryEnum categoryEnum = (NewsCategoryEnum)Enum.Parse(typeof(NewsCategoryEnum), category.ToString());
                    url += string.Format(NewsApiOrgConstants.Category, categoryEnum.ToString());
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(source))
                    {
                        NewsSourceEnum sourceEnum = (NewsSourceEnum)Enum.Parse(typeof(NewsSourceEnum), source.ToString());
                        url += string.Format(NewsApiOrgConstants.Sources, _newsSourcesCollection[sourceEnum.ToString()]);
                    }
                    else
                    {
                        url += string.Format(NewsApiOrgConstants.Country, NewsCountryEnum.bg);
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException("Endpoint should be either Everything or TopHeadlines.");
            }

            from ??= DateTime.Now;
            url += string.Format(NewsApiOrgConstants.FromDate, from.Value.ToString("yyyy-MM-dd"));

            to ??= DateTime.Now;
            url += string.Format(NewsApiOrgConstants.ToDate, to.Value.ToString("yyyy-MM-dd"));

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                NewsSortEnum sortByEnum = (NewsSortEnum)Enum.Parse(typeof(NewsSortEnum), sortBy.ToString());

                if (sortByEnum == NewsSortEnum.popularity || sortByEnum == NewsSortEnum.publishedAt)
                {
                    url += string.Format(NewsApiOrgConstants.SortBy, sortByEnum.ToString());
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Invalid enum. Sort by Popularity or PublishedAt.");
                }
            }
            else
            {
                url += string.Format(NewsApiOrgConstants.SortBy, NewsSortEnum.publishedAt.ToString());
            }

            url += Constants.Credentials.ExternalServices.NewsApiOrg.UrlSuffixApiKey;

            return url;
        }

        private void GenerateNewsSources()
        {
            _newsSourcesCollection.Add("AbcNews", "abc-news");
            _newsSourcesCollection.Add("AbcNewsAu", "abc-news-au");
            _newsSourcesCollection.Add("Aftenposten", "aftenposten");
            _newsSourcesCollection.Add("AlJazeeraEnglish", "al-jazeera-english");
            _newsSourcesCollection.Add("AnsaIt", "ansa");
            _newsSourcesCollection.Add("Argaam", "argaam");
            _newsSourcesCollection.Add("ArsTechnica", "ars-technica");
            _newsSourcesCollection.Add("AryNews", "ary-news");
            _newsSourcesCollection.Add("AssociatedPress", "associated-press");
            _newsSourcesCollection.Add("AustralianFinancialReview", "australian-financial-review");
            _newsSourcesCollection.Add("Axios", "axios");
            _newsSourcesCollection.Add("BBCNews", "bbc-news");
            _newsSourcesCollection.Add("BBCSport", "bbc-sport");
            _newsSourcesCollection.Add("Bild", "bild");
            _newsSourcesCollection.Add("BlastingNewsBR", "blasting-news-br");
            _newsSourcesCollection.Add("BleacherReport", "bleacher-report");
            _newsSourcesCollection.Add("Bloomberg", "bloomberg");
            _newsSourcesCollection.Add("BreitbartNews", "breitbart-news");
            _newsSourcesCollection.Add("BusinessInsider", "business-insider");
            _newsSourcesCollection.Add("BusinessInsiderUK", "business-insider-uk");
            _newsSourcesCollection.Add("Buzzfeed", "buzzfeed");
            _newsSourcesCollection.Add("CBCNews", "cbc-news");
            _newsSourcesCollection.Add("CBSNews", "cbs-news");
            _newsSourcesCollection.Add("CNN", "cnn");
            _newsSourcesCollection.Add("CNNSpanish", "cnn-es");
            _newsSourcesCollection.Add("CryptoCoinsNews", "crypto-coins-news");
            _newsSourcesCollection.Add("DerTagesspiegel", "der-tagesspiegel");
            _newsSourcesCollection.Add("DieZeit", "die-zeit");
            _newsSourcesCollection.Add("ElMundo", "el-mundo");
            _newsSourcesCollection.Add("Engadget", "engadget");
            _newsSourcesCollection.Add("EntertainmentWeekly", "entertainment-weekly");
            _newsSourcesCollection.Add("ESPN", "espn");
            _newsSourcesCollection.Add("ESPNCricInfo", "espn-cric-info");
            _newsSourcesCollection.Add("FinancialPost", "financial-post");
            _newsSourcesCollection.Add("Focus", "focus");
            _newsSourcesCollection.Add("FootballItalia", "football-italia");
            _newsSourcesCollection.Add("Fortune", "fortune");
            _newsSourcesCollection.Add("FourFourTwo", "four-four-two");
            _newsSourcesCollection.Add("FoxNews", "fox-news");
            _newsSourcesCollection.Add("FoxSports", "fox-sports");
            _newsSourcesCollection.Add("Globo", "globo");
            _newsSourcesCollection.Add("GoogleNews", "google-news");
            _newsSourcesCollection.Add("GoogleNewsArgentina", "google-news-ar");
            _newsSourcesCollection.Add("GoogleNewsAustralia", "google-news-au");
            _newsSourcesCollection.Add("GoogleNewsBrasil", "google-news-br");
            _newsSourcesCollection.Add("GoogleNewsCanada", "google-news-ca");
            _newsSourcesCollection.Add("GoogleNewsFrance", "google-news-fr");
            _newsSourcesCollection.Add("GoogleNewsIndia", "google-news-in");
            _newsSourcesCollection.Add("GoogleNewsIsrael", "google - news -is ");
            _newsSourcesCollection.Add("GoogleNewsItaly", "google-news-it");
            _newsSourcesCollection.Add("GoogleNewsRussia", "google-news-ru");
            _newsSourcesCollection.Add("GoogleNewsSaudiArabia", "google-news-sa");
            _newsSourcesCollection.Add("GoogleNewsUK", "google-news-uk");
            _newsSourcesCollection.Add("GöteborgsPosten", "goteborgs-posten");
            _newsSourcesCollection.Add("Gruenderszene", "gruenderszene");
            _newsSourcesCollection.Add("HackerNews", "hacker-news");
            _newsSourcesCollection.Add("Handelsblatt", "handelsblatt");
            _newsSourcesCollection.Add("IGN", "ign");
            _newsSourcesCollection.Add("IlSole24Ore", "il-sole-24-ore");
            _newsSourcesCollection.Add("Independent", "independent");
            _newsSourcesCollection.Add("Infobae", "infobae");
            _newsSourcesCollection.Add("InfoMoney", "info-money");
            _newsSourcesCollection.Add("LaGaceta", "la-gaceta");
            _newsSourcesCollection.Add("LaNacion", "la-nacion");
            _newsSourcesCollection.Add("LaRepubblica", "la-repubblica");
            _newsSourcesCollection.Add("LeMonde", "le-monde");
            _newsSourcesCollection.Add("Lenta", "lenta");
            _newsSourcesCollection.Add("Lequipe", "lequipe");
            _newsSourcesCollection.Add("LesEchos", "les-echos");
            _newsSourcesCollection.Add("Liberation", "liberation");
            _newsSourcesCollection.Add("Marca", "marca");
            _newsSourcesCollection.Add("Mashable", "mashable");
            _newsSourcesCollection.Add("MedicalNewsToday", "medical-news-today");
            _newsSourcesCollection.Add("MSNBC", "msnbc");
            _newsSourcesCollection.Add("MTVNews", "mtv-news");
            _newsSourcesCollection.Add("MTVNewsUK", "mtv-news-uk");
            _newsSourcesCollection.Add("NationalGeographic", "national-geographic");
            _newsSourcesCollection.Add("NationalReview", "national-review");
            _newsSourcesCollection.Add("NBCNews", "nbc-news");
            _newsSourcesCollection.Add("News24", "news24");
            _newsSourcesCollection.Add("NewScientist", "new-scientist");
            _newsSourcesCollection.Add("NewsComAu", "news-com-au");
            _newsSourcesCollection.Add("Newsweek", "newsweek");
            _newsSourcesCollection.Add("NewYorkMagazine", "new-york-magazine");
            _newsSourcesCollection.Add("NextBigFuture", "next-big-future");
            _newsSourcesCollection.Add("NFLNews", "nfl-news");
            _newsSourcesCollection.Add("NHLNews", "nhl-news");
            _newsSourcesCollection.Add("NRK", "nrk");
            _newsSourcesCollection.Add("Politico", "politico");
            _newsSourcesCollection.Add("Polygon", "polygon");
            _newsSourcesCollection.Add("RBC", "rbc");
            _newsSourcesCollection.Add("Recode", "recode");
            _newsSourcesCollection.Add("RedditRAll", "reddit-r-all");
            _newsSourcesCollection.Add("Reuters", "reuters");
            _newsSourcesCollection.Add("RT", "rt");
            _newsSourcesCollection.Add("RTE", "rte");
            _newsSourcesCollection.Add("RTLNieuws", "rtl-nieuws");
            _newsSourcesCollection.Add("SABQ", "sabq");
            _newsSourcesCollection.Add("SpiegelOnline", "spiegel-online");
            _newsSourcesCollection.Add("SvenskaDagbladet", "svenska-dagbladet");
            _newsSourcesCollection.Add("T3n", "t3n");
            _newsSourcesCollection.Add("TalkSport", "talksport");
            _newsSourcesCollection.Add("TechCrunch", "techcrunch");
            _newsSourcesCollection.Add("TechCrunchCN", "techcrunch-cn");
            _newsSourcesCollection.Add("TechRadar", "techradar");
            _newsSourcesCollection.Add("TheAmericanConservative", "the-american-conservative");
            _newsSourcesCollection.Add("TheGlobeAndMail", "the-globe-and-mail");
            _newsSourcesCollection.Add("TheHill", "the-hill");
            _newsSourcesCollection.Add("TheHindu", "the-hindu");
            _newsSourcesCollection.Add("TheHuffingtonPost", "the-huffington-post");
            _newsSourcesCollection.Add("TheIrishTimes", "the-irish-times");
            _newsSourcesCollection.Add("TheJerusalemPost", "the-jerusalem-post");
            _newsSourcesCollection.Add("TheLadBible", "the-lad-bible");
            _newsSourcesCollection.Add("TheNextWeb", "the-next-web");
            _newsSourcesCollection.Add("TheSportBible", "the-sport-bible");
            _newsSourcesCollection.Add("TheTimesOfIndia", "the-times-of-india");
            _newsSourcesCollection.Add("TheVerge", "the-verge");
            _newsSourcesCollection.Add("TheWallStreetJournal", "the-wall-street-journal");
            _newsSourcesCollection.Add("TheWashingtonPost", "the-washington-post");
            _newsSourcesCollection.Add("TheWashingtonTimes", "the-washington-times");
            _newsSourcesCollection.Add("Time", "time");
            _newsSourcesCollection.Add("USAToday", "usa-today");
            _newsSourcesCollection.Add("ViceNews", "vice-news");
            _newsSourcesCollection.Add("Wired", "wired");
            _newsSourcesCollection.Add("WiredDe", "wired-de");
            _newsSourcesCollection.Add("WirtschaftsWoche", "wirtschafts-woche");
            _newsSourcesCollection.Add("XinhuaNet", "xinhua-net");
            _newsSourcesCollection.Add("Ynet", "ynet");
        }
    }
}

// Everything, -, -, -, -, - => https://newsapi.org/v2/everything?q=Bulgaria&from=2022-02-25&to=2022-02-26&sortBy=publishedAt&apiKey=
// Everything, -, -, "Raspberry", -, - => https://newsapi.org/v2/everything?q=Raspberry&from=2022-02-25&to=2022-02-26&sortBy=publishedAt&apiKey=
// Everything, -, -, "Russia", Bbc-News, Popularity => https://newsapi.org/v2/everything?q=Russia&sources=bbc-news&from=2022-02-25&to=2022-02-26&sortBy=popularity&apiKey=
// TopHeadlines, set From, set To, -, -, FoxNews, popularity => https://newsapi.org/v2/top-headlines?sources=fox-news&from=2022-02-23&to=2022-02-24&sortBy=popularity&apiKey=
// TopHeadlines, -, -, Technology, bg, -, popularity => https://newsapi.org/v2/top-headlines?category=Technology&country=bg&from=2022-02-25&to=2022-02-26&sortBy=popularity&apiKey=
// TopHeadlines, -, -, -, -, - => https://newsapi.org/v2/top-headlines?country=bg&from=2022-02-25&to=2022-02-26&sortBy=publishedAt&apiKey=



