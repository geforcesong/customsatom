using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ProcessorUtilities;
using System.Net;
using System.IO;
using System.Net.Cache;

namespace GetHSCodeDetail
{
    class Program
    {
        private static readonly SqlConnection conn = new SqlConnection("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=CustomsAtom;Data Source=localhost");

        static void Main(string[] args)
        {
            conn.Open();

            ParseRootPage();

            conn.Close();
        }

        private static void ParseRootPage()
        {
            for (int i = 0; i < 10; i++)
            {
                string totalPage = string.Empty;
                string totalCount = string.Empty;
                string url = "http://ys.hscode.net/TariffQueryNew.aspx?TariffCode=" + i;
                string htmlContent = HtmlParseUtils.SaveWebPage(url);
                if (!string.IsNullOrEmpty(htmlContent))
                {
                    string content = HtmlParseUtils.FormatHtml(htmlContent, false, true);
                    if (!string.IsNullOrEmpty(content))
                    {
                        totalPage = HtmlParseUtils.GetSubString(content, "总页数：", null, " 每页", "总页数：", null, "每页");
                        totalCount = HtmlParseUtils.GetSubString(content, "总数：", null, " 【", "总数：", null, " 【");
                    }
                }

                var postDataTemplte = "__VIEWSTATE=%2FwEPDwUKLTk5ODk1MzgyMw9kFgICAw9kFgICBw8WAh4LXyFJdGVtQ291bnQCHhY8Zg9kFgZmDxUKCjEwMDExMTAwMDES56eN55So56Gs57KS5bCP6bqmUzE65ZOB5ZCNOzI65piv5ZCm56eN55SoOzM65piv5ZCm56Gs57KSOzQ656eN57G7KOeZvem6puOAgee6oum6puOAgeWGrOm6puOAgeaYpem6pik7BuWNg%2BWFiwPml6ACMSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh4EVGV4dAUGNHhBQnR5FgQeC29ubW91c2VvdmVyBRV3c3VnKGV2ZW50LCAnNHhBQnR5JykeCm9ubW91c2VvdXQFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWGhWQCAQ9kFgZmDxUKCjEwMDExMTAwOTAS56eN55So56Gs57KS5bCP6bqmUzE65ZOB5ZCNOzI65piv5ZCm56eN55SoOzM65piv5ZCm56Gs57KSOzQ656eN57G7KOeZvem6puOAgee6oum6puOAgeWGrOm6puOAgeaYpem6pik7BuWNg%2BWFiwPml6ADNjUlBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUFNHhBQnkWBB8CBRR3c3VnKGV2ZW50LCAnNHhBQnknKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQnphY3pop3lpJZkAgIPZBYGZg8VCgoxMDAxMTkwMDAxEuWFtuS7luehrOeykuWwj%2Bm6plMxOuWTgeWQjTsyOuaYr%2BWQpuenjeeUqDszOuaYr%2BWQpuehrOeykjs0Ouenjeexuyjnmb3puqbjgIHnuqLpuqbjgIHlhqzpuqbjgIHmmKXpuqYpOwbljYPlhYsD5pegAjElBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUGNHhBQnR5FgQfAgUVd3N1ZyhldmVudCwgJzR4QUJ0eScpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWGhWQCAw9kFgZmDxUKCjEwMDExOTAwOTAS5YW25LuW56Gs57KS5bCP6bqmUzE65ZOB5ZCNOzI65piv5ZCm56eN55SoOzM65piv5ZCm56Gs57KSOzQ656eN57G7KOeZvem6puOAgee6oum6puOAgeWGrOm6puOAgeaYpem6pik7BuWNg%2BWFiwPml6ADNjUlBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUFNHhBQnkWBB8CBRR3c3VnKGV2ZW50LCAnNHhBQnknKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQnphY3pop3lpJZkAgQPZBYGZg8VCgoxMDAxOTEwMDAxHuWFtuS7luenjeeUqOWwj%2Bm6puWPiua3t%2BWQiOm6plMxOuWTgeWQjTsyOuaYr%2BWQpuenjeeUqDszOuaYr%2BWQpuehrOeykjs0Ouenjeexuyjnmb3puqbjgIHnuqLpuqbjgIHlhqzpuqbjgIHmmKXpuqYpOwbljYPlhYsD5pegAjElBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUGNHhBQnR5FgQfAgUVd3N1ZyhldmVudCwgJzR4QUJ0eScpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWGhWQCBQ9kFgZmDxUKCjEwMDE5MTAwOTAe5YW25LuW56eN55So5bCP6bqm5Y%2BK5re35ZCI6bqmUzE65ZOB5ZCNOzI65piv5ZCm56eN55SoOzM65piv5ZCm56Gs57KSOzQ656eN57G7KOeZvem6puOAgee6oum6puOAgeWGrOm6puOAgeaYpem6pik7BuWNg%2BWFiwPml6ADNjUlBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUFNHhBQnkWBB8CBRR3c3VnKGV2ZW50LCAnNHhBQnknKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQnphY3pop3lpJZkAgYPZBYGZg8VCgoxMDAxOTkwMDAxGOWFtuS7luWwj%2Bm6puWPiua3t%2BWQiOm6plMxOuWTgeWQjTsyOuaYr%2BWQpuenjeeUqDszOuaYr%2BWQpuehrOeykjs0Ouenjeexuyjnmb3puqbjgIHnuqLpuqbjgIHlhqzpuqbjgIHmmKXpuqYpOwbljYPlhYsD5pegAjElBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUGNHhBQnR5FgQfAgUVd3N1ZyhldmVudCwgJzR4QUJ0eScpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWGhWQCBw9kFgZmDxUKCjEwMDE5OTAwOTAY5YW25LuW5bCP6bqm5Y%2BK5re35ZCI6bqmUzE65ZOB5ZCNOzI65piv5ZCm56eN55SoOzM65piv5ZCm56Gs57KSOzQ656eN57G7KOeZvem6puOAgee6oum6puOAgeWGrOm6puOAgeaYpem6pik7BuWNg%2BWFiwPml6ADNjUlBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUFNHhBQnkWBB8CBRR3c3VnKGV2ZW50LCAnNHhBQnknKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQnphY3pop3lpJZkAggPZBYGZg8VCgoxMDAyMTAwMDAwDOenjeeUqOm7kem6phgxOuWTgeWQjTsyOuaYr%2BWQpuenjeeUqDsG5Y2D5YWLA%2BaXoAIwJQIwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQJBQhYEHwIFEXdzdWcoZXZlbnQsICdBQicpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBA%2BaXoGQCCQ9kFgZmDxUKCjEwMDI5MDAwMDAM5YW25LuW6buR6bqmGDE65ZOB5ZCNOzI65piv5ZCm56eN55SoOwbljYPlhYsD5pegAjMlAjglBDAuMCUEMC4wJQMxMyVkAgEPDxYCHwEFAkFCFgQfAgURd3N1ZyhldmVudCwgJ0FCJykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQED5pegZAIKD2QWBmYPFQoKMTAwMzEwMDAwMAznp43nlKjlpKfpuqYYMTrlk4HlkI07MjrmmK%2FlkKbnp43nlKg7BuWNg%2BWFiwPml6ACMCUEMTYwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQJBQhYEHwIFEXdzdWcoZXZlbnQsICdBQicpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBA%2BaXoGQCCw9kFgZmDxUKCjEwMDM5MDAwMDAM5YW25LuW5aSn6bqmGDE65ZOB5ZCNOzI65piv5ZCm56eN55SoOwbljYPlhYsD5pegAjMlBDE2MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUCQUIWBB8CBRF3c3VnKGV2ZW50LCAnQUInKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQPml6BkAgwPZBYGZg8VCgoxMDA0MTAwMDAwDOenjeeUqOeHlem6phgxOuWTgeWQjTsyOuaYr%2BWQpuenjeeUqDsG5Y2D5YWLA%2BaXoAIwJQIwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQJBQhYEHwIFEXdzdWcoZXZlbnQsICdBQicpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBA%2BaXoGQCDQ9kFgZmDxUKCjEwMDQ5MDAwMDAM5YW25LuW54eV6bqmGDE65ZOB5ZCNOzI65piv5ZCm56eN55SoOwbljYPlhYsD5pegAjIlAjglBDAuMCUEMC4wJQMxMyVkAgEPDxYCHwEFAkFCFgQfAgURd3N1ZyhldmVudCwgJ0FCJykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQED5pegZAIOD2QWBmYPFQoKMTAwNTEwMDAwMQznp43nlKjnjonnsbMhMTrlk4HlkI07MjrmmK%2FlkKbnp43nlKg7Mzrlk4HniYw7BuWNg%2BWFiwPml6ACMSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQY0eEFCeXQWBB8CBRV3c3VnKGV2ZW50LCAnNHhBQnl0JykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQEJ6YWN6aKd5YaFZAIPD2QWBmYPFQoKMTAwNTEwMDA5MAznp43nlKjnjonnsbMhMTrlk4HlkI07MjrmmK%2FlkKbnp43nlKg7Mzrlk4HniYw7BuWNg%2BWFiwPml6ADMjAlBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUFNHhBQnkWBB8CBRR3c3VnKGV2ZW50LCAnNHhBQnknKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQnphY3pop3lpJZkAhAPZBYGZg8VCgoxMDA1OTAwMDAxDOWFtuS7lueOieexsyoxOuWTgeWQjTsyOuaYr%2BWQpuenjeeUqDszOueUqOmAlDs0OuWTgeeJjDsG5Y2D5YWLA%2BaXoAIxJQQxODAlBDAuMCUEMC4wJQMxMyVkAgEPDxYCHwEFBjR4QUJ5dBYEHwIFFXdzdWcoZXZlbnQsICc0eEFCeXQnKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQnphY3pop3lhoVkAhEPZBYGZg8VCgoxMDA1OTAwMDkwDOWFtuS7lueOieexsyoxOuWTgeWQjTsyOuaYr%2BWQpuenjeeUqDszOueUqOmAlDs0OuWTgeeJjDsG5Y2D5YWLA%2BaXoAM2NSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQU0eEFCeRYEHwIFFHdzdWcoZXZlbnQsICc0eEFCeScpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWklmQCEg9kFgZmDxUKCjEwMDYxMDExMDES56eN55So57G857Gz56i76LC3QTE65ZOB5ZCNOzI656eN57G7KOexvOexsyk7MzrmmK%2FlkKbnp43nlKg7NDrljIXoo4Xop4TmoLw7NTrlk4HniYw7BuWNg%2BWFiwPml6ACMSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQY0eEFCeXQWBB8CBRV3c3VnKGV2ZW50LCAnNHhBQnl0JykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQEJ6YWN6aKd5YaFZAITD2QWBmYPFQoKMTAwNjEwMTE5MBLnp43nlKjnsbznsbPnqLvosLdBMTrlk4HlkI07Mjrnp43nsbso57G857GzKTszOuaYr%2BWQpuenjeeUqDs0OuWMheijheinhOagvDs1OuWTgeeJjDsG5Y2D5YWLA%2BaXoAM2NSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQU0eEFCeRYEHwIFFHdzdWcoZXZlbnQsICc0eEFCeScpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWklmQCFA9kFgZmDxUKCjEwMDYxMDE5MDES5YW25LuW56eN55So56i76LC3YjE65ZOB5ZCNOzI656eN57G7KOmmmeexs%2BOAgeW3tOWQnuexs%2BOAgeeZveexs%2BOAgeezr%2Bexs%2BetiSk7MzrmmK%2FlkKbnp43nlKg7NDrljIXoo4Xop4TmoLw7NTrlk4HniYw7BuWNg%2BWFiwPml6ACMSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQY0eEFCeXQWBB8CBRV3c3VnKGV2ZW50LCAnNHhBQnl0JykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQEJ6YWN6aKd5YaFZAIVD2QWBmYPFQoKMTAwNjEwMTk5MBLlhbbku5bnp43nlKjnqLvosLdiMTrlk4HlkI07Mjrnp43nsbso6aaZ57Gz44CB5be05ZCe57Gz44CB55m957Gz44CB57Ov57Gz562JKTszOuaYr%2BWQpuenjeeUqDs0OuWMheijheinhOagvDs1OuWTgeeJjDsG5Y2D5YWLA%2BaXoAM2NSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQU0eEFCeRYEHwIFFHdzdWcoZXZlbnQsICc0eEFCeScpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWklmQCFg9kFgZmDxUKCjEwMDYxMDkxMDES5YW25LuW57G857Gz56i76LC3MjE65ZOB5ZCNOzI656eN57G7KOexvOexsyk7MzrljIXoo4Xop4TmoLw7NDrlk4HniYw7BuWNg%2BWFiwPml6ACMSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQY0eEFCeXQWBB8CBRV3c3VnKGV2ZW50LCAnNHhBQnl0JykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQEJ6YWN6aKd5YaFZAIXD2QWBmYPFQoKMTAwNjEwOTE5MBLlhbbku5bnsbznsbPnqLvosLcyMTrlk4HlkI07Mjrnp43nsbso57G857GzKTszOuWMheijheinhOagvDs0OuWTgeeJjDsG5Y2D5YWLA%2BaXoAM2NSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQU0eEFCeRYEHwIFFHdzdWcoZXZlbnQsICc0eEFCeScpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWklmQCGA9kFgZmDxUKCjEwMDYxMDk5MDEM5YW25LuW56i76LC3XDE65ZOB5ZCNOzI656eN57G7KOmmmeexs%2BOAgeexvOexs%2BOAgeW3tOWQnuexs%2BOAgeeZveexs%2BOAgeezr%2Bexs%2BetiSk7MzrljIXoo4Xop4TmoLw7NDrlk4HniYw7BuWNg%2BWFiwPml6ACMSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQY0eEFCeXQWBB8CBRV3c3VnKGV2ZW50LCAnNHhBQnl0JykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQEJ6YWN6aKd5YaFZAIZD2QWBmYPFQoKMTAwNjEwOTk5MAzlhbbku5bnqLvosLdcMTrlk4HlkI07Mjrnp43nsbso6aaZ57Gz44CB57G857Gz44CB5be05ZCe57Gz44CB55m957Gz44CB57Ov57Gz562JKTszOuWMheijheinhOagvDs0OuWTgeeJjDsG5Y2D5YWLA%2BaXoAM2NSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQU0eEFCeRYEHwIFFHdzdWcoZXZlbnQsICc0eEFCeScpHwMFDndzdWcoZXZlbnQsIDApZAICDxUBCemFjemineWklmQCGg9kFgZmDxUKCjEwMDYyMDEwMDEM57G857Gz57OZ57GzUjE65ZOB5ZCNOzI656eN57G7KOexvOexsyk7MzrliLbkvZzmiJbkv53lrZjmlrnms5Uo57OZ57GzKTs0OuWMheijheinhOagvDs1OuWTgeeJjDsG5Y2D5YWLA%2BaXoAIxJQQxODAlBDAuMCUEMC4wJQMxMyVkAgEPDxYCHwEFBjR4QUJ5dBYEHwIFFXdzdWcoZXZlbnQsICc0eEFCeXQnKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQnphY3pop3lhoVkAhsPZBYGZg8VCgoxMDA2MjAxMDkwDOexvOexs%2Bezmeexs1IxOuWTgeWQjTsyOuenjeexuyjnsbznsbMpOzM65Yi25L2c5oiW5L%2Bd5a2Y5pa55rOVKOezmeexsyk7NDrljIXoo4Xop4TmoLw7NTrlk4HniYw7BuWNg%2BWFiwPml6ADNjUlBDE4MCUEMC4wJQQwLjAlAzEzJWQCAQ8PFgIfAQUFNHhBQnkWBB8CBRR3c3VnKGV2ZW50LCAnNHhBQnknKR8DBQ53c3VnKGV2ZW50LCAwKWQCAg8VAQnphY3pop3lpJZkAhwPZBYGZg8VCgoxMDA2MjA5MDAxDOWFtuS7luezmeexs3MxOuWTgeWQjTsyOuenjeexuyjpppnnsbPjgIHlt7TlkJ7nsbPjgIHnmb3nsbPjgIHns6%2FnsbPnrYkpOzM65Yi25L2c5oiW5L%2Bd5a2Y5pa55rOVKOezmeexsyk7NDrljIXoo4Xop4TmoLw7NTrlk4HniYw7BuWNg%2BWFiwPml6ACMSUEMTgwJQQwLjAlBDAuMCUDMTMlZAIBDw8WAh8BBQY0eEFCeXQWBB8CBRV3c3VnKGV2ZW50LCAnNHhBQnl0JykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQEJ6YWN6aKd5YaFZAIdD2QWBmYPFQoKMTAwNjIwOTA5MAzlhbbku5bns5nnsbNzMTrlk4HlkI07Mjrnp43nsbso6aaZ57Gz44CB5be05ZCe57Gz44CB55m957Gz44CB57Ov57Gz562JKTszOuWItuS9nOaIluS%2FneWtmOaWueazlSjns5nnsbMpOzQ65YyF6KOF6KeE5qC8OzU65ZOB54mMOwbljYPlhYsD5pegAzY1JQQxODAlBDAuMCUEMC4wJQMxMyVkAgEPDxYCHwEFBTR4QUJ5FgQfAgUUd3N1ZyhldmVudCwgJzR4QUJ5JykfAwUOd3N1ZyhldmVudCwgMClkAgIPFQEJ6YWN6aKd5aSWZBgBBR5fX0NvbnRyb2xzUmVxdWlyZVBvc3RCYWNrS2V5X18WAQUFcGFnZXI4%2Fcs%2Bab8xF5oWOmQMWUqJSE%2FJcw%3D%3D&__EVENTTARGET=pager&__EVENTARGUMENT=next&__EVENTVALIDATION=%2FwEWBALxjpWjAgKhrID9BgLm2p%2FaAgLvjry%2FBfG3W2fX8dImMO%2BrRbbueMpie7%2BY&textboxTariff10CodeQuery={0}&textboxProductNameQuery=&pager_PageArgs={1}&pager_PageIndexFromSelect={2}&pager_PageSize=30";

                for (int j = 0; j <= int.Parse(totalPage); j++)
                {
                    var postData = string.Format(postDataTemplte, i, totalCount + "%2c" + (j + 1) + "%2c" + (j - 1) + "%2c30", j);
                    string content = PostWebRequest(url, postData, Encoding.UTF8);

                    List<string> itemList = HtmlParseUtils.GetSubStrings(content, "<table class=\"xind-t\" cellpadding=\"5\" cellspacing=\"0\" width=\"850\" class=\"zwz\">",
                        null, "备注：", "<table class=\"xind-t\" cellpadding=\"5\" cellspacing=\"0\" width=\"850\" class=\"zwz\">", null, "备注：");
                    if (itemList != null)
                    {
                        foreach (var contentDetail in itemList)
                        {
                            List<string> detailList = HtmlParseUtils.GetSubStrings(contentDetail, "style=\"color: black\">", null, "</", "style=\"color: black\">", null, "</");
                            string hsCode = HtmlParseUtils.GetSubString(contentDetail, "<label style=\"color: #2268a6; font-weight: bold;\">", null, "</label>", "<label style=\"color: #2268a6; font-weight: bold;\">", null, "</label>");
                            if (detailList != null && detailList.Count >= 10)
                            {
                                try
                                {
                                    string Name = detailList[0];
                                    string DeclarationFactor = detailList[1];
                                    string FirstUnitName = detailList[2];
                                    string SecondUnitName = detailList[3];
                                    string ManagementName = detailList[9];

                                    string saveSql = @"INSERT INTO [dbo].[HSCodeDictionary]
                                                           ([Code]
                                                           ,[Name]
                                                           ,[ManagementName]
                                                           ,[DeclarationFactor]
                                                           ,[FirstUnitName]
                                                           ,[SecondUnitName])
                                                     VALUES
                                                           (@Code
                                                           ,@Name
                                                           ,@ManagementName
                                                           ,@DeclarationFactor
                                                           ,@FirstUnitName
                                                           ,@SecondUnitName)";

                                    SqlParameter paramName = new SqlParameter("@Name", Name);
                                    SqlParameter paramDeclarationFactor = new SqlParameter("@DeclarationFactor", DeclarationFactor);
                                    SqlParameter paramFirstUnitName = new SqlParameter("@FirstUnitName", FirstUnitName);
                                    SqlParameter paramSecondUnitName = new SqlParameter("@SecondUnitName", SecondUnitName);
                                    SqlParameter paramManagementName = new SqlParameter("@ManagementName", ManagementName);
                                    SqlParameter paramCode = new SqlParameter("@Code", hsCode);

                                    using (SqlCommand command = new SqlCommand(saveSql, conn))
                                    {
                                        command.Parameters.Add(paramName);
                                        command.Parameters.Add(paramDeclarationFactor);
                                        command.Parameters.Add(paramFirstUnitName);
                                        command.Parameters.Add(paramSecondUnitName);
                                        command.Parameters.Add(paramManagementName);
                                        command.Parameters.Add(paramCode);
                                        command.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }
        private static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.Accept = "image/jpeg, image/gif, image/pjpeg, application/x-ms-application, application/xaml+xml, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
                webReq.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
                //webReq.
                RequestCachePolicy p = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                webReq.CachePolicy = p;
                CookieContainer c = new CookieContainer();
                Cookie cookie = new Cookie();

                c.Add(new Cookie("lzstat_uv", "12411622392981350024|737288", "", "ys.hscode.net"));
                c.Add(new Cookie("Pager-PageSize", "30", "", "ys.hscode.net"));
                webReq.CookieContainer = c;

                webReq.Host = "ys.hscode.net";
                webReq.Referer = "http://ys.hscode.net/TariffQueryNew.aspx?TariffCode=0";
                webReq.KeepAlive = true;
                webReq.ContentLength = byteArray.Length;
                webReq.KeepAlive = false;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), dataEncode);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                ex.HelpLink = postUrl;
                return string.Empty;
            }
            return ret;
        }
    }
}
