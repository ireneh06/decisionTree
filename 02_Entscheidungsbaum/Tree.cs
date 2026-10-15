using _05_Entscheidungsbaum;
using System.Linq;
//2-Gini Impurity-Implementation
/// <summary>
/// Class which generates a Tree based on Gini-Impurity
/// </summary>
/// <typeparam name="T"></typeparam>
public class Tree<T> where T : Enum
{
    Data data = new Data();

    public TreeNode<T> tree { get; set; } = new();
    private T[,] TrainingData { get; set; }
    private int FeatureCount { get; set; }
    private int DataSetCount { get; set; }

    private int LabelColumn { get; set; }
    private T[] FirstFeatureVector { get; set; }
    private Dictionary<string, double> FeatureGini { get; set; }
    private HashSet<(int, int)> LabelNeeded { get; set; }
    public Tree()
    {
        FeatureCount = data.trainingData.GetLength(1);
        DataSetCount = data.trainingData.GetLength(0);
        TrainingData = GetData();
        FirstFeatureVector = GetFeatureVector();
        LabelColumn = GetLabelColumn(); //Column where Label is
        FeatureGini = new(); //featurename, gini
        LabelNeeded = new(); //featurecolumnnumber, index of value -> if gini = 0, label is needed
    }

    public void Train()
    {
        List<string> previousNodes = new(); //previous nodes
        List<int> usedFeature = new(); //columnnumber of previous used features
        tree = CalcFeatureGini(previousNodes, usedFeature);


    }

    public void PrintTree()
    {
        tree.PrintTree();
    }

    public void Evaluate(T[] featureVector)
    {
        Console.WriteLine(tree.Evaluate(featureVector));
    }

    /// <summary>
    /// Builds the tree recursive
    /// </summary>
    /// <param name="previousNodes">Previous nodes</param>
    /// <param name="usedFeature">already used Features</param>
    /// <returns>TreeNode for the Tree</returns>
    public TreeNode<T> CalcFeatureGini(List<string> previousNodes, List<int> usedFeature)
    {
        //featurename, gini of next node
        KeyValuePair<string, double> next = GiniOfFeature(previousNodes, usedFeature);
        string nextFeature = next.Key;


        for (int i = 0; i < FeatureCount; i++)
        {
            if (i == LabelColumn)
                continue;
            if (FirstFeatureVector[i].GetType().Name.Equals(nextFeature))
            {
                //Add node to usedFeature
                usedFeature.Add(i);
                break;
            }
        }

        TreeNode<T> node = new();
        node.FeatureName = nextFeature;

        node.Children = new Dictionary<string, TreeNode<T>>();

        //get Values of current parent node
        string[] values = { };
        foreach (var name in FirstFeatureVector)
        {
            if (name.GetType().Name.Equals(node.FeatureName))
            {
                values = Enum.GetNames(name.GetType());
                break;
            }
        }

        int valueCounter = 0;
        foreach (var value in values)
        {
            previousNodes.Add(value);

            //if currentFeature, value needs a label or every Feature is used in tree-branch --> Label
            if (LabelNeeded.Contains((usedFeature.Last(), valueCounter))
                || usedFeature.Count == FeatureCount - 1)                                                                                               //ür value - featurevektor
            {
                TreeNode<T> nodeLabel = new();
                nodeLabel.FeatureName = GetLabel(previousNodes);

                node.Children.Add(value, nodeLabel);
            }
            else
            {
                //recursive call
                node.Children.Add(value, CalcFeatureGini(previousNodes, usedFeature));
            }

            valueCounter++;
            //Remove value to get to level above
            previousNodes.Remove(value);
        }
        usedFeature.RemoveAt(usedFeature.Count - 1);

        return node;
    }
    /// <summary>
    /// Calculates each gini of every feature with isnt already used
    /// </summary>
    /// <param name="previousNodes">List of already used Nodes e.x Sunny</param>
    /// <param name="usedFeature">int number of usedFeature in the DataSet</param>
    /// <returns>KeyValuePair of Feature with lowest Gini</returns>
    public KeyValuePair<string, double> GiniOfFeature(List<string> previousNodes, List<int> usedFeature)
    {
        FeatureGini.Clear();
        //Calc Gini for every feature exept Label
        for (int i = 0; i < FeatureCount; i++)
        {
            if (i == LabelColumn)
                continue;
            if (!usedFeature.Contains(i))
                GetNode(previousNodes, i);
        }
        //get feature with lowest gini
        var sortedFeatures = FeatureGini.OrderBy(x => x.Value);
        KeyValuePair<string, double> nextFeature = sortedFeatures.First();
        return nextFeature;
    }
    /// <summary>
    /// Adds the name of the feature and the calculated gini to the FeatureGini Dictionary
    /// </summary>
    /// <param name="previousNodes">List of used Nodes</param>
    /// <param name="nextFeature">Columnr of the used Feature in the given Dataset</param>
    public void GetNode(List<string> previousNodes, int nextFeature)
    {
        string currentFeature = FirstFeatureVector[nextFeature].GetType().Name;
        double gini = EvaluateFeatureGini(nextFeature, previousNodes);

        FeatureGini.Add(currentFeature, gini);
    }

    /// <summary>
    /// Gets the label of the Featurevector which contains search List
    /// </summary>
    /// <param name="search">Featurevector for which the label is needed</param>
    /// <returns>Label of the Featurevector</returns>
    private string GetLabel(List<string> search)
    {
        //transfer DataSet to list of featurevectors
        List<string[]> featureVectors = new();
        for (int k = 0; k < DataSetCount; k++)
        {
            string[] featureVector = new string[FeatureCount];
            for (int i = 0; i < FeatureCount; i++)
            {
                featureVector[i] = TrainingData[k, i].ToString();
            }
            featureVectors.Add(featureVector);
        }

        foreach (string[] item in featureVectors)
        {
            bool allSearchStringsFound = true;
            foreach (string searchString in search)
            {
                //check if featurevector contains ever searchstring
                if (!item.Contains(searchString))
                {
                    allSearchStringsFound = false;
                }
            }
            if (allSearchStringsFound)
            {
                return item[LabelColumn];
            }

        }
        //no label for featurevector
        return "no data";
    }

    /// <summary>
    /// Calculates Gini-Impurity for a Feature 
    /// </summary>
    /// <param name="featureNr">Nr of Feature in FirstfeatureVector</param>
    /// <param name="previousNodes">previous Parent nodes</param>
    /// <returns>double value of gini</returns>
    private double EvaluateFeatureGini(int featureNr, List<string> previousNodes)
    {
        int countOfValues = GetValueCountOfFeature(featureNr);
        double sumFeatureGinis = 0;
        //calculate gini for every value of feature
        for (int i = 0; i < countOfValues; i++)
        {
            sumFeatureGinis += MultiplyWithCount(featureNr, previousNodes, i);
        }
        return sumFeatureGinis;
    }
    /// <summary>
    /// Multiplies a part-Gini with the Count/DataSetCount
    /// </summary>
    /// <param name="featureNr">Nr of Feature in FirstfeatureVector e.g "Outlook"</param>
    /// <param name="previousNodes">List of search e.g Empty or "Sunny" if Sunny is Parent Node</param>
    /// <param name="i">Number of the Value from the Feature</param>
    /// <returns>Double value of Gini from a value (Gini of Sunny)</returns>
    private double MultiplyWithCount(int featureNr, List<string> previousNodes, int i)
    {
        //get enum value from featureVector
        string[] values = Enum.GetNames(FirstFeatureVector[featureNr].GetType());
        string currentValue = values[i];


        List<string> list = new(previousNodes);
        list.Add(currentValue);
        //count of rows which contain list
        double countOfValue = CountValueList(list);

        if (countOfValue == 0)
            return 1;

        double sum = CalculateValueGini(list, countOfValue);
        //Label is Needed when sum = 0
        if (sum == 0)
            LabelNeeded.Add((featureNr, i));

        return (countOfValue / CountValueList(previousNodes)) * sum;

    }

    /// <summary>
    /// Calculates the Part of Gini where 1-((Count.Yes/Count.Variable)² + (Count.No/Count.Variable)²)
    /// </summary>
    /// <param name="currentValue">current List of string with Values e.x "Sunny", "Hot",</param>
    /// <param name="countOfValue">calculated count of rows where Features are "Sunny", and "Hot"</param>
    /// <returns></returns>
    private double CalculateValueGini(List<string> currentValue, double countOfValue)
    {
        //LabelValues
        string[] values = Enum.GetNames(FirstFeatureVector[LabelColumn].GetType());

        List<string> checkLabel0 = new(currentValue);
        List<string> checkLabel1 = new(currentValue);

        checkLabel0.Add(values[0]);
        checkLabel1.Add(values[1]);
        //calc count of yes / count of vectors
        double subTotal1 = CountValueList(checkLabel0) / countOfValue;
        double subTotal2 = CountValueList(checkLabel1) / countOfValue;
        return (1 - (Math.Pow(subTotal1, 2) + Math.Pow(subTotal2, 2)));
    }

    /// <summary>
    /// Counts the Values of a Feature Outlook..3
    /// </summary>
    /// <param name="k">number of Feature in FeatureVector</param>
    /// <returns>Count of Values</returns>
    private int GetValueCountOfFeature(int k)
    {
        string[] valuesOfEnumType = Enum.GetNames(FirstFeatureVector[k].GetType());
        int attributCount = valuesOfEnumType.GetLength(0);

        return attributCount;
    }

    /// <summary>
    /// Counts the rows which contain all items of search List
    /// </summary>
    /// <param name="search">List of search e.g. "Rain", "High"</param>
    /// <returns>double value of count</returns>
    private double CountValueList(List<string> search)
    {
        //transfer DataSet to list of string[]
        List<string[]> featureVectors = new();
        for (int k = 0; k < DataSetCount; k++)
        {
            string[] featureVector = new string[FeatureCount];
            for (int i = 0; i < FeatureCount; i++)
            {
                featureVector[i] = TrainingData[k, i].ToString();
            }
            featureVectors.Add(featureVector);
        }
        int successfulSearch = 0;
        int count = 0;

        foreach (string[] item in featureVectors)
        {
            successfulSearch = 0;
            foreach (string searchString in search)
            {
                if (item.Contains(searchString))
                    successfulSearch++;
            }
            //if every searchString is found in the featureVector
            if (successfulSearch == search.Count)
                count++;
        }

        return count;
    }

    /// <summary>
    /// Gets the First row -> FeatureVector
    /// </summary>
    /// <returns>First row of Trainingdata as T[] </returns>
    private T[] GetFeatureVector()
    {
        T[] featureVector = new T[FeatureCount];
        for (int i = 0; i < FeatureCount; i++)
        {
            featureVector[i] = (T)TrainingData[0, i];
        }

        return featureVector;
    }

    /// <summary>
    /// Evaluates the Column where the Labels are and assigns it to the Property
    /// </summary>
    /// <exception cref="ArgumentException">If there are no Labels</exception>
    private int GetLabelColumn()
    {
        for (int i = 0; i < FeatureCount; i++)
        {
            string[] valuesOfFeature = Enum.GetNames(FirstFeatureVector[i].GetType());
            //evaluate if current column has Label values
            if (IsLabel(valuesOfFeature))
                return i;
        }
        throw new ArgumentException("No Label Column found!");
    }

    /// <summary>
    /// Checks if the Features values are a Label
    /// </summary>
    /// <param name="valuesOfFeature">Possible Values of selected Feature</param>
    /// <returns>If the feature Column is a label column</returns>
    private bool IsLabel(string[] valuesOfFeature)
    {
        //check if column is Label
        foreach (var item in valuesOfFeature)
        {
            if (!(item.ToLower() == "yes" || item.ToLower() == "no"
                || item == "1" || item == "0"
                || item.ToLower() == "false" || item.ToLower() == "true"))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Converts the Enum[,] array of training Data to T[,]
    /// so that it can be assigned to Property TrainingData
    /// </summary>
    /// <returns>Array of T</returns>
    private T[,] GetData()
    {
        T[,] resultArray = new T[DataSetCount, FeatureCount];

        for (int i = 0; i < DataSetCount; i++)
        {
            for (int k = 0; k < FeatureCount; k++)
            {
                resultArray[i, k] = (T)data.trainingData[i, k];
            }
        }
        return resultArray;
    }
}

/// <summary>
/// class which represents Tree
/// </summary>
/// <typeparam name="T"></typeparam>
public class TreeNode<T>
{
    public string? FeatureName { get; set; }
    public Dictionary<string, TreeNode<T>>? Children { get; set; }

    /// <summary>
    /// Prints tree in Console
    /// </summary>
    /// <param name="depth">depth of current node</param>
    /// <param name="isLastChild">if child is last one</param>
    public void PrintTree(int depth = 0)
    {
        
        string arrow = " -> ";

        Console.Write("".PadLeft(depth * 4));
        Console.Write(arrow);
        Console.ForegroundColor = ConsoleColor.Green;
        if (Children == null) //is a label
            Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(FeatureName);
        Console.ResetColor();

        if (Children != null)
        {
            foreach (var child in Children)
            {
                //identation to print tree
                Console.Write("".PadLeft((depth+1) * 4));
                Console.Write(arrow);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(child.Key); // Print Key value	
                Console.ResetColor();
                //recursive call                                     		
                child.Value.PrintTree(depth + 2);
            }
        }
    }

    /// <summary>
    /// Evaluates Label on a given FeatureVector
    /// </summary>
    /// <param name="featureVector">Given Vector</param>
    /// <param name="label"></param>
    /// <returns>string label</returns>
    public string Evaluate(T[] featureVector, string label = " ")
    {
        for (int i = 0; i < featureVector.Length; i++)
        {
            if (Children != null && featureVector[i] != null)
            {
                foreach (var child in Children)
                {
                    T feature = featureVector[i];
                    //if feature != null --> ToString
                    string? currentValue = feature != null ? feature.ToString() : null;


                    if (child.Key.ToString() == currentValue)
                    {
                        //set label to child.Value.Featurename
                        if (child.Value.FeatureName != null)
                        {
                            label = child.Value.FeatureName.ToString();
                        }
                        //recursive call
                        label = child.Value.Evaluate(featureVector, label);
                        break;
                    }
                }

            }
        }
        return label;
    }

}


