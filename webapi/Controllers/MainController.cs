using Microsoft.AspNetCore.Mvc;
using MP7_progi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class MainController : ControllerBase
{
    private readonly ILogger<MainController> _logger;

    public MainController(ILogger<MainController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetFrontPageData")]
    [Route("frontpagedata")]
    public string GetFrontPageData()
    {
        Dictionary<string, List<Object>> data = DatabaseFunctions.read(new Ad(),
                                                               new List<Table> { new Pet(), new ColorPet(), new hasColor(), new photoAd() },
                                                               new List<DatabaseFunctions.joinType> {
                                                                   DatabaseFunctions.joinType.Natural,
                                                                   DatabaseFunctions.joinType.Natural,
                                                                   DatabaseFunctions.joinType.Natural,
                                                                   DatabaseFunctions.joinType.Natural
                                                               }, null);

        return DatabaseFunctions.ConvertDictionaryToJson(data);
    }

    [HttpGet(Name = "Login")]
    [Route("login")]
    public string Login([FromQuery] string usrname, [FromQuery] string password)
    {
        Dictionary<string, List<Object>> result = new Dictionary<string, List<Object>>();
        Expression where = new Expression();
        string userID;

        try
        {
            userID = DatabaseFunctions.checkLoginData(usrname, password);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return "";
        }

        where.addElement((Object)"userID", Expression.OP.EQUAL);
        where.addElement((Object)userID, Expression.OP.None);
        result = DatabaseFunctions.read(new User(), new List<Table> { new Regular(), new typeOfUser() }, new List<DatabaseFunctions.joinType> { DatabaseFunctions.joinType.Natural, DatabaseFunctions.joinType.Natural }, where);

        if (result["Values"].Count != 0){
            Console.WriteLine(result.Values.Count);
            return DatabaseFunctions.ConvertDictionaryToJson(result);
        }

        result = DatabaseFunctions.read(new User(), new List<Table> { new Shelter(), new typeOfUser() }, new List<DatabaseFunctions.joinType> { DatabaseFunctions.joinType.Natural, DatabaseFunctions.joinType.Natural }, where);
        return DatabaseFunctions.ConvertDictionaryToJson(result);
    }

    [HttpPost(Name = "RegisterUser")]
    [Route("register_user")]
    public int RegisterUser([FromQuery] string usrname, [FromQuery] string password, [FromQuery] string email, [FromQuery] string phoneNum, [FromQuery] string name, [FromQuery] string surname)
    {
        List<Object> userRow = new List<Object>();
        int newId = DatabaseFunctions.getNextAvailableID(new User());
        userRow.Add(newId);
        userRow.Add(usrname);
        userRow.Add(email);
        userRow.Add(phoneNum);
        userRow.Add(password);


        int code1 = DatabaseFunctions.insert(new User(), userRow);

        List<Object> regularRow = new List<Object>();
        regularRow.Add(newId);
        regularRow.Add(name);
        regularRow.Add(surname);

        int code2 = DatabaseFunctions.insert(new Regular(), regularRow);

        List<Object> typeOfUserRow = new List<Object>();
        typeOfUserRow.Add(newId);
        typeOfUserRow.Add("regular");

        int code3 = DatabaseFunctions.insert(new typeOfUser(), typeOfUserRow);

        if (code1 == 200 && code2 == 200 && code3 == 200)
        {
            return code1;
        }
        else if (code1 != 200)
        {
            return code1;
        }
        else if (code2 != 200)
        {
            return code2;
        }
        return code3;
    }

    [HttpPost(Name = "RegisterShelter")]
    [Route("register_shelter")]
    public int RegisterShelter([FromQuery] string usrname, [FromQuery] string password, [FromQuery] string email, [FromQuery] string phoneNum, [FromQuery] string shelterName)
    {

        List<Object> userRow = new List<Object>();
        int newId = DatabaseFunctions.getNextAvailableID(new User());
        userRow.Add(newId);
        userRow.Add(usrname);
        userRow.Add(email);
        userRow.Add(phoneNum);
        userRow.Add(password);

        int code1 = DatabaseFunctions.insert(new User(), userRow);

        List<Object> shelterRow = new List<Object>();
        shelterRow.Add(newId);
        shelterRow.Add(shelterName);

        int code2 = DatabaseFunctions.insert(new Shelter(), shelterRow);

        List<Object> typeOfUserRow = new List<Object>();
        typeOfUserRow.Add(newId);
        typeOfUserRow.Add("shelter");

        int code3 = DatabaseFunctions.insert(new typeOfUser(), typeOfUserRow);

        if (code1 == 200 && code2 == 200 && code3 == 200)
        {
            return code1;
        }
        else if (code1 != 200)
        {
            return code1;
        }
        else if (code2 != 200)
        {
            return code2;
        }
        return code3;
    }

    [HttpPost(Name = "PostAd")]
    [Route("postAd")]
    public int PostAd([FromBody] string insertJSON)
    {
        Dictionary<string, List<Object>> insertDictionary;

        try
        {
            insertDictionary = DatabaseFunctions.ConvertJsonToDictionary(insertJSON);
        }
        catch
        {
            Console.WriteLine("Incorrect JSON format!");
            return 400;
        }

        List<Object> names = insertDictionary["Names"];
        List<Object> values = insertDictionary["Values"];
        List<Object> row = new List<Object>();

        Ad ad = new Ad();
        Pet pet = new Pet();
        hasColor hc = new hasColor();
        photoAd pa = new photoAd();

        row = (List<Object>)values[0];

        try
        {
            /* Insert into Ad */
            List<Object> insertRowAd = new List<Object>();
            foreach (var name in names)
            {
                if (ad.returnColumnTypes().ContainsKey(name.ToString()))
                {
                    int index = names.IndexOf(name.ToString());
                    insertRowAd[ad.returnColumnTypes().Keys.ToList().IndexOf(name.ToString())] = row[index];
                }
            }
            DatabaseFunctions.insert(ad, insertRowAd);

            /* Insert into Pet */
            List<Object> insertRowPet = new List<Object>();
            foreach (var name in names)
            {
                if (pet.returnColumnTypes().ContainsKey(name.ToString()))
                {
                    int index = names.IndexOf(name.ToString());
                    insertRowPet[pet.returnColumnTypes().Keys.ToList().IndexOf(name.ToString())] = row[index];
                }
            }
            DatabaseFunctions.insert(pet, insertRowPet);

            /* Insert into hasColor */
            List<Object> insertRowHC = new List<Object>();
            foreach (var name in names)
            {
                if (hc.returnColumnTypes().ContainsKey(name.ToString()))
                {
                    int index = names.IndexOf(name.ToString());
                    insertRowHC[hc.returnColumnTypes().Keys.ToList().IndexOf(name.ToString())] = row[index];
                }
            }
            DatabaseFunctions.insert(hc, insertRowHC);

            /* Insert into photoAd */
            List<Object> insertRowPA = new List<Object>();
            foreach (var name in names)
            {
                if (pa.returnColumnTypes().ContainsKey(name.ToString()))
                {
                    int index = names.IndexOf(name.ToString());
                    insertRowPA[pa.returnColumnTypes().Keys.ToList().IndexOf(name.ToString())] = row[index];
                }
            }
            DatabaseFunctions.insert(pa, insertRowPA);
        }
        catch (Exception ex)
        {
            // Missing database cleanup - required Delete method.
            Console.WriteLine(ex.ToString());
            return 400;
        }

        return 200;
    }

}


