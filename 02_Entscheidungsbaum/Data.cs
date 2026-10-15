namespace _05_Entscheidungsbaum;

//#region GoTennis
//public class Data
//{

//    public Enum[,] trainingData =
//    {
//        {Enums.Outlook.Sunny, Enums.Temperature.Hot, Enums.Humidity.High, Enums.Wind.Weak, Enums.GoTennis.No},
//        {Enums.Outlook.Sunny, Enums.Temperature.Hot, Enums.Humidity.High, Enums.Wind.Strong, Enums.GoTennis.No},
//        {Enums.Outlook.Overcast, Enums.Temperature.Hot, Enums.Humidity.High, Enums.Wind.Weak, Enums.GoTennis.Yes},
//        {Enums.Outlook.Rain, Enums.Temperature.Mild,Enums.Humidity.High, Enums.Wind.Weak, Enums.GoTennis.Yes},
//        {Enums.Outlook.Rain, Enums.Temperature.Cold, Enums.Humidity.Normal,Enums.Wind.Weak, Enums.GoTennis.Yes},
//        {Enums.Outlook.Rain, Enums.Temperature.Cold, Enums.Humidity.Normal, Enums.Wind.Strong, Enums.GoTennis.No},
//        {Enums.Outlook.Overcast, Enums.Temperature.Cold, Enums.Humidity.Normal, Enums.Wind.Weak, Enums.GoTennis.Yes},
//        {Enums.Outlook.Sunny, Enums.Temperature.Mild, Enums.Humidity.High, Enums.Wind.Weak, Enums.GoTennis.No},
//        {Enums.Outlook.Sunny, Enums.Temperature.Cold, Enums.Humidity.Normal, Enums.Wind.Weak, Enums.GoTennis.Yes},
//        {Enums.Outlook.Rain, Enums.Temperature.Mild, Enums.Humidity.Normal, Enums.Wind.Strong, Enums.GoTennis.No},
//        {Enums.Outlook.Sunny, Enums.Temperature.Mild, Enums.Humidity.Normal, Enums.Wind.Strong, Enums.GoTennis.Yes},
//        {Enums.Outlook.Overcast, Enums.Temperature.Mild, Enums.Humidity.High, Enums.Wind.Strong, Enums.GoTennis.Yes},
//        {Enums.Outlook.Overcast, Enums.Temperature.Hot, Enums.Humidity.Normal, Enums.Wind.Weak, Enums.GoTennis.Yes},
//        {Enums.Outlook.Rain, Enums.Temperature.Mild, Enums.Humidity.High, Enums.Wind.Strong, Enums.GoTennis.No}
//    };
//}

//public class Enums
//{
//    public enum Outlook
//    {
//        Sunny,
//        Overcast,
//        Rain,
//    }
//    public enum Temperature
//    {
//        Hot,
//        Mild,
//        Cold
//    }
//    public enum Humidity
//    {
//        High,
//        Normal
//    }
//    public enum Wind
//    {
//        Strong,
//        Weak
//    }
//    public enum GoTennis
//    {
//        Yes,
//        No
//    }

//}
//#endregion

#region Creditworthiness
public class Data
{
    public Enum[,] trainingData =
    {
        {FinancialEnums.Age.Under25, FinancialEnums.Education.ALevels, FinancialEnums.Status.Married, FinancialEnums.Income.High, FinancialEnums.Creditworthiness.Yes},
        {FinancialEnums.Age.Under25, FinancialEnums.Education.Bachelor, FinancialEnums.Status.Divorced, FinancialEnums.Income.High, FinancialEnums.Creditworthiness.Yes},
        {FinancialEnums.Age.Under35, FinancialEnums.Education.ALevels, FinancialEnums.Status.Married, FinancialEnums.Income.Low, FinancialEnums.Creditworthiness.Yes},
        {FinancialEnums.Age.Under35, FinancialEnums.Education.HighSchoolDiploma, FinancialEnums.Status.Divorced, FinancialEnums.Income.Low, FinancialEnums.Creditworthiness.Yes},
        {FinancialEnums.Age.Under45, FinancialEnums.Education.HighSchoolDiploma, FinancialEnums.Status.Married, FinancialEnums.Income.High, FinancialEnums.Creditworthiness.Yes},
        {FinancialEnums.Age.Under45, FinancialEnums.Education.Bachelor, FinancialEnums.Status.Married, FinancialEnums.Income.Moderate, FinancialEnums.Creditworthiness.Yes},
        {FinancialEnums.Age.Under45, FinancialEnums.Education.HighSchoolDiploma, FinancialEnums.Status.Divorced, FinancialEnums.Income.Moderate, FinancialEnums.Creditworthiness.No},
        {FinancialEnums.Age.Above45, FinancialEnums.Education.ALevels, FinancialEnums.Status.Divorced, FinancialEnums.Income.High, FinancialEnums.Creditworthiness.No},
    };
}

public class FinancialEnums
{
    public enum Age
    {
        Under25,
        Under35,
        Under45,
        Above45,
    }

    public enum Education
    {
        ALevels,
        Bachelor,
        HighSchoolDiploma
    }

    public enum Status
    {
        Married,
        Divorced
    }

    public enum Income
    {
        Low,
        Moderate,
        High
    }

    public enum Creditworthiness
    {
        Yes,
        No
    }
}
#endregion