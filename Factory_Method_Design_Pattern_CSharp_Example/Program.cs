//GarantiBank garantiBank = new("asd", "123");
//garantiBank.ConnectGaranti();


//VakifBank vakifBank = new(new() { UserCode = "gncy", Mail = "gncy@gencayyildiz.com" }, "123");
//bool result = vakifBank.ValidateCredential();
//if (result)
//{
//    //...
//}

//HalkBank halkBank = new("gncy");
//halkBank.Password = "123";

using System.Reflection;

BankCreator bankCreator = new();
GarantiBank? garanti = bankCreator.Create(BankType.Garanti) as GarantiBank;
HalkBank? halkBank = bankCreator.Create(BankType.HalkBank) as HalkBank;
VakifBank? vakifbank = bankCreator.Create(BankType.VakifBank) as VakifBank;

GarantiBank? garanti2 = bankCreator.Create(BankType.Garanti) as GarantiBank;
HalkBank? halkBank2 = bankCreator.Create(BankType.HalkBank) as HalkBank;
VakifBank? vakifbank2 = bankCreator.Create(BankType.VakifBank) as VakifBank;

GarantiBank? garanti3 = bankCreator.Create(BankType.Garanti) as GarantiBank;
HalkBank? halkBank3 = bankCreator.Create(BankType.HalkBank) as HalkBank;
VakifBank? vakifbank3 = bankCreator.Create(BankType.VakifBank) as VakifBank;



#region Abstract Product
interface IBank
{

}
#endregion

#region Concrete Products
class GarantiBank : IBank
{
    string _userCode, _password;
    GarantiBank(string userCode, string password)
    {
        Console.WriteLine($"{nameof(GarantiBank)} nesnesi oluşturuldu.");
        _userCode = userCode;
        _password = password;
    }

    static GarantiBank()
    {
        _garantiBank = new("asd", "123");
    }

    static GarantiBank _garantiBank;

    public static GarantiBank GetInstance()
    {
        return _garantiBank;
    }

    public void ConnectGaranti()
        => Console.WriteLine($"{nameof(GarantiBank)} - Connected.");
    public void SendMoney(int amount)
        => Console.WriteLine($"{amount} money sent.");
}

class HalkBank : IBank
{
    string _userCode, _password;
    HalkBank(string userCode)
    {
        Console.WriteLine($"{nameof(HalkBank)} nesnesi oluşturuldu.");
        _userCode = userCode;
    }

    static HalkBank() => _halkbank = new("asd");

    static HalkBank _halkbank;
    public static HalkBank GetInstance => _halkbank;

    public string Password { set => _password = value; }

    public void Send(int amount, string accountNumber)
        => Console.WriteLine($"{amount} money sent.");
}

class CredentialVakifBank
{
    public string UserCode { get; set; }
    public string Mail { get; set; }
}
class VakifBank : IBank
{
    string _userCode, _email, _password;
    public bool isAuthentcation { get; set; }
    VakifBank(CredentialVakifBank credential, string password)
    {
        Console.WriteLine($"{nameof(VakifBank)} nesnesi oluşturuldu.");
        _userCode = credential.UserCode;
        _email = credential.Mail;
        _password = password;
    }

    static VakifBank() => _vakifbank = new(new() { Mail = "gncy@gencayyildiz.com", UserCode = "gncy" }, "123");


    static VakifBank _vakifbank;
    public static VakifBank GetInstance => _vakifbank;
    public void ValidateCredential()
    {
        if (true) //validating
            isAuthentcation = true;
    }

    public void SendMoneyToAccountNumber(int amount, string recipientName, string accountNumber)
        => Console.WriteLine($"{amount} money sent.");
}
#endregion

#region Abstract Factory
interface IBankFactory
{
    static IBankFactory GetInstance {  get; }
    IBank CreateInstance();
}
#endregion
#region Concrete Factories
class GarantiFactory : IBankFactory
{
    GarantiFactory() { }
    static GarantiFactory() => _garantiFactory = new GarantiFactory();
    static readonly GarantiFactory _garantiFactory;
    public static IBankFactory GetInstance => _garantiFactory;

    public IBank CreateInstance()
    {
        GarantiBank garanti = GarantiBank.GetInstance();
        garanti.ConnectGaranti();
        return garanti;
    }
}
class HalkBankFactory : IBankFactory
{
    HalkBankFactory() { }
    static HalkBankFactory() => _halkbankFactory = new HalkBankFactory();
    static readonly HalkBankFactory _halkbankFactory;
    public static IBankFactory GetInstance => _halkbankFactory;

    public IBank CreateInstance()
    {
        HalkBank halkBank = HalkBank.GetInstance;
        halkBank.Password = "123";
        return halkBank;
    }
}
class VakifBankFactory : IBankFactory
{
    VakifBankFactory() { }
    static VakifBankFactory() => _vakifbankFactory = new VakifBankFactory();
    static readonly VakifBankFactory _vakifbankFactory;
    public static IBankFactory GetInstance => _vakifbankFactory;
    public IBank CreateInstance()
    {
        VakifBank vakifBank = VakifBank.GetInstance;
        vakifBank.ValidateCredential();
        return vakifBank;
    }
}
#endregion

#region Creator
enum BankType
{
    Garanti, HalkBank, VakifBank
}
class BankCreator
{
    //static Dictionary<BankType, BankCreator> _factories = new();
    public IBank Create(BankType bankType)
    {
        //if (!_factories.ContainsKey(bankType))
        //{
        //    _factories[bankType] = new IBankFactory()
        //}
        string factory = $"{bankType}Factory";
        Type? type = Assembly.GetExecutingAssembly().GetType(factory);
        PropertyInfo? getInstanceMethod = type?.GetProperty(nameof(IBankFactory.GetInstance)/*, BindingFlags.Static | BindingFlags.Public*/);
        object? factoryInstance = getInstanceMethod?.GetValue(null, null);
        IBankFactory? bankFactory = factoryInstance as IBankFactory;
        return bankFactory.CreateInstance();
    }
}
#endregion