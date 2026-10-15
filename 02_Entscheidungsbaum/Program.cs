// See https://aka.ms/new-console-template for more information

using _05_Entscheidungsbaum;

Tree<Enum> giniTree = new();
giniTree.Train();
giniTree.PrintTree();
Console.WriteLine();

//GoTennis Evaluate
//Enum[] featureVector = { Enums.Outlook.Sunny, Enums.Humidity.Normal, Enums.Temperature.Cold, Enums.Wind.Weak };
//Console.Write("Given featureVector evaluates: ");

//string[] featureStrings = new string[featureVector.Length];
//for (int i = 0; i < featureVector.Length; i++)
//{
//    featureStrings[i] = featureVector[i].ToString();
//}
//Console.WriteLine(String.Join(", ", featureStrings));

//giniTree.Evaluate(featureVector);

//FinancialNums
Enum[] featureVector = { FinancialEnums.Age.Under35, FinancialEnums.Education.ALevels, FinancialEnums.Status.Married, FinancialEnums.Income.Low };
Console.WriteLine("Given featureVector evaluates: ");
string[] featureStrings = new string[featureVector.Length];
for (int i = 0; i < featureVector.Length; i++)
{
    featureStrings[i] = featureVector[i].ToString();
}
Console.WriteLine(String.Join(", ", featureStrings));
giniTree.Evaluate(featureVector);